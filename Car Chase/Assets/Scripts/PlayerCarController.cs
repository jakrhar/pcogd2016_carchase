using UnityEngine;
using System.Collections;

public class PlayerCarController : MonoBehaviour {

    public float idealRpm = 500.0f;
    public float maxRpm = 1000.0f;

    public Transform centerOfGravity;

    public WheelCollider WheelFrontLeft;
    public WheelCollider WheelFrontRight;
    public WheelCollider WheelBackLeft;
    public WheelCollider WheelBackRight;

    public float turnRadius = 6.0f;
    public float torque = 25.0f;
    public float brakeTorque = 100.0f;
    public float antiRoll = 20000.0f;

    public float collisionBreakingTime = 1.5f;
    public float collisionBreakingTimeTorque = 4000f;

    private Rigidbody rb;

    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfGravity.localPosition;    
	}

    public float Speed()
    {
        return WheelBackRight.radius * Mathf.PI * WheelBackRight.rpm * 60.0f / 1000.0f; 
    }

    public float Rpm()
    {
        return WheelBackRight.rpm;
    }
	
	void FixedUpdate ()
    {
        float scaledTorque = Input.GetAxis("Vertical") * torque;

        if (WheelBackLeft.rpm < idealRpm)
        {
            scaledTorque = Mathf.Lerp(scaledTorque / 10.0f, scaledTorque, WheelBackLeft.rpm / idealRpm);
        }
        else
        {
            scaledTorque = Mathf.Lerp(scaledTorque, 0.0f, (WheelBackLeft.rpm - idealRpm ) / (maxRpm - idealRpm) );
        }

        DoRollBar(WheelFrontRight, WheelFrontLeft);
        DoRollBar(WheelBackRight, WheelBackLeft);

        WheelFrontRight.steerAngle = Input.GetAxis("Horizontal") * turnRadius;
        WheelFrontLeft.steerAngle = Input.GetAxis("Horizontal") * turnRadius;

        WheelFrontRight.motorTorque = scaledTorque;
        WheelFrontLeft.motorTorque = scaledTorque;
        WheelBackRight.motorTorque = scaledTorque;
        WheelBackLeft.motorTorque = scaledTorque;

        if (Input.GetButton("Fire1"))
        {
            WheelFrontRight.brakeTorque = brakeTorque;
            WheelFrontLeft.brakeTorque = brakeTorque;
            WheelBackRight.brakeTorque = brakeTorque;
            WheelBackLeft.brakeTorque = brakeTorque;
        }
        else
        {
            WheelFrontRight.brakeTorque = 0;
            WheelFrontLeft.brakeTorque = 0;
            WheelBackRight.brakeTorque = 0;
            WheelBackLeft.brakeTorque = 0;
        }
    }

    /// <summary>
    /// When player hits something make the wheel break so it is easier to continue to move again. 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player" && !isBoosting)
        {
            //start the speed boost
            StartCoroutine(Break());
        }
    }

    /// <summary>
    /// This method will increase the players speed temporarily. And then destroy the gameobject. 
    /// </summary>
    /// <returns></returns>
    IEnumerator Break()
    {
        WheelFrontRight.brakeTorque = collisionBreakingTimeTorque;
        WheelFrontLeft.brakeTorque = collisionBreakingTimeTorque;
        WheelBackRight.brakeTorque = collisionBreakingTimeTorque;
        WheelBackLeft.brakeTorque = collisionBreakingTimeTorque;

        yield return new WaitForSeconds(collisionBreakingTime);

        WheelFrontRight.brakeTorque = 0;
        WheelFrontLeft.brakeTorque = 0;
        WheelBackRight.brakeTorque = 0;
        WheelBackLeft.brakeTorque = 0;
    }

    void DoRollBar(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
        {
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }

        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
        {
            rb.AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);
        }

        if (groundedR)
        {
            rb.AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);
        }
    }
}
