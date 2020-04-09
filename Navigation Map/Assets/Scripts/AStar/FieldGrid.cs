using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavigationMap
{
    public class FieldGrid : MonoBehaviour
    {
        public FieldNode[,] grid;
        public bool displayGridGizmos = false;
        public Transform player;      // Generate a Node Grid
        public Vector3 gridWorldSize; // Limit the grid size to the given playing field
        public float nodeRadius;       // Radius of every node so that each nodes will not collide to each other
        public LayerMask unPlaceableNode; // mask to know where not to place spawners ;) or nodes
        public int gridSizeX;              // size of the grid on X axis
        public int gridSizeY;              // size of the grid on Y axis

        public TerrainType[] walkableRegions;
        public int obstacleProximityPenalty = 10;
        LayerMask walkable;
        Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

        float nodeDiamater;         // diameter of a node

        int penaltyMin = int.MaxValue;
        int penaltyMax = int.MinValue;


        //this should be awake
        void Awake()
        {
            GenerateGrid();
        }

        public int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }
        public void GenerateGrid()
        {
            nodeDiamater = nodeRadius * 2;  // get the diameter of a Node
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiamater); // get the total nodes that can fit on grid X
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiamater); // get the total nodes that can fit on grid Y
                                                                          //after setting the size Create a Grid
            foreach (TerrainType region in walkableRegions)
            {
                walkable.value |= region.terrainMask.value;
                walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
            }
            CreateGrid();
        }

        void CreateGrid()
        {
            grid = new FieldNode[gridSizeX, gridSizeY];   // Instantiate a new Grid with x,y
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;    //calculate the most bottomleft coordinate
            Debug.Log($"Bottom Left Coordinates: {worldBottomLeft}");
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiamater + nodeRadius) + Vector3.forward * (y * nodeDiamater + nodeRadius);
                    bool isPlaceable = !(Physics.CheckSphere(worldPoint, nodeRadius, unPlaceableNode));
                    int movementPenalty = 0;
                    // raycast  all layers that has movement Penalty



                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, walkable))
                    {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                    if (!isPlaceable)
                    {
                        movementPenalty += obstacleProximityPenalty;
                    }

                    grid[x, y] = new FieldNode(isPlaceable, worldPoint, x, y, movementPenalty);
                }
            }
            BlurPenaltyMap(3);
        }

        public List<FieldNode> GetNeighbors(FieldNode node)
        {
            List<FieldNode> neighbors = new List<FieldNode>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbors.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbors;
        }


        public FieldNode GetRandomFieldNodePoint()
        {
            int gridX = Random.Range(0, gridSizeX);
            int gridY = Random.Range(0, gridSizeY);

            if (grid[gridX, gridY].isPlaceable)
            {
                Debug.Log("is Placeable????" + grid[gridX, gridY].isPlaceable);
                return grid[gridX, gridY];
            }
            else
            {
                GetRandomFieldNodePoint();
            }
            return grid[gridX, gridY];
        }

        public FieldNode NodeFromFieldGrid(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x, y];
        }

        public void BlurPenaltyMap(int blurSize)
        {
            int kernelSize = blurSize * 2 + 1;
            int kernelExtents = (kernelSize - 1) / 2;

            int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
            int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];
            // loop the grid horizontally
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int x = -kernelExtents; x <= kernelExtents; x++)
                {
                    int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                    penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;

                }
                for (int x = 1; x < gridSizeX; x++)
                {
                    int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                    int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);
                    penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;

                }
            }
            //
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = -kernelExtents; y <= kernelExtents; y++)
                {
                    int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                    penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];

                }
                int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
                grid[x, 0].movementPenalty = blurredPenalty;
                for (int y = 1; y < gridSizeY; y++)
                {
                    int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                    int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                    penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                    blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                    grid[x, y].movementPenalty = blurredPenalty;

                    if (blurredPenalty > penaltyMax)
                    {
                        penaltyMax = blurredPenalty;
                    }
                    if (blurredPenalty < penaltyMin)
                    {
                        penaltyMin = blurredPenalty;
                    }
                }
            }
        }
        public List<FieldNode> path = new List<FieldNode>();

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
            // // Gizmos.DrawWireSphere(transform.position, nodeDiamater);
            // if (OnlyDisplayPath)
            // {
            //     if (path != null)
            //     {
            //         foreach (FieldNode n in path)
            //         {
            //             Gizmos.color = Color.yellow;
            //             Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiamater - .1f));
            //         }
            //     }
            // }
            // else
            // {
            if (grid != null && displayGridGizmos)
            {
                foreach (FieldNode n in grid)
                {
                    FieldNode playerNode = NodeFromFieldGrid(player.position);
                    Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                    Gizmos.color = (n.isPlaceable) ? Gizmos.color : Color.red;
                    // if (path != null)
                    // {
                    //     if (path.Contains(n))
                    //         Gizmos.color = Color.yellow;
                    // }
                    // if (playerNode == n)
                    // {
                    //     Gizmos.color = Color.cyan;
                    // }
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiamater));

                }
            }
        }


    }
    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;

    }
}






