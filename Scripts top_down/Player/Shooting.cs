using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{

    //This script will contain  weapon behavior which will be used in the Shoot function
    public class Shooting : MonoBehaviour
    {

        [System.NonSerialized]
        public float timer;
        public Transform firePoint;
        public WeaponManager weaponManager;
        public GameObject bulletPrefab;
        public float rateOfFire = 0.15f;
        Projectile projectile;
        public float offset;
        float minOffset;
        [Range(0, 180)]
        public float maxOffset = 10f;
        public float bulletForce = 20f;
        public int currentAmmo;
        public int clipSize;
        public int maxAmmo;

        void Start()
        {
            maxAmmo = weaponManager.currWeapon.weaponProperties.maxAmmo;
            clipSize = weaponManager.currWeapon.weaponProperties.clipSize;
            currentAmmo = weaponManager.currWeapon.weaponProperties.clipSize;
        }


        // Update is called once per frame
        void Update()
        {



        }

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
            currentAmmo--;

        }
        public void TurretShoot()
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
            currentAmmo--;

        }

        public void Reload()
        {
            int toReload = 0;

            if (currentAmmo < clipSize && maxAmmo > 0) // 14/15 cs = 30 
            {
                toReload = clipSize - currentAmmo;    // tR = 30 - 14 = 16
                if (toReload > maxAmmo)         // 16 > 15
                {
                    toReload = maxAmmo;         //tR = 15   
                }                                // 14 + 15 = 29
                maxAmmo -= toReload;           //15 - 15 
                currentAmmo += toReload;       // 

            }

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
}
