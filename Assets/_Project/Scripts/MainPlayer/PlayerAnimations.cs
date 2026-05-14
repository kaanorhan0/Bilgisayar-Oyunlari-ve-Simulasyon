using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    // Movement scriptinin eriþebilmesi için public býraktýk
    [HideInInspector] public bool isCrouching = false;

    void Start()
    {
        // Modelin içindeki Animator'ý ve ana objedeki Controller'ý bulur
        anim = GetComponentInChildren<Animator>();
        controller = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        if (anim == null || controller == null) return;

        // --- EÐÝLME (CROUCH) TOGGLE ---
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            // Animator'daki Bool parametresini tetikler
            anim.SetBool("isCrouching", isCrouching);
        }

        // --- KISITLAMA ---
        // Eðer karakter çömelmiþse yürüme deðerlerini sýfýrla ve fonksiyonu burada kes
        if (isCrouching)
        {
            anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
            anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return;
        }

        // 1. Eksen Deðerlerini Al (A/D ve W/S)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 2. Blend Tree Parametrelerini Güncelle
        anim.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);

        // 3. Yer Kontrolü ve Zýplama
        anim.SetBool("isGrounded", controller.isGrounded);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            anim.SetTrigger("Jump");
        }
    }
}