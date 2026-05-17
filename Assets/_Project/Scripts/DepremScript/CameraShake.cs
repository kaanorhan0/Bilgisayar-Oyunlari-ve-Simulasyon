using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class KameraSallanti : MonoBehaviour
{
    [Header("Gerçekçi Deprem Ayarları")]
    public float depremSuresi = 10f;
    public float savrulmaSiddeti = 1.5f;
    public float dalgaHizi = 3f;

    [Header("Güvenli Bölge Objeleri")]
    public GameObject[] safeZoneObjeleri; 

    [Header("Ses ve Fizik Ayarları")]
    public AudioSource depremSesKaynagi; 
    public AudioClip[] siraliSesler; 
    public GameObject[] dusecekEsyalar; 

    private List<Rigidbody> aktifRigidler = new List<Rigidbody>(); 

    private Vector3 orjinalPozisyon;
    [HideInInspector] public bool depremOluyorMu = false;
    public bool depremBitti = false; 

    private Coroutine sesDovusCoroutine;

    void Start()
    {
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
        sesDovusCoroutine = StartCoroutine(SesleriSiraylaCal());

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

    IEnumerator SesleriSiraylaCal()
    {
        if (depremSesKaynagi == null || siraliSesler.Length == 0) yield break;

        depremSesKaynagi.loop = false; 
        int sesIndex = 0;

        while (depremOluyorMu)
        {
            AudioClip siradakiKlip = siraliSesler[sesIndex];
            depremSesKaynagi.clip = siradakiKlip;
            depremSesKaynagi.Play();

            yield return new WaitForSeconds(siradakiKlip.length);

            sesIndex++;
            
            if (sesIndex >= siraliSesler.Length)
            {
                sesIndex = 0; 
            }
        }
    }

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
                    Vector3 sarsintiGucu = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)) * (savrulmaSiddeti * 2f);
                    rb.AddForce(sarsintiGucu, ForceMode.Force);
                }
            }

            gecenZaman += Time.deltaTime;
            yield return null;
        }

        // --- DEĞİŞEN KISIM: BIÇAK GİBİ KESMEK YERİNE YUMUŞAKÇA KIS ---
        if (sesDovusCoroutine != null)
        {
            StopCoroutine(sesDovusCoroutine);
        }

        if (depremSesKaynagi != null) 
        {
            // Sesi 0.5 saniyede yavaşça sıfıra indirip öyle durduran yeni fonksiyonu çağırıyoruz
            StartCoroutine(SesiYumusakcaKapat(depremSesKaynagi, 0.5f));
        }
        // -------------------------------------------------------------

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

        foreach (GameObject zone in safeZoneObjeleri)
        {
            if (zone != null) zone.SetActive(false);
        }
    }

    // --- YENİ FONKSİYON: SESİ YAVAŞÇA KISMA ---
    IEnumerator SesiYumusakcaKapat(AudioSource sesKaynagi, float fadeSuresi)
    {
        float baslangicSesi = sesKaynagi.volume;
        float gecenZaman = 0f;

        while (gecenZaman < fadeSuresi)
        {
            gecenZaman += Time.deltaTime;
            sesKaynagi.volume = Mathf.Lerp(baslangicSesi, 0f, gecenZaman / fadeSuresi);
            yield return null;
        }

        sesKaynagi.Stop();
        sesKaynagi.volume = baslangicSesi; // Sonraki depremler için sesi tekrar eski %100 haline getir
    }
    // ------------------------------------------

    public void GuvenliAlanaGirdi() { }
    public void GuvenliAlandanCikti() { }
}