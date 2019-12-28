using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    [System.NonSerialized]
    public float timer;

    public Transform firePoint;
    public float rateOfFire = 0.15f;
    public float bulletForce = 20f;
    public Vector2 randomLoc;
    public float offset;
    float minOffset;
    [Range(0, 180)]
    public float maxOffset = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //What is new?
    // Shooting now has a slight random spread to simulate inaccuracy
    // How it works
    // given a vector, this vector is converted to an angle then add a random offset then converted back to vector
    public void Shoot()
    {
        minOffset = maxOffset * -1f;
        timer = 0f;
        offset = Random.Range(minOffset, maxOffset);
        float angle;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Vector2 direction = bullet.transform.up;
        angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
        angle += offset;

        direction.x = Mathf.Cos(angle * Mathf.Deg2Rad);
        direction.y = Mathf.Sin(angle * Mathf.Deg2Rad);

        bullet.transform.up = direction;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

     //   Debug.Log("Bullet direction Vector: " + direction + " Angle: " + angle);
    //    Debug.Log(direction.x + " " + direction.y);

        rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);

    }


    public void Fire()
    {
        timer += Time.deltaTime;
        if (timer >= rateOfFire && Time.deltaTime != 0)
        {
            Shoot();
        }

    }
}
