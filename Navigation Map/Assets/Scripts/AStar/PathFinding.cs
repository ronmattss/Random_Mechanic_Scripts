using System.Collections;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
This class contains A* algorithm
Note: Functions with Simulate is only used in the Simulation Scene
FindPath method is the A8 Algorithm
*/
namespace NavigationMap
{
    public class PathFinding : MonoBehaviour
    {
        public Transform seeker, target;
        public float timeScale = 0.5f;
        FieldGrid grid;

        void Awake()
        {
            grid = this.GetComponent<FieldGrid>();
        }

        void Update()
        {

        }

        //changing to IEnumerator
        // Main A* algo



        /*
     A* Search Algorithm From G4G
        1.  Initialize the open list
        2.  Initialize the closed list
    put the starting node on the open 
    list (you can leave its f at zero)

    3.  while the open list is not empty
     a) find the node with the least f on 
          the open list, call it "q"

      b) pop q off the open list
  
        c) generate q's 8 successors and set their 
            parents to q
   
     d) for each successor
        i) if successor is the goal, stop search
          successor.g = q.g + distance between 
                              successor and q
          successor.h = distance from goal to 
          successor 
          successor.f = successor.g + successor.h

        ii) if a node with the same position as 
            successor is in the OPEN list which has a 
           lower f than successor, skip this successor

        iii) if a node with the same position as 
            successor  is in the CLOSED list which has
            a lower f than successor, skip this successor
            otherwise, add  the node to the open list
     end (for loop)
  
    e) push q on the closed list
    end (while loop)         
        */
        public void FindPath(PathRequest request, Action<PathResult> callback) //public void FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            FieldNode startNode = grid.NodeFromFieldGrid(request.pathStart);
            FieldNode endNode = grid.NodeFromFieldGrid(request.pathEnd);


            Stopwatch sw = new Stopwatch();
            sw.Start();

            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            // check if startNode and endNode is in a placeable node
            if (startNode.isPlaceable && endNode.isPlaceable)
            {

                Heap<FieldNode> openSet = new Heap<FieldNode>(grid.MaxSize); // List for unoptimized version
                HashSet<FieldNode> closedSet = new HashSet<FieldNode>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    /*push q on the closed list
                     end */
                    FieldNode currentNode = openSet.RemoveFirst();


                    closedSet.Add(currentNode);

                    if (currentNode == endNode)
                    {
                        sw.Stop();
                        print(this.transform.name + " path Found " + sw.ElapsedMilliseconds + "ms");
                        pathSuccess = true;
                        // TracePath(startNode, endNode);
                        break;
                        // return;
                    }
                    // calculate gCost and hCost
                    // by nearest neighbor
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
            callback(new PathResult(waypoints, pathSuccess, request.callBack));
        }


        // Simplifies the lines VISUALLY
        Vector3[] TracePath(FieldNode startNode, FieldNode endNode)
        {
            List<FieldNode> path = new List<FieldNode>();
            FieldNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }
        Vector3[] SimulateTracePath(FieldNode startNode, FieldNode endNode)
        {
            List<FieldNode> path = new List<FieldNode>();
            FieldNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            Vector3[] waypoints = SimulateSimplePath(path);
            Array.Reverse(waypoints);
            return waypoints;
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

        // Change to IEnumerator

        public IEnumerator FindPathSimulation(PathRequest request, Action<PathResult> callback) //public void FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            FieldNode startNode = grid.NodeFromFieldGrid(request.pathStart);
            Spawn.instance.placeVisualCloseNode(startNode);
            yield return new WaitForSecondsRealtime(timeScale);
            FieldNode endNode = grid.NodeFromFieldGrid(request.pathEnd);
            Spawn.instance.placeVisualCloseNode(endNode);
            yield return new WaitForSeconds(timeScale);


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

                    closedSet.Add(currentNode);
                    Spawn.instance.placeVisualOpenNode(currentNode);
                    yield return new WaitForSeconds(timeScale);

                    if (currentNode == endNode)
                    {
                        sw.Stop();
                        print(this.transform.name + " path Found " + sw.ElapsedMilliseconds + "ms");
                        pathSuccess = true;
                        // foreach (FieldNode n in closedSet)
                        // {
                        //     Spawn.instance.placeVisualCloseNode(n);
                        //     yield return new WaitForSeconds(timeScale);
                        // }

                        // TracePath(startNode, endNode);
                        Spawn.instance.placeVisualCloseNode(endNode);
                        yield return new WaitForSeconds(timeScale);
                        break;
                        // return;
                    }
                    foreach (FieldNode neighbor in grid.GetNeighbors(currentNode))
                    {
                        if (!neighbor.isPlaceable || closedSet.Contains(neighbor))
                        {
                            Spawn.instance.placeVisualCloseNode(neighbor);
                            yield return new WaitForSeconds(timeScale);
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
                            Spawn.instance.placeVisualOpenNode(neighbor);
                            yield return new WaitForSeconds(timeScale);

                        }

                    }
                }
            }
            if (pathSuccess)
            {
                waypoints = SimulateTracePath(startNode, endNode);
                Spawn.instance.placeVisualPathNode(startNode);
                yield return new WaitForSeconds(timeScale);
                pathSuccess = waypoints.Length > 0;
                foreach (Vector3 waypoint in waypoints)
                {
                    Spawn.instance.placeVisualPathNode(waypoint);
                    yield return new WaitForSeconds(timeScale);

                }
                Spawn.instance.placeVisualPathNode(endNode);
                yield return new WaitForSeconds(timeScale);

            }
            callback(new PathResult(waypoints, pathSuccess, request.callBack));
            yield return null;
        }


        Vector3[] SimulateSimplePath(List<FieldNode> path)
        {
            List<Vector3> waypoints = new List<Vector3>();

            for (int i = 1; i < path.Count; i++)
            {
                waypoints.Add(path[i].worldPosition);       // Modified  OLD: [i]
            }
            return waypoints.ToArray();
        }

        //Get Distance between nodeA and Nodeb
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