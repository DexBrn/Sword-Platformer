using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float MoveSpeed = 5f;
    public float ExtraJumpSpeed = 5f;
    public float JumpPower = 10f;
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public Transform Camera;
    
    public Vector2 MouseRotate;
    public Vector3 CameraStartPos;

    [Header("Slide")]
    public float SlideYScale = 0.5f;
    private float StartYScale;
    public bool IsSliding = false;

    public float MaxSlideTime;
    public float SlideForce;
    private float SlideTimer;

    float Horizontal;
    float Vertical;


    [Header("Wall Run")]
    public LayerMask WallRunWall;
    public float WallRunForce;
    public float MaxWallRun;
    private float WallRunTimer;

    public float WallCheckDistance;
    public float MinJumpHeight;
    private RaycastHit LeftWallHit;
    private RaycastHit RightWallHit;
    private bool WallLeft;
    private bool WallRight;

    public float WallJumpUpForce;
    public float WallJumpSideForce;

    public bool IsWallRunning;
   
    ///////////////////////////////////////
    


   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        StartYScale = transform.localScale.y;
        CameraStartPos = Camera.localPosition;
    }

    
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        MouseRotate.x += Input.GetAxis("Mouse X");
        MouseRotate.y += Input.GetAxis("Mouse Y");

        if (MouseRotate.y > 27)
        {
            MouseRotate.y = 27;
        }
        if (MouseRotate.y < -20)
        {
            MouseRotate.y = -20;
        }


        //Moving Camera
        transform.localRotation = Quaternion.Euler(transform.rotation.x, MouseRotate.x, transform.rotation.z);
        Camera.localRotation = Quaternion.Euler(-MouseRotate.y, 0, 0);

        //Moving Player
        transform.position += transform.forward * Vertical * (MoveSpeed / 100);
        transform.position += transform.right * Horizontal * (MoveSpeed / 100);




        //Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //rb.velocity = new Vector3(rb.velocity.x + ExtraJumpSpeed, JumpPower, rb.velocity.y + ExtraJumpSpeed);
            rb.AddForce(transform.up * JumpPower, ForceMode.Impulse);
        }

        //Slide

        if (Input.GetButtonDown("Slide") && (Vertical != 0 || Horizontal != 0))
        {
            StartSlide();
            //transform.localRotation = Quaternion.Euler(-27, MouseRotate.x, transform.rotation.z);
            
            
        }

        if (Input.GetButtonUp("Slide") && IsSliding)
        {
            StopSlide();
        }


        //Respawn
        if (transform.position.y < -20) 
        {
            transform.position = new Vector3(0, 10, 0);
        
        
        }


        CheckForWall();
        wallRunState();


        

    }

    


    private void StartSlide()
    {
        IsSliding = true;

        transform.localScale = new Vector3(transform.localScale.x, SlideYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        SlideTimer = MaxSlideTime;
    }

    private void StopSlide()
    {
        IsSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
        
    }

    private void SlidingMovement()
    {
        Vector3 InputDirection = transform.forward * Vertical + transform.right * Horizontal;

        //rb.AddForce(InputDirection.normalized * SlideForce, ForceMode.Force);
        rb.AddForce(transform.forward * SlideForce, ForceMode.Force);
        SlideTimer -= Time.deltaTime;

        if (SlideTimer <= 0)
        {
            IsSliding = false;
            StopSlide();


        }
    }



    bool IsGrounded()
    {
        return Physics.CheckSphere(GroundCheck.position, 0.5f, GroundLayer);

    }
    

    private void CheckForWall()
    {
        
        WallLeft = Physics.Raycast(transform.position, -transform.right, out LeftWallHit, WallCheckDistance, WallRunWall);
        WallRight = Physics.Raycast(transform.position, transform.right, out RightWallHit, WallCheckDistance, WallRunWall);
    }

    private void FixedUpdate()
    {
        if (IsSliding)
        {
            SlidingMovement();
        }

        if (IsWallRunning)
        {
            WallRunMovement();
        }


    }



    private void wallRunState()
    {
        
        if((WallLeft || WallRight))//&& Vertical > 0 && !IsGrounded())
        {
            
            if (!IsWallRunning)
            {
                
                StartWallRun();
            }
            if (Input.GetButtonDown("Jump")) WallJump();
            
        }
        else
        {
            if (IsWallRunning)
            {
                
                StopWallRun();
                rb.useGravity = true;
            }
        }

    }

    private void StartWallRun()
    {
        IsWallRunning = true;
        if (WallRight)
        {
            Camera.position = new Vector3(Camera.position.x + 3, Camera.position.y, Camera.position.z);
        }
    }

    private void WallRunMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


        Vector3 WallNormal = WallRight ? RightWallHit.normal : LeftWallHit.normal;
        Vector3 WallForward = Vector3.Cross(WallNormal, transform.up);

        

        if ((transform.forward - WallForward).magnitude > (transform.forward - -WallForward).magnitude)
        {
            WallForward = -WallForward;
        }
        rb.AddForce(WallForward * WallRunForce, ForceMode.Force);

        if (!(WallLeft && Horizontal > 1) && !(WallRight && Horizontal < 0))
        {
            rb.AddForce(-WallNormal * 100, ForceMode.Force);
            
        }


    }

    private void StopWallRun()
    { 
        IsWallRunning=false;
        if (Camera.position != CameraStartPos)
        {

            Camera.localPosition = CameraStartPos;
        }
    }

    private void WallJump()
    {
        print("Try to wall jump");
        Vector3 WallNormal = WallRight ? RightWallHit.normal : LeftWallHit.normal;

        Vector3 ForceToApply = transform.up * WallJumpUpForce + WallNormal * WallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(ForceToApply, ForceMode.Impulse);



    }












}









