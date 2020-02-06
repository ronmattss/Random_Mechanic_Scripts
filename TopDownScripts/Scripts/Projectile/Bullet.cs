using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerScripts;

public class Bullet : MonoBehaviour
{
    public Projectile projectile;

    public int damage = 20;
    Status entity;
    void OnCollisionEnter2D(Collision2D other)
    {
        entity = other.gameObject.GetComponent<Status>();
        Debug.Log(other.gameObject.name);

        if (entity != null && entity.healthPoints > 0)
        {
            entity.healthPoints -= damage;
            Debug.Log(other.gameObject.name + "HP: " + entity.healthPoints);
            Destroy(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject,5f);

    }


}
