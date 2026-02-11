using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    private Rigidbody rb;
    private Vector3 moveInput;
    bool canJump = true;
    float force = 5; // up force
    float offset = 2; // offset from object's position (up direction)
    float selfRightCooldown = 0f; // time remaining until self-righting is allowed
    public float pauseDuration = 2f; // seconds to wait after tipping too far


    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from WASD or Arrow keys
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(moveX, 0f, moveZ).normalized; // direction of movement

    if(canJump) {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * 7f, ForceMode.Impulse); 
        }
    }
    }

    void FixedUpdate() // runs every x seconds regardless of frame rate
    {
        Vector3 targetForce;
        float tilt = transform.eulerAngles.z;
        
        if(Physics.Raycast(transform.position, Vector3.down, 1.1f)) { //find if the player is hitting the ground, if they are allow jumping
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if(transform.eulerAngles.z > Mathf.Abs(30))
        {
            targetForce = Vector3.zero;
        }
        else {
        targetForce = moveInput * moveSpeed; // scale direction with speed
        }
        rb.AddForce(targetForce, ForceMode.Acceleration);

        // if the player falls they automatically right themselves
        var point = transform.TransformPoint(offset * Vector3.up); 
        rb.AddForceAtPosition(force * Vector3.up, point);
    }
}