using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    private PlayerAnimations playerAnim;

    [Header("Hareket Ayarlarý")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpHeight = 2.5f;
    public float gravity = -25f;
    public float mouseSensitivity = 100f;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // Ayný obje üzerindeki Animations scriptini bulur
        playerAnim = GetComponent<PlayerAnimations>();
    }

    void Update()
    {
        // 1. Yer Kontrolü
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Fare ile Bakýþ (Çömelirken de her zaman çalýþýr)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // --- ÇÖMELME KONTROLÜ (HAREKET KÝLÝDÝ) ---
        if (playerAnim != null && playerAnim.isCrouching)
        {
            // Sadece yerçekimi uygula, klavye giriþlerini okuma
            ApplyGravity();
            return;
        }

        // 3. Klavye Giriþleri
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // 4. Hareket Uygulama
        if (move.magnitude >= 0.1f)
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            controller.Move(move.normalized * speed * Time.deltaTime);
        }

        // 5. Zýplama Giriþi
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        ApplyGravity();
    }

    void ApplyGravity()
    {
        // 6. Yerçekimi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}