using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using TMPro;

public class KameraSallanti : MonoBehaviour
{
    [Header("Gerçekçi Deprem Ayarları")]
    public float depremSuresi = 10f;
    public float savrulmaSiddeti = 1.5f;
    public float dalgaHizi = 3f;

    [Header("Arayüz (UI) Arkaplanları")]
    public GameObject tehlikePanosu; 
    public GameObject guvendePanosu; 

    [Header("Güvenli Bölge Objeleri")]
    public GameObject[] safeZoneObjeleri; 

    // --- SES SİSTEMİ GÜNCELLENDİ ---
    [Header("Ses ve Fizik Ayarları")]
    public AudioSource depremSesKaynagi; // Sesi dışarı verecek ana hoparlörümüz
    public AudioClip[] siraliSesler; // Sırayla çalacak mp3/wav dosyaları
    public GameObject[] dusecekEsyalar; 
    // -------------------------------

    private List<Rigidbody> aktifRigidler = new List<Rigidbody>(); 

    private Vector3 orjinalPozisyon;
    [HideInInspector] public bool depremOluyorMu = false;
    public bool depremBitti = false; 

    void Start()
    {
        if(tehlikePanosu != null) tehlikePanosu.SetActive(false);
        if(guvendePanosu != null) guvendePanosu.SetActive(false);
        
        foreach (GameObject zone in safeZoneObjeleri)
        {
            if (zone != null) zone.SetActive(false);
        }

        Invoke("DepremiBaslat", 15f);
    }

    void DepremiBaslat()
    {
        if (!depremOluyorMu) 
        {
            StartCoroutine(DalgalanmaSekansi());
            EfektleriTetikle(); 
        }
    }

    void EfektleriTetikle()
    {
        // Sesleri sırayla çalacak sistemi başlatıyoruz
        StartCoroutine(SesleriSiraylaCal());

        aktifRigidler.Clear();

        foreach (GameObject esya in dusecekEsyalar)
        {
            if (esya != null)
            {
                Rigidbody rb = esya.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    StartCoroutine(EsyaSarsintiGecikmesi(rb));
                }
            }
        }
    }

    // --- YENİ BÖLÜM: SESLERİ BİRBİRİ ARDINA ÇALAN SİSTEM ---
    IEnumerator SesleriSiraylaCal()
    {
        // Eğer ses kaynağı yoksa veya liste boşsa hiç yorulma
        if (depremSesKaynagi == null || siraliSesler.Length == 0) yield break;

        // Deprem döngüsü özelliğini kapat (çünkü biz kendimiz sırayla değiştireceğiz)
        depremSesKaynagi.loop = false; 

        int sesIndex = 0;

        // Deprem devam ettiği sürece bu döngü döner
        while (depremOluyorMu)
        {
            // Sıradaki sesi hoparlöre tak
            AudioClip siradakiKlip = siraliSesler[sesIndex];
            depremSesKaynagi.clip = siradakiKlip;
            depremSesKaynagi.Play();

            // Sesin toplam süresi kadar bekle (Örn: uğultu 4 saniyeyse 4 saniye bekler)
            yield return new WaitForSeconds(siradakiKlip.length);

            // Bekleme bitince bir sonraki sese geç
            sesIndex++;
            
            // Eğer listedeki tüm sesler bittiyse ve deprem hala sürüyorsa, listeyi başa sar
            if (sesIndex >= siraliSesler.Length)
            {
                sesIndex = 0; 
            }
        }
    }
    // --------------------------------------------------------

    IEnumerator EsyaSarsintiGecikmesi(Rigidbody rb)
    {
        float beklemeSuresi = Random.Range(0.5f, 4.5f);
        yield return new WaitForSeconds(beklemeSuresi);

        if (rb != null)
        {
            rb.isKinematic = false; 
            aktifRigidler.Add(rb); 
        }
    }

    public IEnumerator DalgalanmaSekansi()
    {
        depremOluyorMu = true;
        depremBitti = false;
        orjinalPozisyon = transform.localPosition;

        if(tehlikePanosu != null) tehlikePanosu.SetActive(true);
        foreach (GameObject zone in safeZoneObjeleri)
        {
            if (zone != null) zone.SetActive(true);
        }

        float gecenZaman = 0f;
        while (gecenZaman < depremSuresi)
        {
            float x = (Mathf.PerlinNoise(Time.time * dalgaHizi, 0f) - 0.5f) * 2f * savrulmaSiddeti;
            float y = (Mathf.PerlinNoise(0f, Time.time * dalgaHizi) - 0.5f) * 2f * (savrulmaSiddeti * 0.3f); 
            float z = (Mathf.PerlinNoise(Time.time * dalgaHizi, Time.time * dalgaHizi) - 0.5f) * 2f * savrulmaSiddeti;
            transform.localPosition = new Vector3(orjinalPozisyon.x + x, orjinalPozisyon.y + y, orjinalPozisyon.z + z);

            foreach (Rigidbody rb in aktifRigidler)
            {
                if (rb != null)
                {
                    Vector3 sarsintiGucu = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)) * (savrulmaSiddeti * 0.5f);
                    rb.AddForce(sarsintiGucu, ForceMode.Force);
                }
            }

            gecenZaman += Time.deltaTime;
            yield return null;
        }

        // --- GÜNCELLEME: Deprem bitince sesi tamamen sustur ---
        if (depremSesKaynagi != null) 
        {
            depremSesKaynagi.Stop(); 
        }
        // --------------------------------------------------------

        float donusZamani = 0f;
        Vector3 sonPozisyon = transform.localPosition;
        while (donusZamani < 1f)
        {
            transform.localPosition = Vector3.Lerp(sonPozisyon, orjinalPozisyon, donusZamani);
            donusZamani += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = orjinalPozisyon;
        depremOluyorMu = false;
        depremBitti = true; 

        if(tehlikePanosu != null) tehlikePanosu.SetActive(false);
        if(guvendePanosu != null) guvendePanosu.SetActive(false);
        foreach (GameObject zone in safeZoneObjeleri)
        {
            if (zone != null) zone.SetActive(false);
        }
    }

    public void GuvenliAlanaGirdi()
    {
        if (depremOluyorMu)
        {
            if(tehlikePanosu != null) tehlikePanosu.SetActive(false); 
            if(guvendePanosu != null) guvendePanosu.SetActive(true);
        }
    }

    public void GuvenliAlandanCikti()
    {
        if (depremOluyorMu)
        {
            if(guvendePanosu != null) guvendePanosu.SetActive(false); 
            if(tehlikePanosu != null) tehlikePanosu.SetActive(true);
        }
    }
}