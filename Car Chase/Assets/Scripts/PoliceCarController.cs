using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoliceCarController : MonoBehaviour, IAdjustDifficulty
{

    public Transform target;
    public Transform[] targets;

    //character
    //private CharacterController controller;
    private int difficultyLevel = 1;
    public float maxDifficultySpeedIncrease = 300.0f;
    public float maxSpeed = 25.0f;
    public float maxTurningSpeed = 20.0f;
    public float waypointDistance = 3.0f;
    
    public bool patrolMode = true;
    private int nextTarget = 1;
    public float radarDistance = 100.0f;
    public float chasingDistance = 150.0f;
    public Light roofLight;

    private Rigidbody rb;
    private int nextControlPoint = 1;

    private Vector3[] path;
    private int index = 0; //index of the path[]

    public float pathFindingIntervalInSeconds = 0.5f;
    private float timeElapsed = 0;
    

    void Start()
    {
        nextControlPoint = nextTarget;
        target = targets[nextTarget];

        rb = gameObject.GetComponent<Rigidbody>();
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        StateMachine();

        if (timeElapsed >= pathFindingIntervalInSeconds)
        {
            target = targets[nextTarget];
            //print("time elapsed - inside: " + timeElapsed + "ms");
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            timeElapsed = 0;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessfull)
    {
        if (pathSuccessfull)
        {
            
            StopCoroutine("FollowPath");
            path = newPath;

            if (path.Length > 0)
            {
                StartCoroutine("FollowPath");
            }
        }
    }

    IEnumerator FollowPath()
    {
        index = 0; //new path -> reset index

        //find the waypoint that is closest to the object and set it as next
        for (int i = 1; i < path.Length; i++)
        {
            if (Vector3.Distance(transform.position, path[index]) > Vector3.Distance(transform.position, path[i]))
            {
                index = i;
            }
        }

        //if the found waypoint is further away than the police cars position from the end target
        //update to the next index
        if (Vector3.Distance(path[index], path[path.Length - 1]) > Vector3.Distance(transform.position, path[path.Length - 1]))
        {
            if (index < path.Length - 1)
            {
                index++;
            }   
        }

        Vector3 currentWayPoint = path[index];


        while (true)
        {
            //check if we are close to the target waypoint
            if (waypointDistance > Vector3.Distance(transform.position, currentWayPoint))
            //if (transform.position == currentWayPoint)
            {
                index++; //update index to next waypoint
                if (index >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[index];
            }

            //stop if there are no waypoints left in the path
            if (index >= path.Length)
            {
                yield break;
            }

            var speed = maxSpeed + difficultyLevel * 0.01f * maxDifficultySpeedIncrease;

            var moveDirection = new Vector3(currentWayPoint.x - transform.position.x, 0, currentWayPoint.z - transform.position.z);
            moveDirection.Normalize();
            moveDirection *= speed;

            if (moveDirection != Vector3.zero)
            {
                if (rb.velocity.magnitude > 5)
                {
                    transform.rotation = Quaternion.Slerp(
                                transform.rotation,
                                Quaternion.LookRotation(rb.velocity * 20.0f),
                                Time.deltaTime * maxTurningSpeed
                                );
                }
            }

            rb.AddForce(moveDirection);

            yield return null;
        }
    }

    public bool DrawPathGizmos;

    void OnDrawGizmos()
    {
        if (path != null && path.Length > 1 && DrawPathGizmos)
        {
            for (int i = index; i < path.Length; i++)
            {
                Gizmos.color = Color.black;

                if (i == index)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    public void StateMachine()
    {
        if (patrolMode)
        {
            //we patrolling -> if we are close to the player -> change mode 
            if (radarDistance > Vector3.Distance(transform.position, targets[0].transform.position))
            {
                //print("patrolMode -> chasing");
                nextTarget = 0;
                patrolMode = false;
            }
        }
        else
        {
            //print("chasing mode");
            //we are in chasing mode
            if (chasingDistance < Vector3.Distance(transform.position, targets[0].transform.position))
            {
                //print("chasing -> patrolMode");
                nextTarget = nextControlPoint;
                patrolMode = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "PatrolPoint")
        {
            //print("PatrolPoint reached.");
            int tmp = 0;
            do
            {
                tmp = UnityEngine.Random.Range(1, targets.Length - 1);
            }
            while (tmp == nextTarget);

            nextControlPoint = nextTarget = tmp;
        }
    }

    //implementation of IAdjustDifficulty interface methods

    /// <summary>
    /// Increases the maximun speed if the police car when difficulty is increased
    /// </summary>
    /// <param name="percentage"></param>
    public void AdjustDifficulty(int percentage)
    {
        difficultyLevel += percentage;
        //Debug.Log("Police car maxSpeed adjusted" + percentage);
        CheckDifficultyBoundaries();

    }

    /// <summary>
    /// Sets the difficultu level, adjust the maximun speed of the police car
    /// </summary>
    /// <param name="percentage"></param>
    public void SetDifficultyLevel(int percentage)
    {
        difficultyLevel = percentage;
        CheckDifficultyBoundaries();
    }

    private void CheckDifficultyBoundaries()
    {
        if (difficultyLevel < 0)
        {
            difficultyLevel = 0;
        }

        if (difficultyLevel > 100)
        {
            difficultyLevel = 100;
        }
    }
}

