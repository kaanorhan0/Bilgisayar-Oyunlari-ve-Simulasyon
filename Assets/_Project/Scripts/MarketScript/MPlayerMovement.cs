using UnityEngine;

public class MPlayerMovement : MonoBehaviour
{
    [Header("Hassasiyet Ayarları")]
    public float horizontalSensitivity = 150f; // Sağa-sola dönüş hızı (Daha hızlı)
    public float verticalSensitivity = 80f;   // Yukarı-aşağı bakış hızı (Daha yavaş/yumuşak)

    [Header("Hareket Ayarları")]
    public float speed = 5f;
    public float gravity = -9.81f;

    [Header("Bileşenler")]
    public CharacterController controller;
    public Transform playerCamera;

    private float xRotation = 0f;
    private Vector3 velocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (controller == null) controller = GetComponent<CharacterController>();
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        // 1. Yer Kontrolü
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Bakış Sistemi (İstediğin Ayarlar Burada)
        // Sağa-Sola dönüş (Horizontal)
        float mouseX = Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime;
        // Yukarı-Aşağı bakış (Vertical)
        float mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity * Time.deltaTime;

        // Yukarı-Aşağı kısıtlama
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Sağa-Sola gövde dönüşü
        transform.Rotate(Vector3.up * mouseX);

        // 3. Yürüme
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // 4. Yerçekimi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}