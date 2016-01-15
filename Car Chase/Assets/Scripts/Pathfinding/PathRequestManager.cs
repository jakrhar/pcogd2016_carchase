using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour
{
    public bool usePathSmoothing = false;
    private Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;
    private PathSmoothing pathSmoothing;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
        pathSmoothing = GetComponent<PathSmoothing>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newPathRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.PathRequestQueue.Enqueue(newPathRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && PathRequestQueue.Count > 0)
        {
            currentPathRequest = PathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindingPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    private Vector3[] originalPath;
    public void FinishedProcessingPath(Vector3[] path, bool success )
    {
        if (usePathSmoothing && pathSmoothing != null)
        {
            originalPath = path;
            path = pathSmoothing.PathAdjusting(path);
        }

        currentPathRequest.callback(path, success);

        isProcessingPath = false;
        TryProcessNext();
    }
    
    void OnDrawGizmos()
    {
        if (originalPath != null && usePathSmoothing)
        {
            for (int i = 1; i < originalPath.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(originalPath[i - 1], originalPath[i]);
            }
        }
    }
    

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback)
        {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
            callback = _callback;
        }
    }



}
