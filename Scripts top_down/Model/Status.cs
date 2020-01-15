using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerScripts;
namespace PlayerScripts
{
    //base properties for all entities
    //base class for all Entities that will have health and armor
    // This will also handle Status effects
    public class Status : MonoBehaviour
    {
        public int healthPoints;
        public float armor;

        void Update()
        {
            if(healthPoints <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("dish should be dead");
            }
            else
            {

            }
        }

    }
}