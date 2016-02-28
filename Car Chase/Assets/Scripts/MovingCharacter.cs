using UnityEngine;
using System.Collections;

public class MovingCharacter : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocityVector;
    private Vector3 lastDirection = Vector3.forward;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection.Normalize();

            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
            
            
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        if (!(moveDirection.x == 0 && moveDirection.z == 0))
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            lastDirection = moveDirection;
        }
        else
          
            transform.rotation = Quaternion.LookRotation(lastDirection);
        

    }
}
