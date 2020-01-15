using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class ResourceNode
    {
        public Transform resourceNodeTransform;

        private int resourceAmount;
        public ResourceNode(Transform resourceNodeTransform)
        {
            this.resourceNodeTransform = resourceNodeTransform;
            resourceAmount = 5;
        }
        public Vector2 GetPosition()
        {
            return resourceNodeTransform.position;
        }

        public void GrabResource()
        {
            resourceAmount -= 1;
            if (resourceAmount <= 0)
            {
                Debug.Log($"This resource is");
              //  resourceNodeTransform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.goldNodeDepletedSprite;
            }
            //CMDebug.TextPopupMouse("resourceAmount: "+resourceAmount);
        }

        public bool HasResources()
        {
            return resourceAmount > 0;
        }
    }
}