using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class FieldGrid : MonoBehaviour
    {
        public FieldNode[,] grid;        // Generate a Node Grid
        public Vector3 gridWorldSize; // Limit the grid size to the given playing field
        public float nodeRadius;       // Radius of every node so that each nodes will not collide to each other
        public LayerMask unPlaceableNode; // mask to know where not to place spawners ;) or nodes

        float nodeDiamater;         // diameter of a node
        public int gridSizeX;              // size of the grid on X axis
        public int gridSizeY;              // size of the grid on Y axis


        void Start()
        {
            nodeDiamater = nodeRadius * 2;  // get the diameter of a Node
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiamater); // get the total nodes that can fit on grid X
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiamater); // get the total nodes that can fit on grid Y
                                                                          //after setting the size Create a Grid
            CreateGrid();
        }

        void CreateGrid()
        {
            grid = new FieldNode[gridSizeX, gridSizeY];   // Instantiate a new Grid with x,y
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;    //calculate the most bottomleft coordinate
            Debug.Log($"Bottom Left Coordinates: {worldBottomLeft}");
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiamater + nodeRadius) + Vector3.up * (y * nodeDiamater + nodeRadius);
                    Vector2 points = new Vector2(worldPoint.x, worldPoint.y);
                    bool isPlaceable = !(Physics2D.BoxCast(points, new Vector2(1, 1), 0, Vector2.up, 0.1f, unPlaceableNode));
                    grid[x, y] = new FieldNode(isPlaceable, worldPoint);
                }
            }
        }

        public FieldNode GetRandomFieldNodePoint()
        {
            return grid[Random.Range(0,gridSizeX),Random.Range(0,gridSizeY)];
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
            if (grid != null)
            {
                foreach (FieldNode n in grid)
                {
                    Gizmos.color = (n.isPlaceable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiamater - .1f));
                }
            }
        }





    }

}