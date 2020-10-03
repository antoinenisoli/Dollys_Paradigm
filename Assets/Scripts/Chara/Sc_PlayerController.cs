using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_PlayerController : Sc_Character
{
    public Camera viewCam => Camera.main;
    Rigidbody rb => GetComponent<Rigidbody>();

    [SerializeField] float moveSpeed = 800;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float camSensitivity = 150;
    [SerializeField] float camBounds = 40;
    float rotY;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDist = 1f;
    bool detectedGround;

    void FPS_Move()
    {
        Vector3 horizontalAxis = transform.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        Vector3 verticalAxis = transform.forward * Input.GetAxisRaw("Vertical") * Time.deltaTime;
        Vector3 move = (horizontalAxis + verticalAxis) * moveSpeed;
        move.y = rb.velocity.y;
        rb.velocity = move;
    }

    void CameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector2 mouseInput = new Vector2(mouseX, mouseY) * camSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - mouseInput.x, transform.rotation.eulerAngles.z);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotY += mouseY * camSensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -camBounds, camBounds);
        viewCam.transform.rotation = Quaternion.Euler(rotY, viewCam.transform.rotation.eulerAngles.y, viewCam.transform.rotation.eulerAngles.z);
    }

    void Jump()
    {
        if (detectedGround && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        detectedGround = Physics.Raycast(transform.position, Vector3.down, groundDist);
    }

    private void Update()
    {
        FPS_Move();
        CameraControl();
        Jump();
    }
}
