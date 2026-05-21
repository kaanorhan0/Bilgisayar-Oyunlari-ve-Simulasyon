using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class SinematikAtla : MonoBehaviour
{
    [Header("Bağlantılar")]
    public PlayableDirector director;
    public GameObject skipArayuzu;
    public GameObject bg1;
    public GameObject bg2;

    [Header("Karakter Kilidi")]
    public MonoBehaviour karakterHareketKodu;

    [Header("Zaman Ayarı")]
    [Tooltip("Sinematiğin kaçıncı saniyede otomatik biteceğini yaz (Örn: 28.5)")]
    public float sinematikSuresi = 28.5f;

    private bool islemYapildi = false;

    void Start()
    {
        // 1. Oyun başlar başlamaz karakteri kilitle
        if (karakterHareketKodu != null)
            karakterHareketKodu.enabled = false;

        // 2. Otomatik geri sayımı başlat
        if (director != null)
            StartCoroutine(SinematikGeriSayim());
    }

    void Update()
    {
        // 1. MANUEL ATLA (ENTER)
        if (!islemYapildi && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            StopAllCoroutines(); // Arka plandaki otomatik sayacı durdur
            BitirVeKamerayaGec();
        }
    }

    IEnumerator SinematikGeriSayim()
    {
        // Belirlediğin süre kadar kronometre sayar (Örn: 28.5 saniye)
        yield return new WaitForSeconds(sinematikSuresi);

        if (!islemYapildi)
        {
            BitirVeKamerayaGec();
        }
    }

    // HEM ENTER HEM OTOMATİK BİTİŞTE ÇALIŞACAK KESİN ÇÖZÜM
    void BitirVeKamerayaGec()
    {
        if (islemYapildi) return;
        islemYapildi = true;

        if (director != null)
        {
            // İŞTE PROJEYİ KURTARAN ORİJİNAL MANTIK:
            // Timeline'ı 80. saniyeye sarıp zorla okutuyoruz ki Main Camera satırın tetiklensin!
            director.time = director.duration;
            director.Evaluate();
            director.Stop();
        }

        // Karakterin hareket kilidini aç
        if (karakterHareketKodu != null)
            karakterHareketKodu.enabled = true;

        // Oyun yöneticisini ve müziği başlat
        if (EvGameManager.Instance != null)
            EvGameManager.Instance.OyunuVeMuzigiBaslat();

        // UI Elemanlarını kapat
        if (skipArayuzu != null) skipArayuzu.SetActive(false);
        if (bg1 != null) bg1.SetActive(false);
        if (bg2 != null) bg2.SetActive(false);
    }
}