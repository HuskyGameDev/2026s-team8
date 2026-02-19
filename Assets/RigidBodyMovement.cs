using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    private Rigidbody rb;
    private Vector3 moveInput;
    bool canJump = true; 
  
    public float pauseDuration = 2f; // seconds to wait after tipping too far
    public float tiltTimer = 0f; // countdown
    bool isFallen = false;
    public float forced = 25f;
    public float moveTilt = 2f;

    

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

    if (tiltTimer > 0) // if the timer has started
    {
        tiltTimer -= Time.deltaTime; // decrement until it hits 0
    }


    }

    void FixedUpdate() // runs every x seconds regardless of frame rate
    {
        Vector3 targetForce;
        
        if(tiltTimer <= 0 && moveInput != Vector3.zero) //make sure the player is not fallen and is moving
        {
            Vector3 tiltAxis = Vector3.Cross(Vector3.up, moveInput); //get the axis to rotate the player around
            rb.AddTorque(tiltAxis * moveTilt * rb.linearVelocity.magnitude, ForceMode.Acceleration);
        }

        float angle = Vector3.Angle(transform.up, Vector3.up); // find the angle between world up and player up (how much the player is tilted)
        if (angle > 30f && !isFallen) // if the player tilts too much and they are not already tipped over
{
    // the player has fallen and cannot get up for 2 seconds
    isFallen = true;
    tiltTimer = pauseDuration;
}

// once timer is done and the player is upright, clear the fallen state
if (isFallen && tiltTimer <= 0 && angle < 15f)
{
    isFallen = false;
}

        if(tiltTimer > 0) // dont let the player move if they have fallen
        {
            targetForce = Vector3.zero;
        }

        else {
        targetForce = moveInput * moveSpeed; // scale direction with speed
        }
        rb.AddForce(targetForce, ForceMode.Acceleration);

        // if the player falls they automatically right themselves
        if(tiltTimer <= 0) {

            if(Physics.Raycast(transform.position, Vector3.down, 1.1f)) { //find if the player is hitting the ground, if they are allow jumping
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    // right the player if they fall or tilt
    Vector3 uprightAxis = Vector3.Cross(transform.up, Vector3.up);
    float tiltAmount = uprightAxis.magnitude;
    rb.AddTorque(uprightAxis.normalized * forced * tiltAmount, ForceMode.Acceleration);
    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, 0.1f);
}
else
        {
            canJump = false;
        }


    }
}