using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    enum State
    {
        Idle, Moving, Animating,
    }
    public class ResourceGathererUnit : MonoBehaviour, IUnit
    {

        private Vector3 targetPosition;
        private float stopDistance;
        private Action onArrivedAtPosition;
        private State state;
        private MovementAI moveAI;
        private const float speed = 30f;
        private Vector2 thisTransform;

        void Awake()
        {
            thisTransform = transform.position;
            moveAI = gameObject.GetComponent<MovementAI>();
        }
        public bool IsIdle()
        {
            return state == State.Idle;
        }


        void Update()
        {
            switch (state)
            {
                case State.Idle:
                    // animation 
                    break;
                case State.Moving:
                    HandleMovement();
                    break;
                case State.Animating:
                    // animation
                    break;

            }
        }

        private void HandleMovement()
        {
             if (Vector2.Distance(thisTransform, targetPosition) > stopDistance)
             {
                 Vector3 moveDir = (targetPosition - transform.position).normalized;

                 float distanceBefore = Vector2.Distance(transform.position, targetPosition);
                 moveAI.target = targetPosition;
                 // animation
                // this.transform.position = transform.position + moveDir * speed * Time.deltaTime;
             }
               else
              {
            // Arrived
            // animation
            if (onArrivedAtPosition != null)
            {
                Action tmpAction = onArrivedAtPosition;
                onArrivedAtPosition = null;
                state = State.Idle;
                this.transform.position = moveAI.target;
                tmpAction();
            }
             }
        }

        //moves to target position using A*
        public void MoveTo(Vector3 position)
        {
            SetTargetPosition(position);
            state = State.Moving;
            Debug.Log(position);
            moveAI.target = position;
        }

        //sets the target position Nodes/Storage
        public void SetTargetPosition(Vector3 targetPosition)
        {
            // uncomment for vector 3
            // targetPosition.z = 0f;
            this.targetPosition = targetPosition;
        }

        
    }
}
