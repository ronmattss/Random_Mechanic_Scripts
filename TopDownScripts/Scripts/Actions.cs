using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Actions : MonoBehaviour
    {
        public GameObject player;
        public Rigidbody2D rb;
        Shooting shooting;
        public float moveSpeed = 5f;
        Vector2 movement;
        Vector2 mousePos;
        public Vector2 lookDir;
        public Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            shooting = this.gameObject.GetComponent<Shooting>();
        }

        // Update is called once per frame
        void Update()
        {
            shooting.timer += Time.deltaTime;
            
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if(Input.GetButton("Fire1") && shooting.timer >= shooting.rateOfFire && Time.timeScale !=0)
            {
                shooting.Shoot();
            }
            

        }

        void FixedUpdate()
        {
           // rb.velocity = Vector2.zero;
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
            rb.velocity = movement * moveSpeed;
            lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
            Debug.Log("Velocity of Player: "+ rb.velocity);
        }
    }
}