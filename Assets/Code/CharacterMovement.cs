using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Di chuyển")]
    public float speed = 5f;          // Tốc độ di chuyển
    public float jumpHeight = 2f;     // Độ cao nhảy
    public float gravity = -9.81f;    // Trọng lực

    [Header("Camera")]
    public Transform cameraTransform; // Camera theo dõi
    public float mouseSensitivity = 2f; // Độ nhạy chuột

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    [SerializeField] GameObject lightSource;
    private bool lightOn = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Ẩn và khóa chuột
        lightSource.SetActive(false);
    }

    void Update()
    {
        // 📡 Kiểm tra xem có chạm đất không
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 🎮 Nhận input từ bàn phím
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Di chuyển theo hướng của nhân vật
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // 🏃‍♂️ Nhảy khi bấm phím "Jump"
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 🚀 Áp dụng trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 🖱 Xử lý camera xoay theo chuột
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Giới hạn góc nhìn lên/xuống

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            lightOn = !lightOn;
            lightSource.SetActive(lightOn);
        }
    }
}
