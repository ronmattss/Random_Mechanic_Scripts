using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerScripts
{
    public class Actions : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float sprintSpeed = 10f;
        float currSpeed;
        public Camera cam;
        public Rigidbody2D rb;
        Shooting shooting;


        Vector2 movement;
        Vector2 mousePos;
        void Start()
        {
            currSpeed = moveSpeed;
            shooting = this.gameObject.GetComponent<Shooting>();
        }

        // Update is called once per frame
        void Update()
        {
            shooting.timer += Time.deltaTime;
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetButton("Fire1") && shooting.currentAmmo > 0 && shooting.timer >= shooting.rateOfFire && Time.timeScale != 0)
            {
                shooting.Shoot();

            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                currSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, 1);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currSpeed = Mathf.Lerp(sprintSpeed, moveSpeed, 5);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                shooting.Reload();
            }

            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        void FixedUpdate()
        {
            rb.MovePosition(rb.position + movement * currSpeed * Time.deltaTime);

            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;

        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Walls")
            {
                rb.velocity = Vector2.zero;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.tag == "Walls")
            {
                rb.velocity = Vector2.zero;
            }
        }
    }


}
