using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    void Start()
    {
        // Player objesinin içindeki modeldeki Animator'ý bulur
        anim = GetComponentInChildren<Animator>();
        // Ana objedeki Character Controller'ý bulur
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (anim == null || controller == null) return;

        // 1. Hýz Parametresini Blend Tree'ye Gönder
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // Magnitude 0-1 arasý deðer döner, Blend Tree buna göre Idle/Walk yapar
        anim.SetFloat("Speed", moveDirection.magnitude);

        // 2. Yerde Olma Durumunu Güncelle (Havada kalma animasyonu için)
        anim.SetBool("isGrounded", controller.isGrounded);

        // 3. Zýplama Tetikleyicisi
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            anim.SetTrigger("Jump");
        }
    }
}