using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Slide")]
    public float SlideYScale = 0.5f;
    private float StartYScale;
    public bool IsSliding = false;

    public float MaxSlideTime;
    public float SlideForce;
    private float SlideTimer;

    float Horizontal;
    float Vertical;

    



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        StartYScale = transform.localScale.y;
    }

    // Update is called once per frame
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





        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x + ExtraJumpSpeed, JumpPower, rb.velocity.y + ExtraJumpSpeed);
        }

        

        if (Input.GetButtonDown("Slide") && (Vertical != 0 || Horizontal != 0))
        {
            StartSlide();
            //transform.localRotation = Quaternion.Euler(-27, MouseRotate.x, transform.rotation.z);
            
            
        }

        if (Input.GetButtonUp("Slide") && IsSliding)
        {
            StopSlide();
        }


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
    
    IEnumerator Slide()
    {
        while (Input.GetButtonDown("Slide"))
        {

            yield return new WaitForSeconds(0.1f);
            transform.localRotation = Quaternion.Euler(-27, transform.rotation.y, transform.rotation.z);
        }
        
        


    }


    private void FixedUpdate()
    {
        if (IsSliding)
        {
            SlidingMovement();
        }
    }


}
