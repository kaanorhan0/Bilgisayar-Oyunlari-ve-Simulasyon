using System.Collections;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    public string interactText = "Dolabý Sabitle [F]", securingText = "Sabitleniyor...";
    public AudioClip drillClip;
    public float sabitlemeSuresi = 4f; // 4 saniye olarak güncellendi
    public bool isSecured = false, isSecuring = false;
    private AudioSource audioSource;
    private Coroutine secureCoroutine;

    [Header("Matkap Efekti")]
    public GameObject matkapIkoni;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && drillClip != null) audioSource.clip = drillClip;
    }

    public void Secure() { if (!isSecured && !isSecuring) secureCoroutine = StartCoroutine(SecureRoutine()); }

    public void StopSecure()
    {
        if (isSecuring)
        {
            if (secureCoroutine != null) StopCoroutine(secureCoroutine);
            if (audioSource != null) audioSource.Stop();
            isSecuring = false;
        }
    }

    private IEnumerator SecureRoutine()
    {
        isSecuring = true;
        if (audioSource != null) audioSource.Play();

        float gecenSure = 0f;

        // 4 saniye boyunca bekle ama her karede (frame) kontrol et
        while (gecenSure < sabitlemeSuresi)
        {
            // --- KRÝTÝK KONTROL: Oyun bittiyse anýnda iptal et ---
            if (EvGameManager.Instance != null && EvGameManager.Instance.oyunBitti)
            {
                StopSecure(); // Sesi sustur ve durumu sýfýrla
                yield break;  // Coroutine'i burada bitir (Aþaðýdaki puan kýsmýna ASLA geçmez)
            }

            gecenSure += Time.deltaTime;
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Eðer buraya ulaþtýysa süre bitmeden sabitleme tamamlanmýþ demektir
        if (audioSource != null) audioSource.Stop();

        isSecured = true;
        isSecuring = false;

        if (matkapIkoni != null) matkapIkoni.SetActive(false);
        if (EvGameManager.Instance != null) EvGameManager.Instance.PuanEkle(5);
    }
}