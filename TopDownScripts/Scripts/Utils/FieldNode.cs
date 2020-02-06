using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class FieldNode
    {

        public bool isPlaceable;
        public Vector3 worldPosition;

        public FieldNode(bool _isPlaceable, Vector3 _worldPos)
        {
            isPlaceable = _isPlaceable;
            worldPosition = _worldPos;
        }
        public FieldNode()
        {

        }
    }
}