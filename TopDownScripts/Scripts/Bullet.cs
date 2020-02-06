using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class Bullet : MonoBehaviour
    {
        void Update()
        {
            Destroy(this.gameObject, 5f);
        }
        public int damage = 60;
        Rigidbody2D rb;
        Status entity;
        void OnCollisionEnter2D(Collision2D other)
        {
            entity = other.gameObject.GetComponent<Status>();
            rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                rb.velocity = Vector2.zero;
                Destroy(this.gameObject);
                rb.isKinematic = true;
                rb.isKinematic = false;
                rb.angularVelocity = 0f;
            }
            if (entity != null && entity.healthPoints > 0)
            {

                entity.healthPoints -= damage;
                Debug.Log(other.gameObject.name + "HP: " + entity.healthPoints);
                Destroy(this.gameObject);

            }


        }


    }
}
