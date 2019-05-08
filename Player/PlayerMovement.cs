using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float runSpeed  = 6f;
    public float RotationSpeed = 1;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    Transform player;
    Quaternion targetRotation;

    float up = -90;
    float down = 90;
    float right = 0;
    float left = 180;
    float upRight = -45;
    float upLeft = 135;
    float downRight = 45;
    float downLeft = -135;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");

        anim = GetComponent<Animator>();
        player = GetComponent<Transform>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.LeftShift))
        Move(h, v, runSpeed);
        else
        {
            Move(h,v, walkSpeed);
        }

        Turning();
        
        SecondTurning();

        Animating(h, v);

    }


    void Move(float h, float v,float speed)
    {
        
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);

    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;

            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            playerRigidbody.MoveRotation(newRotation);
        }
    }

    //Use this for none Raycast Mouse Turning
    void SecondTurning()
    {
         if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            targetRotation = Quaternion.Euler(0, upLeft, 0);
        
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            targetRotation = Quaternion.Euler(0, downLeft, 0);
       
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            targetRotation = Quaternion.Euler(0, downRight, 0);

        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            targetRotation = Quaternion.Euler(0, upRight, 0);

        else if (Input.GetKey(KeyCode.A))
            targetRotation = Quaternion.Euler(0, up, 0);

        else if (Input.GetKey(KeyCode.D))
            targetRotation = Quaternion.Euler(0, down, 0);

        else if (Input.GetKey(KeyCode.W))
            targetRotation = Quaternion.Euler(0, right, 0);

        else if (Input.GetKey(KeyCode.S))
            targetRotation = Quaternion.Euler(0, left, 0);
          


        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * RotationSpeed);

    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;

        anim.SetBool("IsWalking", walking);
        
        anim.SetBool("IsRunning" , Input.GetKey(KeyCode.LeftShift));
        

    }
}
