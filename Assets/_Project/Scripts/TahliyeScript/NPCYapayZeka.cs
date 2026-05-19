using UnityEngine;
using UnityEngine.AI;

public class NPCYapayZeka : MonoBehaviour
{
    [Header("Gidilecek Yer")]
    [Tooltip("Stadyumun içindeki hedef noktayý (Create Empty) buraya sürükle")]
    public Transform stadyumHedefi;

    private NavMeshAgent ajan;
    private Animator anim;

    // NPC'yi durduracak frenimiz
    public bool hareketSerbest = false;

    void Start()
    {
        ajan = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Oyun ilk açýldýðýnda koþma animasyonunu zorla kapatýyoruz
        if (anim != null)
        {
            anim.SetBool("Kosuyor", false);
        }
    }

    void Update()
    {
        // Hareket serbest deðilse veya hedef/ajan yoksa kodun aþaðýsýný okuma, bekle
        if (!hareketSerbest || ajan == null || stadyumHedefi == null) return;

        // Hedefe doðru yola çýktýysa ve hedef belliyse mesafeyi kontrol et
        if (!ajan.pathPending && ajan.remainingDistance <= 0.4f)
        {
            // Þartlar saðlandýysa karakteri sahneden tamamen sil
            Destroy(gameObject);
        }
    }

    // GameManager'ýn dýþarýdan çaðýrýp NPC'leri tetikleyeceði fonksiyon
    public void HareketeGec()
    {
        // Hedef boþsa oyunu çökertme, sadece Console'a ismini yaz ve bu NPC'yi atla!
        if (stadyumHedefi == null)
        {
            Debug.LogError("DÝKKAT: " + gameObject.name + " isimli NPC'nin hedefi YOK! Inspector'dan hedefi atamayý unutmuþsun.");
            return;
        }

        hareketSerbest = true;
        ajan.SetDestination(stadyumHedefi.position);

        if (anim != null)
        {
            anim.SetBool("Kosuyor", true);
        }
    }
}