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

        //Moving Camera
        transform.localRotation = Quaternion.Euler(-MouseRotate.y, MouseRotate.x, 0); 

        //Moving Player
        //rb.velocity = new Vector3(Horizontal * MoveSpeed, rb.velocity.y, Vertical * MoveSpeed);
        transform.position += transform.forward * Vertical * (MoveSpeed / 100);
        transform.position += transform.right * Horizontal * (MoveSpeed / 100);


        if (Input.GetButtonUp("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x + ExtraJumpSpeed, JumpPower, rb.velocity.y + ExtraJumpSpeed);
        }
    }

    
    bool IsGrounded()
    {
        return Physics.CheckSphere(GroundCheck.position, 0.5f, GroundLayer);

    }
    


}
