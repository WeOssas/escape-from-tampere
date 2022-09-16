using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerMovement : MonoBehaviour
{
    //References
    public Transform player;
    Rigidbody rb;

    //Variables and settings for jumping
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool jumpReady = true;

    public KeyCode jumpKey = KeyCode.Space;


    //Ground Checking
    public float groundDrag;

    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    //Player movement related
   
    public float moveSpeed;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    //Functions start here
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    private void Update()
    {
        //Is the player grounded?
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        MyInput();
        SpeedControl();
        
        rb.drag = grounded ? groundDrag : 0;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && jumpReady && grounded)
        {
            Debug.Log(jumpReady);
            
            jumpReady = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = player.forward * verticalInput + player.right * horizontalInput;

        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force );
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        jumpReady = true;
    }








}

