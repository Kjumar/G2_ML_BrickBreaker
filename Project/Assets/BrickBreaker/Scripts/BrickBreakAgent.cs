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
    public float forceMultiplier = 10;

    private Rigidbody rBody;
    public Rigidbody targetRB;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // reset paddle position
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, -4, 0);

        // Reset Ball
        Target.localPosition = Vector3.zero;
        targetRB.velocity = new Vector3(Random.value * 4 - 2, Random.value * -2 - 2, 0);

        brickManager.SetBricks();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

        // Ball evlocity
        sensor.AddObservation(targetRB.velocity.x);
        sensor.AddObservation(targetRB.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 1
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Reached target
        if (brickManager.BricksRemaining() <= 0)
        {
            SetReward(brickManager.GetReward());
            EndEpisode();
        }

        // Ball hit Floor
        else if (Target.transform.localPosition.y < -5)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }
}
