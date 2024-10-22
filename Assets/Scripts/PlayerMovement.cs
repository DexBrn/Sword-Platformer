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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
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
        //transform.localRotation = Quaternion.Euler(-MouseRotate.y, MouseRotate.x, 0);
        //transform.localRotation = Quaternion.Euler(transform.rotation.x, MouseRotate.x, transform.rotation.z);
        Camera.localRotation = Quaternion.Euler(-MouseRotate.y, 0, 0);

        //Moving Player
        //rb.velocity = new Vector3(Horizontal * MoveSpeed, rb.velocity.y, Vertical * MoveSpeed);
        transform.position += transform.forward * Vertical * (MoveSpeed / 100);
        transform.position += transform.right * Horizontal * (MoveSpeed / 100);


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x + ExtraJumpSpeed, JumpPower, rb.velocity.y + ExtraJumpSpeed);
        }

        while (Input.GetButtonDown("Slide"))
        {
            transform.localRotation = Quaternion.Euler(-27, transform.rotation.y, transform.rotation.z);
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


}
