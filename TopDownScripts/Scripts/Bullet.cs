using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        Destroy(this.gameObject, 5f);
    }
    public int damage = 20;
    Rigidbody2D rb;
    void OnCollisionEnter2D(Collision2D other)
    {
        rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Destroy(this.gameObject);
        }
        else{
            rb.velocity = Vector2.zero;
            Destroy(this.gameObject);
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.angularVelocity = 0f;
        }


    }


}
