using System.Collections;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
First Get nearest path to Stairs
while path is not on that floor
traverse to nearest stair
if is on the desired floor continue A* path // Call A* on that floor and disable other while storing paths


*/
namespace NavigationMap
{
    public class PathFinding : MonoBehaviour
    {
        public Transform seeker, target;
        FieldGrid grid;
    //    PathRequestManager requestManager;

        void Awake()
        {
            grid = this.GetComponent<FieldGrid>();
     //       requestManager = GetComponent<PathRequestManager>();
        }

        void Update()
        {
          //  if (Input.GetKeyDown(KeyCode.Return))
          //      FindPath(seeker.position, target.position);
        }

        //changing to IEnumerator
        public void FindPath(PathRequest request, Action<PathResult> callback) //public void FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            FieldNode startNode = grid.NodeFromFieldGrid(request.pathStart);
            FieldNode endNode = grid.NodeFromFieldGrid(request.pathEnd);


            Stopwatch sw = new Stopwatch();
            sw.Start();

            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;


            if (startNode.isPlaceable && endNode.isPlaceable)
            {
                Heap<FieldNode> openSet = new Heap<FieldNode>(grid.MaxSize); // List for unoptimized version
                HashSet<FieldNode> closedSet = new HashSet<FieldNode>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    FieldNode currentNode = openSet.RemoveFirst();
                    //openSet non Optimized
                    // for (int i = 1; i < openSet.Count; i++)
                    // {
                    //     if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    //     {
                    //         currentNode = openSet[i];

                    //     }
                    // } openSet.Remove(currentNode);

                    closedSet.Add(currentNode);

                    if (currentNode == endNode)
                    {
                        sw.Stop();
                        print(this.transform.name+" path Found " + sw.ElapsedMilliseconds + "ms");
                        pathSuccess = true;
                        // TracePath(startNode, endNode);
                        break;
                        // return;
                    }
                    foreach (FieldNode neighbor in grid.GetNeighbors(currentNode))
                    {
                        if (!neighbor.isPlaceable || closedSet.Contains(neighbor))
                        {
                            continue;
                        }
                        int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + neighbor.movementPenalty;
                        if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            neighbor.gCost = newMovementCostToNeighbor;
                            neighbor.hCost = GetDistance(neighbor, endNode);
                            neighbor.parent = currentNode;

                            if (!openSet.Contains(neighbor))
                            {
                                openSet.Add(neighbor);

                            }
                            else
                            {
                                openSet.UpdateItem(neighbor);
                            }

                        }
                    }
                }
            }
            if (pathSuccess)
            {
                waypoints = TracePath(startNode, endNode);

                pathSuccess = waypoints.Length > 0;
            }
            callback(new PathResult(waypoints,pathSuccess,request.callBack));
        }

        // public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
        // {
        //     StartCoroutine(FindPath(startPosition, targetPosition));
        // }

        Vector3[] TracePath(FieldNode startNode, FieldNode endNode)
        {
            List<FieldNode> path = new List<FieldNode>();
            FieldNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
         //   if (currentNode == startNode)
         //       path.Add(currentNode);
         //   path.Add(startNode);
            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
            //grid.path = path;

        }

        Vector3[] SimplifyPath(List<FieldNode> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;
        //    waypoints.Add(path[0].worldPosition); // Modified final Node
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionnew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (directionnew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);       // Modified  OLD: [i]
                    directionOld = directionnew;
                }
            }
            return waypoints.ToArray();
        }

        int GetDistance(FieldNode nodeA, FieldNode nodeB)
        {
            int xDistance = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int yDistance = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (xDistance > yDistance)
                return 14 * yDistance + 10 * (xDistance - yDistance);
            return 14 * xDistance + 10 * (yDistance - xDistance);

        }

    }
}               //openSet non Optimized
                // for (int i = 1; i < openSet.Count; i++)
                // {
                //     if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                //     {
                //         currentNode = openSet[i];

//     }
// }

// openSet.Remove(currentNode);