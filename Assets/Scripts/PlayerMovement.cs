using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Hareket Ayarlarý")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpHeight = 2.5f;
    public float gravity = -25f; // Biraz daha aðýr hissettirmesi için artýrdýk
    public float mouseSensitivity = 100f;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. Yer Kontrolü
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. Fare ile Bakýþ (Saða-Sola Dönüþ)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // 3. Klavye Giriþleri (WASD)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // 4. Hareket Uygulama
        if (move.magnitude >= 0.1f)
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            controller.Move(move.normalized * speed * Time.deltaTime);
        }

        // 5. Zýplama Giriþi (Space)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 6. Yerçekimi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}