using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavigationMap
{
    public class FieldNode : IHeapItem<FieldNode>
    {

        public bool isPlaceable;
        public Vector3 worldPosition;

        public int gCost;
        public int hCost;
        public int gridX;
        public int gridY;
        public int movementPenalty;
        int heapIndex;

        public FieldNode parent;


        public FieldNode(bool _isPlaceable, Vector3 _worldPos, int _x, int _y,int _movementPenalty)
        {
            isPlaceable = _isPlaceable;
            worldPosition = _worldPos;
            gridX = _x;
            gridY = _y;
            movementPenalty = _movementPenalty;
        }
        public FieldNode()
        {

        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(FieldNode other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }
}