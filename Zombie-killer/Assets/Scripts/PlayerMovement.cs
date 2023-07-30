using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    public float walkSpeed;
    public float runningSpeed;
    public float groundDrag;
    private float playerSpeed;

    [Header("Jump")]
    public float jumpForce;
    private bool isJumping;
    public float airMultiplier;

    [Header("Another stuff")]
    public Transform orientation;
    public Animator animator;
    
    [Header("Ground Check")] 
    public LayerMask whatIsGround;
    public bool grounded;
    
    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;
    public bool cooldownMoving;

    private MovementState state;
    private enum MovementState
    {
        walking,
        running,
        air
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    void Update()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.5f, whatIsGround);
        
        MyInput();
        StateHandler();
        FastStopOnGround();
        SpeedControl();
        
        if (state is MovementState.walking or MovementState.running)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        if(!cooldownMoving)
            MovePlayer();
    }
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(grounded)
            {
                Jump();
            }
        }
    }
    
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * playerSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * playerSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }
    
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void StateHandler()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            state = MovementState.running;
            playerSpeed = runningSpeed;
        }
        
        else if (grounded)
        {
            state = MovementState.walking;
            playerSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
            playerSpeed = walkSpeed;
        }
    }
    
    private void FastStopOnGround()
    {
        var speedBefore = playerSpeed;

        if (horizontalInput == 0 && verticalInput == 0)
        {
            if (grounded)
            {
                playerSpeed = 0;
            }
            else
            {
                float inertiaSpeed = Mathf.Lerp(speedBefore, 0f, 5f * Time.deltaTime);
                playerSpeed = Mathf.Max(playerSpeed, inertiaSpeed);
            }
        }
        else
        {
            playerSpeed = speedBefore;
        }
    }
    
    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > playerSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * playerSpeed;
            
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
