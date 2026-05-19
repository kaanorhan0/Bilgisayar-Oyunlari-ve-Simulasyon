using UnityEngine;
using UnityEngine.AI;

public class NPCYapayZeka : MonoBehaviour
{
    [Header("Gidilecek Yer")]
    [Tooltip("Stadyumun içindeki hedef noktayý (Create Empty) buraya sürükle")]
    public Transform stadyumHedefi;

    private NavMeshAgent ajan;
    private Animator anim;

    void Start()
    {
        ajan = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (stadyumHedefi != null)
        {
            ajan.SetDestination(stadyumHedefi.position);

            if (anim != null)
            {
                anim.SetBool("Kosuyor", true);
            }
        }
    }

    void Update()
    {
        // Hedefe doðru yola çýktýysa ve hedef belliyse mesafeyi kontrol et
        if (ajan != null && stadyumHedefi != null)
        {
            // pathPending: Yol hesaplamasý bitmiþ mi?
            // remainingDistance: Hedefe kalan mesafe 1.5 metreden az mý?
            if (!ajan.pathPending && ajan.remainingDistance <= 0.2f)
            {
                // Þartlar saðlandýysa karakteri sahneden tamamen sil
                Destroy(gameObject);
            }
        }
    }
}