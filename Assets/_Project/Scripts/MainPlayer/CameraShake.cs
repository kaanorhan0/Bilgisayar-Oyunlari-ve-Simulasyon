using UnityEngine;
using System.Collections;
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
    public GameObject[] safeZoneObjeleri; // Sahnedeki tüm yeşil silindirleri buraya atacağız

    private Vector3 orjinalPozisyon;
    [HideInInspector] public bool depremOluyorMu = false;

    void Start()
    {
        // Oyun başında her şeyi gizle
        if(tehlikePanosu != null) tehlikePanosu.SetActive(false);
        if(guvendePanosu != null) guvendePanosu.SetActive(false);
        
        // Güvenli bölgeleri oyun başında tek tek kapatıyoruz
        foreach (GameObject zone in safeZoneObjeleri)
        {
            if (zone != null) zone.SetActive(false);
        }

        Invoke("DepremiBaslat", 15f);
    }

    void DepremiBaslat()
    {
        if (!depremOluyorMu) StartCoroutine(DalgalanmaSekansi());
    }

    public IEnumerator DalgalanmaSekansi()
    {
        depremOluyorMu = true;
        orjinalPozisyon = transform.localPosition;

        // Deprem başladı: Panoyu aç ve Güvenli Bölgeleri görünür yap!
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
            gecenZaman += Time.deltaTime;
            yield return null;
        }

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

        // Deprem bitti: Her şeyi tekrar gizle
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