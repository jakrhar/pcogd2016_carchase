using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float maxSpeed = 50.0f;
    public float inputDelay = 0.1f;
    public float horzePower = 12.0f;
    public float turnSpeed = 99.1f;

    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput = 0;
    float turnInput = 0;


    // Use this for initialization
    void Start ()
    {
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("Rigidbody required.");
        }

	}

    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(turnSpeed * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        Turn();
	}

    void FixedUpdate()
    {
        //move
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * forwardInput * horzePower);
        }
    }


}
