using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TahliyeNoktasi : MonoBehaviour
{
    [Header("UI Ayarları")]
    public GameObject tebriklerPaneli;
    public GameObject bg;
    public CanvasGroup siyahEkran;
    public GameObject etkilesimYazisi;
    public GameObject etkilesimResmi;

    [Header("Kapatılacak Oyun İçi Ekranlar")]
    public GameObject[] gizlenecekArayuzler;

    [Header("Işınlanma Ayarları")]
    [Tooltip("Stadyumun içindeki ışınlanma noktasını buraya sürükle")]
    public Transform stadyumIciNoktasi;
    [Tooltip("Kendi karakterini (Player) buraya sürükle")]
    public GameObject oyuncu;

    private bool alandaMi = false;
    private bool oyunBitti = false;

    void Update()
    {
        if (alandaMi && Input.GetKeyDown(KeyCode.E) && !oyunBitti)
        {
            StartCoroutine(BitisSekansi());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alandaMi = true;
            if (etkilesimYazisi != null) etkilesimYazisi.SetActive(true);
            if (etkilesimResmi != null) etkilesimResmi.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alandaMi = false;
            if (etkilesimYazisi != null) etkilesimYazisi.SetActive(false);
            if (etkilesimResmi != null) etkilesimResmi.SetActive(false);
        }
    }

    private IEnumerator BitisSekansi()
    {
        oyunBitti = true;

        if (etkilesimYazisi != null) etkilesimYazisi.SetActive(false);
        if (etkilesimResmi != null) etkilesimResmi.SetActive(false);

        ZamanYonetici sayac = Object.FindFirstObjectByType<ZamanYonetici>();
        TGameManager gameManager = Object.FindFirstObjectByType<TGameManager>();

        // E tuşuna basıldığı an zaman donar ve siren sesi anında durur
        if (sayac != null)
        {
            sayac.SayaciDurdur();
        }

        // --- IŞINLANMA İŞLEMİ ---
        if (oyuncu != null && stadyumIciNoktasi != null)
        {
            Debug.Log("Işınlanma kodu çalıştı!");

            CharacterController cc = oyuncu.GetComponent<CharacterController>();
            Rigidbody rb = oyuncu.GetComponent<Rigidbody>();

            // Fiziksel hesaplamaları anlık olarak durduruyoruz
            if (cc != null) cc.enabled = false;
            if (rb != null) rb.isKinematic = true;

            // Karakteri stadyuma taşıyoruz
            oyuncu.transform.position = stadyumIciNoktasi.position;
            oyuncu.transform.rotation = stadyumIciNoktasi.rotation;

            // Unity'nin fizik motoruna pozisyonun değiştiğini zorla kabul ettiriyoruz
            Physics.SyncTransforms();

            // Fiziksel hesaplamaları geri açıyoruz
            if (cc != null) cc.enabled = true;
            if (rb != null) rb.isKinematic = false;
        }
        else
        {
            Debug.LogWarning("Kanka Inspector'da Oyuncu veya Stadyum boş kalmış, atamaları kontrol et!");
        }

        // Karakter stadyumun içine ışınlandı. Şimdi 3 saniye etrafa bakması için bekletiyoruz
        yield return new WaitForSeconds(3f);
        // -------------------------------------------------

        // 3 saniye bittikten sonra normal bitiş ekranlarını gösteriyoruz
        if (tebriklerPaneli != null)
        {
            if (bg != null) bg.SetActive(true);
            tebriklerPaneli.SetActive(true);
        }

        // Tebrik yazısı ekrandayken 2.5 saniye daha bekle
        yield return new WaitForSeconds(2.5f);

        float gecenZaman = 0f;
        float fadeSuresi = 1.5f;
        float maksimumKararma = 0.7f;

        while (gecenZaman < fadeSuresi)
        {
            gecenZaman += Time.deltaTime;
            siyahEkran.alpha = (gecenZaman / fadeSuresi) * maksimumKararma;
            yield return null;
        }

        if (tebriklerPaneli != null) tebriklerPaneli.SetActive(false);

        foreach (GameObject arayuz in gizlenecekArayuzler)
        {
            if (arayuz != null) arayuz.SetActive(false);
        }

        // Durdurulmuş temiz zamanı alıp puanı hesaplatıyoruz
        if (gameManager != null && sayac != null)
        {
            gameManager.PuanHesaplaVeGoster(sayac.GetGecenZaman());
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
}