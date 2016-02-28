using UnityEngine;
using System.Collections;

public class PathControllerCivil : MonoBehaviour {

    private Transform target;
    public Transform[] targets;

    public float maxSpeed = 25.0f;
    public float maxTurningSpeed = 20.0f;
    public float waypointDistance = 3.0f;
    
    private int nextTarget = 1;
    
    public Light roofLight;

    private Rigidbody rb;

    Vector3[] path;
    int targetIndex;

    public float pathFindingIntervalInSeconds = 0.5f;
    private float timeElapsed = 0;
    void Start()
    {
        target = targets[nextTarget];

        rb = gameObject.GetComponent<Rigidbody>();
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

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
        targetIndex = 0; //new path -> reset index

        //find the waypoint that is closest to the object and set it as next
        for (int i = 1; i < path.Length; i++)
        {
            if (Vector3.Distance(transform.position, path[targetIndex]) > Vector3.Distance(transform.position, path[i]))
            {
                targetIndex = i;
            }
        }

        //if the found waypoint is further away than the police cars position from the end target
        //update to the next index
        if (Vector3.Distance(path[targetIndex], path[path.Length - 1]) > Vector3.Distance(transform.position, path[path.Length - 1]))
        {
            if (targetIndex < path.Length - 1)
            {
                targetIndex++;
            }   
        }

        Vector3 currentWayPoint = path[targetIndex];


        while (true)
        {
            //check if we are close to the target waypoint
            if (waypointDistance > Vector3.Distance(transform.position, currentWayPoint))
            //if (transform.position == currentWayPoint)
            {
                targetIndex++; //update index to next waypoint
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[targetIndex];
            }

            //stop if there are no waypoints left in the path
            if (targetIndex >= path.Length)
            {
                yield break;
            }

            var speed = maxSpeed;

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
        if (path != null && DrawPathGizmos)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;

                if (i == targetIndex)
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

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "PatrolPoint")
        {
            //print("PatrolPoint reached.");
            int tmp = 0;
            do
            {
                tmp = UnityEngine.Random.Range(0, targets.Length);
            }
            while (tmp == nextTarget);

            nextTarget = tmp;
        }
    }
}

