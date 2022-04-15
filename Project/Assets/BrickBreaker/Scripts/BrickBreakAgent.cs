using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BrickBreakAgent : Agent
{
    public Transform Target;
    public BrickManager brickManager;
    public float forceMultiplier = 4;

    private Rigidbody rBody;
    public Rigidbody targetRB;

    bool hitBall = false;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if (Target.transform.position.y < -5 || brickManager.BricksRemaining() <= 0)
        {
            // reset paddle position
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, -4, 0);

            // Reset Ball
            Target.localPosition = Vector3.zero;
            targetRB.velocity = new Vector3(Random.value * 8 - 4, Random.value * -4 - 4, 0);

            brickManager.SetBricks();
        }
        hitBall = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.x);

        // Ball evlocity
        sensor.AddObservation(targetRB.velocity.x);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 1
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        rBody.velocity = (controlSignal * forceMultiplier);

        // Reached target
        if (hitBall || brickManager.BricksRemaining() <= 0)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Ball hit Floor
        else if (Target.transform.position.y < -5)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            hitBall = true;
        }
    }
}
