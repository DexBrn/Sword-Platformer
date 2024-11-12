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

    public PhysicMaterial Frictionless;
    public float BaseFriction;

    [Header("Wall Run")]
    public LayerMask WallRunWall;
    public float WallRunForce;
    public float MaxWallRun;
    private float WallRunTimer;

    public float WallCheckDistance;
    public float MinJumpHeight;
    private RaycastHit LeftWallHit;
    private RaycastHit RightWallHit;
    private RaycastHit ForwardWallHit;
    private bool WallLeft;
    private bool WallRight;
    private bool WallForward;

    public float WallJumpUpForce;
    public float WallJumpSideForce;

    public bool IsWallRunning;

    private float wallRunCooldown = 0.5f;
    private float lastWallRunTime;

    public float WallClimbSpeed;


    [Header("Slope Handling")]
    public float MaxSlopeAngle;
    RaycastHit SlopeHit;
    public float SlopePushdownForce;
    public bool ExitingSlope = false;
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
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


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
        if (!IsWallRunning)
        {
            
            //transform.localRotation = Quaternion.Euler(20, MouseRotate.x, transform.rotation.z);
        }
        transform.localRotation = Quaternion.Euler(transform.rotation.x, MouseRotate.x, transform.rotation.z);
        Camera.localRotation = Quaternion.Euler(-MouseRotate.y, 0, 0);

        
        

        
        




        //Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //rb.velocity = new Vector3(rb.velocity.x + ExtraJumpSpeed, JumpPower, rb.velocity.y + ExtraJumpSpeed);
            rb.AddForce(transform.up * JumpPower, ForceMode.Impulse);
            ExitingSlope = true;
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
            transform.position = new Vector3(90, 3, -17);
            
        
        }


       


        CheckForWall();
        wallRunState();

        if (IsWallRunning)
        {
            float WallX = transform.position.x;
            transform.position += transform.forward * WallRunForce / 100;
            //transform.position = new Vector3(WallX, transform.position.y, transform.position.z);
            
        }

        if (Input.GetKey(KeyCode.Space) && WallForward)
        {
            
            transform.position += transform.up * WallClimbSpeed / 1000;
        }

        
        

        if (OnSlope())
        {
            transform.rotation = Quaternion.Euler(20, MouseRotate.x, transform.rotation.z);
        }


    }

    


    private void StartSlide()
    {
        IsSliding = true;

        transform.localScale = new Vector3(transform.localScale.x, SlideYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        Frictionless.staticFriction = 1;
        SlideTimer = MaxSlideTime;
    }

    private void StopSlide()
    {
        IsSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
        Frictionless.staticFriction = BaseFriction;
        JumpPower = 75;
    }

    private void SlidingMovement()
    {
        Vector3 InputDirection = transform.forward * Vertical + transform.right * Horizontal;

        //rb.AddForce(InputDirection.normalized * SlideForce, ForceMode.Force);
        rb.AddForce(transform.forward * SlideForce, ForceMode.Force);
        if (!OnSlope())
            SlideTimer -= Time.deltaTime;
        if (OnSlope())
            JumpPower = 200;

        if (SlideTimer <= 0)
        {
            IsSliding = false;
            StopSlide();


        }
    }



    bool IsGrounded()
    {
        ExitingSlope = false;
        return Physics.CheckSphere(GroundCheck.position, 0.5f, GroundLayer);
    }
    

    private void CheckForWall()
    {

        WallLeft = Physics.Raycast(transform.position, -transform.right, out LeftWallHit, WallCheckDistance, WallRunWall);
        WallRight = Physics.Raycast(transform.position, transform.right, out RightWallHit, WallCheckDistance, WallRunWall);
        WallForward = Physics.Raycast(GroundCheck.position, transform.forward, out ForwardWallHit, WallCheckDistance);
        //WallLeft = Physics.SphereCast(transform.position, 0.3f, -transform.right, out LeftWallHit, WallCheckDistance, WallRunWall);
        //WallRight = Physics.SphereCast(transform.position, 0.3f, transform.right, out RightWallHit, WallCheckDistance, WallRunWall);
    }

    private void FixedUpdate()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        //Moving Player
        if (!IsWallRunning)
        {
            transform.position += transform.forward * Vertical * (MoveSpeed / 100);
            transform.position += transform.right * Horizontal * (MoveSpeed / 100);
            
            
            

        }


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
        if (Time.time - lastWallRunTime < wallRunCooldown) return;

        IsWallRunning = true;
        lastWallRunTime = Time.time;
        rb.useGravity = false;
        Vector3 WallNormal = WallRight ? RightWallHit.normal : LeftWallHit.normal;



        IsWallRunning = true;
        if (WallRight)
        {
            Camera.localPosition = new Vector3(Camera.localPosition.x - 3, Camera.localPosition.y, Camera.localPosition.z);
        }
    }

    
    private void WallRunMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


        //Vector3 WallNormal = WallRight ? RightWallHit.normal : LeftWallHit.normal;
        //Vector3 WallForward = Vector3.Cross(WallNormal, transform.up);

        Vector3 WallNormal = WallRight ? RightWallHit.normal : LeftWallHit.normal;
        Vector3 WallForward = Vector3.Reflect(transform.forward, WallNormal);

        rb.AddForce(-WallNormal * 100f, ForceMode.Force);

        if ((transform.forward - WallForward).magnitude > (transform.forward - -WallForward).magnitude)
        {
            //WallForward = -WallForward;
        }
        //rb.AddForce(WallForward * WallRunForce, ForceMode.Force);

        if (!(WallLeft && Horizontal > 1) && !(WallRight && Horizontal < 0))
        {
            //rb.AddForce(-WallNormal * 100, ForceMode.Force);
            
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
        
        Vector3 WallNormal = WallRight ? RightWallHit.normal : LeftWallHit.normal;
        

        Vector3 adjustedJumpDirection = Vector3.Reflect(transform.up, WallNormal);
        //rb.AddForce(adjustedJumpDirection * WallJumpUpForce, ForceMode.VelocityChange);

        
        Vector3 TForceToApply = transform.up * WallJumpUpForce + WallNormal * WallJumpSideForce;
        Vector3 ForceToApply = transform.up * WallJumpUpForce;
        Vector3 SForceToApply = WallNormal * WallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //rb.velocity = new Vector3(0f, 0f, 0f);
        rb.AddForce(SForceToApply, ForceMode.Impulse);
        rb.AddForce(ForceToApply, ForceMode.Impulse);
        rb.AddForce(TForceToApply, ForceMode.Impulse);
        


    }


    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out SlopeHit, 1.2f))
        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return angle < MaxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(transform.forward, SlopeHit.normal).normalized;
    }
    







}









