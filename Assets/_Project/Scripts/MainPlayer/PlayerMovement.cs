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

    [Range(0.1f, 1f)]
    [Tooltip("Yukarý-aþaðý bakýþýn saða-sola bakýþa göre ne kadar yavaþ olacaðýný belirler. (0.5 yarý yarýya demektir)")]
    public float verticalMouseMultiplier = 0.5f;

    [Header("Kamera Ayarlarý")]
    public Transform playerCamera; // Karakterin içindeki ana kamerayý buraya sürükle
    private float xRotation = 0f; // Yukarý aþaðý bakýþ açýsýný tutacak

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerAnim = GetComponent<PlayerAnimations>();

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        // 1. Yer Kontrolü
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Fare ile Bakýþ (Yukarý-Aþaðý ve Saða-Sola)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Dikey bakýþ giriþini belirlediðimiz çarpanla yavaþlatýyoruz
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * verticalMouseMultiplier * Time.deltaTime;

        // Karakteri saða-sola döndürür (Y ekseni etrafýnda)
        transform.Rotate(Vector3.up * mouseX);

        // Kamerayý yukarý-aþaðý döndürür (X ekseni etrafýnda)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Kafanýn arkaya takla atmasýný engeller
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // --- ÇÖMELME KONTROLÜ (HAREKET KÝLÝDÝ) ---
        if (playerAnim != null && playerAnim.isCrouching)
        {
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