using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavigationMap
{
    public class PathRequestManager : MonoBehaviour
    {

        //      Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
        //      PathRequest currentPathRequest;

        Queue<PathResult> results = new Queue<PathResult>();
        static PathRequestManager instance;
        PathFinding pathFinding;
        //    bool isProcessingPath;


        void Awake()
        {
            instance = this;
            pathFinding = GetComponent<PathFinding>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (results.Count > 0)
            {
                int itemsInQuueue = results.Count;
                lock (results)
                {
                    for (int i = 0; i < itemsInQuueue; i++)
                    {
                        PathResult result = results.Dequeue();
                        result.callBack(result.path, result.success);
                    }
                }
            }
        }

        public void RequestPath(PathRequest request) // this should be static
        {
            ThreadStart threadStart = delegate
            {
                pathFinding.FindPath(request, FinishedProcessingPath);
            };
            threadStart.Invoke();
        }

        public void RequestSimulatePath(PathRequest request) // this should be static
        {
            ThreadStart threadStart = delegate
            {
                StopAllCoroutines();
                StartCoroutine(pathFinding.FindPathSimulation(request, FinishedProcessingPath));
            };
            threadStart.Invoke();
        }

        // void TryProcessNext()
        // {
        //     if (!isProcessingPath && PathRequestQueue.Count > 0)
        //     {
        //         currentPathRequest = PathRequestQueue.Dequeue();
        //         isProcessingPath = true;
        //         pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        //     }
        // }

        public void FinishedProcessingPath(PathResult result)
        {
            lock (results)
            {
                results.Enqueue(result);
            }
        }


    }


    public struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callBack;
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callBack)
        {
            pathStart = _start;
            pathEnd = _end;
            callBack = _callBack;
        }
    }


    public struct PathResult
    {
        public Vector3[] path;
        public bool success;
        public Action<Vector3[], bool> callBack;

        public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callBack)
        {
            this.path = path;
            this.success = success;
            this.callBack = callBack;
        }
    }
}
