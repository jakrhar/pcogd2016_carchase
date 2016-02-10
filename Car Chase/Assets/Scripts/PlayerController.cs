using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed = 25.0f;
    public float maxTurningSpeed = 20.0f;

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        moveDirection *= maxSpeed;

        if (moveDirection != Vector3.zero)
        {
            if (rb.velocity.magnitude > 1)
            {
                transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            Quaternion.LookRotation(rb.velocity * 20.0f),
                            Time.deltaTime * maxTurningSpeed
                            );
            }
        }

        rb.AddForce(moveDirection);
    }
}
