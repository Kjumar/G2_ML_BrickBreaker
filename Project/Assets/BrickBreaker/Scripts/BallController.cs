using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;

    private int flipX = 1;
    private int flipY = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.velocity.x) <= 1)
        {
            rb.velocity = new Vector3(2 * flipX, rb.velocity.y, 0);
            flipX = flipX * -1;
        }

        if (Mathf.Abs(rb.velocity.y) <= 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, 2 * flipY, 0);
            flipY = flipY * -1;
        }
    }
}
