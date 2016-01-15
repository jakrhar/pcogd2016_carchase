﻿using UnityEngine;
using System.Collections;

public class PathControllerCivil : MonoBehaviour {

    private Transform target;
    public Transform[] targets;

    public float maxSpeed = 25;
    public float forceMultiplier = 1.0f;
    public float minForce = 10;
    public float maxForce = 100;
    public int maxTurningSpeed = 6;
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
            path = newPath;
            StopCoroutine("FollowPath");

            if (path.Length > 0)
            {
                StartCoroutine("FollowPath");
            }
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWayPoint = path[0];

        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }

                currentWayPoint = path[targetIndex];
            }

            if (rb.velocity.magnitude < maxSpeed)
            {
                var forceDirection =
                    new Vector3(currentWayPoint.x - transform.position.x, 0, currentWayPoint.z - transform.position.z) *
                    forceMultiplier * Time.deltaTime;

                var magnitude = forceDirection.magnitude;

                if (magnitude < minForce)
                {
                    forceDirection = forceDirection * (minForce / magnitude);
                }

                if (magnitude > maxForce)
                {
                    forceDirection = forceDirection * (magnitude / maxForce);
                }
                if (forceDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(forceDirection),
                        Time.deltaTime * maxTurningSpeed
                        );
                }
                rb.AddForce(forceDirection);
            }
            //transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
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
                tmp = Random.Range(0, targets.Length - 1);
            }
            while (tmp == nextTarget);

            nextTarget = tmp;
        }
    }
}

