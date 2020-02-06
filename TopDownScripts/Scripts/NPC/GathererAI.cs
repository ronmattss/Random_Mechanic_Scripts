using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class GathererAI : MonoBehaviour
    {
        // AI has states
        // isIdle
        // isMoving
        // isHarvesting
        // isDelivering
        private enum State
        {
            Idle,
            MovingToResourceNode,
            GatheringResources,
            MovingToStorage,
        }
        [SerializeField] private Transform resourceNodePosition;
        [SerializeField] private Transform storageNodePosition;

        private IUnit unit;
        [SerializeField] private State state;
        private ResourceNode resourceNode;
        private Transform storageTransform;
        [SerializeField] private int resourceAmount;


        void Awake()
        {

            unit = this.gameObject.GetComponent<IUnit>();
            state = State.Idle;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            switch (state)
            {
                case State.Idle:
                    resourceNode = GameHandler.GetResourceNode_Static();
                    state = State.MovingToResourceNode;

                    break;
                case State.MovingToResourceNode:

                    Debug.Log("Moving to Resource Node");
                    unit.MoveTo(resourceNode.GetPosition());
                    Debug.Log(resourceNode.GetPosition());

                    break;
                case State.GatheringResources:

                    Debug.Log("Unit is Idle");
                    if (resourceAmount >= 3)
                    {
                        Debug.Log("Delivering Resource");
                        //Move to storage
                        storageTransform = GameHandler.GetStorage_Static();
                        state = State.MovingToStorage;
                    }
                    else
                    {
                        Debug.Log("Gathering Resource");
                        //some animation + goldInventoryamount
                        resourceAmount++;
                    }

                    break;
                case State.MovingToStorage:

                    unit.MoveTo(storageTransform.position);

                    break;

            }

        }


        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.tag);
            if (other.CompareTag("ResourceNode") && resourceAmount < 3)
            {
                resourceAmount = 3;
                state = State.GatheringResources;
            }
            else if (other.CompareTag("StorageNode"))
            {
                resourceAmount = 0;
                state = State.Idle;
            }
        }

        // void OnTriggerStay2D(Collider2D other)
        // {
        //  Debug.Log(other.tag);
        //     if (other.CompareTag("ResourceNode") && resourceAmount < 3)
        //     {
        //         state = State.GatheringResources;


        //         resourceAmount++;
        //     }
        //     else if (resourceAmount >= 3 && other.CompareTag("ResourceNode"))
        //     {
        //         Debug.Log("MovingToStorage");
        //         storageTransform = GameHandler.GetStorage_Static();
        //         state = State.MovingToStorage;
        //     }
        //     else if (other.CompareTag("StorageNode"))
        //     {
        //         Debug.Log(state);
        //         state = State.Idle;
        //     }
        // }
    }
}