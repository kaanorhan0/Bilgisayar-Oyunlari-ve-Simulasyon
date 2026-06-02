using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AnneRehber : MonoBehaviour
{
    [Header("Gerekli Referanslar")]
    public Transform player;
    public Transform asilHedef;

    [Header("Mesafe Ayarlarý")]
    public float maksimumMesafe = 5f;
    public float beklemeMesafesi = 3f;
    // YENÝ EKLENEN: Hedefe ne kadar yaklaþýnca yok olacaðýný belirler
    public float yokOlmaMesafesi = 2f;

    private NavMeshAgent agent;
    private bool yolaDevam = true;

    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (asilHedef != null)
        {
            agent.SetDestination(asilHedef.position);
        }
    }

    void Update()
    {
        if (player == null || asilHedef == null) return;

        // --- YENÝ EKLENEN: HEDEFE VARMA KONTROLÜ ---
        // Anne ile gitmeye çalýþtýðý asýl hedef (stadyum) arasýndaki mesafeyi ölçüyoruz
        float hedefeMesafe = Vector3.Distance(transform.position, asilHedef.position);

        // Eðer hedef noktasýna belirlediðimiz mesafe kadar (örneðin 2 metre) yaklaþtýysa
        if (hedefeMesafe <= yokOlmaMesafesi)
        {
            // Anneyi sahneden tamamen sil ve kodu burada durdur
            Destroy(gameObject);
            return;
        }
        // ------------------------------------------

        // Annenin senin karakterinle arasýndaki mesafe kontrolü (Bekleme Sistemi)
        float mesafe = Vector3.Distance(transform.position, player.position);

        if (mesafe > maksimumMesafe)
        {
            yolaDevam = false;
            agent.isStopped = true;
        }
        else if (mesafe < beklemeMesafesi)
        {
            yolaDevam = true;
            agent.isStopped = false;
            agent.SetDestination(asilHedef.position);
        }

        // Animasyon Kontrolü
        if (anim != null)
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }

        // Anne durduðunda sana doðru döner
        if (!yolaDevam)
        {
            Vector3 bakisYonu = (player.position - transform.position).normalized;
            bakisYonu.y = 0;
            Quaternion hedefDonus = Quaternion.LookRotation(bakisYonu);
            transform.rotation = Quaternion.Slerp(transform.rotation, hedefDonus, Time.deltaTime * 5f);
        }
    }
}