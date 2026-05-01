using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 6f;
    public float mouseSensitivity = 100f;
    public float jumpForce = 1.5f;
    public float gravity = -9.81f;

    public Transform playerCamera;

    private CharacterController controller;
    private float yVelocity;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        Vector3 move = transform.right * x + transform.forward * z;

        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f; // keeps player grounded
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}