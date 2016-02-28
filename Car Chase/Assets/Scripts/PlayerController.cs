using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed = 25.0f;
    public float maxTurningSpeed = 20.0f;

    private Rigidbody rb;
    private Vector3 lastDirection = Vector3.down;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        moveDirection *= maxSpeed;

        //transform.rotation = Quaternion.Slerp(
        //                transform.rotation,
        //                Quaternion.LookRotation(rb.velocity * 20.0f),
        //                Time.deltaTime * maxTurningSpeed
        //                );

        if (!(moveDirection.x == 0 && moveDirection.z == 0))
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            lastDirection = moveDirection;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(lastDirection);
        }

        transform.Translate(moveDirection * Time.deltaTime);
    }
}
