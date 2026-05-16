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

    [Header("Kapatılacak Oyun İçi Ekranlar (YENİ)")]
    public GameObject[] gizlenecekArayuzler; // Süre, oyun içi puan, crosshair vs. buraya atılacak

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

        // --- YENİ: SÜREYİ VE SİRENLERİ ANINDA KESİYORUZ ---
        // E tuşuna basıldığı an zaman donar ve siren sesi anında durur
        if (sayac != null)
        {
            sayac.SayaciDurdur();
        }
        // -------------------------------------------------

        if (tebriklerPaneli != null)
        {
            if (bg != null) bg.SetActive(true);
            tebriklerPaneli.SetActive(true);
        }

        // Tebrik yazısı ekrandayken artık süre akmayacak ve ses gelmeyecek
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

        // Durdurulmuş olan o temiz nihai zamanı alıp puanı hesaplatıyoruz ve genel paneli açıyoruz
        if (gameManager != null && sayac != null)
        {
            gameManager.PuanHesaplaVeGoster(sayac.GetGecenZaman());
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; 
    }
}