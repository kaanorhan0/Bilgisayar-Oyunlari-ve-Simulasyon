using UnityEngine;

public class AnneEvIciBasitTakip : MonoBehaviour
{
    [Header("Gerekli Referanslar")]
    public Transform player;

    [Header("Hareket Ayarları")]
    public float arkadaDurmaMesafesi = 1.5f;
    public float yürümeHizi = 3f;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        // Senin sırtındaki hedef noktayı belirliyoruz
        Vector3 oyuncuArkasi = player.position - (player.forward * arkadaDurmaMesafesi);

        // Annenin havaya uçmasını veya yere girmesini engellemek için Y eksenini sabitliyoruz
        oyuncuArkasi.y = transform.position.y;

        // Hedefle anne arasındaki anlık mesafe
        float mesafe = Vector3.Distance(transform.position, oyuncuArkasi);

        if (mesafe > 0.5f)
        {
            // 1. HEDEFE YÜRÜME DURUMU
            // Kendi pozisyonundan, hedef pozisyona doğru düz bir çizgide ilerle
            transform.position = Vector3.MoveTowards(transform.position, oyuncuArkasi, yürümeHizi * Time.deltaTime);

            // Yürürken yüzünü gittiği yere dön
            Vector3 yon = (oyuncuArkasi - transform.position).normalized;
            if (yon != Vector3.zero)
            {
                Quaternion hedefDonus = Quaternion.LookRotation(yon);
                transform.rotation = Quaternion.Slerp(transform.rotation, hedefDonus, Time.deltaTime * 10f);
            }

            // Animasyonu aç
            if (anim != null) anim.SetBool("isWalking", true);
        }
        else
        {
            // 2. HEDEFE VARMA (DURMA) DURUMU
            // Durunca seninle aynı yöne (dolaba) baksın
            Vector3 bakisYonu = player.forward;
            bakisYonu.y = 0;
            if (bakisYonu != Vector3.zero)
            {
                Quaternion hedefDonus = Quaternion.LookRotation(bakisYonu);
                transform.rotation = Quaternion.Slerp(transform.rotation, hedefDonus, Time.deltaTime * 5f);
            }

            // Animasyonu kapat
            if (anim != null) anim.SetBool("isWalking", false);
        }
    }
}