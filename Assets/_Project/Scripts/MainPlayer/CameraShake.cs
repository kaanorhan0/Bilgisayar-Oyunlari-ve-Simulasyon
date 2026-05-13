using UnityEngine;
using System.Collections;

public class KameraSallanti : MonoBehaviour
{
    [Header("Gerçekçi Deprem Ayarları")]
    public float depremSuresi = 10f;
    public float savrulmaSiddeti = 1.5f; // Sağa-sola ve öne-arkaya ne kadar uzağa gitsin?
    public float dalgaHizi = 3f;         // Savrulma hızı (Çok artırırsan baş döndürür)

    private Vector3 orjinalPozisyon;
    private bool depremOluyorMu = false;

    void Update()
    {
        // Test etmek için T tuşuna bas
        if (Input.GetKeyDown(KeyCode.T) && !depremOluyorMu)
        {
            StartCoroutine(DalgalanmaSekansi());
        }
    }

    public IEnumerator DalgalanmaSekansi()
    {
        depremOluyorMu = true;
        orjinalPozisyon = transform.localPosition;

        float gecenZaman = 0f;

        while (gecenZaman < depremSuresi)
        {
            // PerlinNoise kullanarak yumuşak ve dalgalı (sağa-sola, yukarı-aşağı, öne-arkaya) hareket üretiyoruz
            float x = (Mathf.PerlinNoise(Time.time * dalgaHizi, 0f) - 0.5f) * 2f * savrulmaSiddeti;
            
            // Y eksenini (yukarı/aşağı) biraz daha az tuttum ki mide bulandırmasın
            float y = (Mathf.PerlinNoise(0f, Time.time * dalgaHizi) - 0.5f) * 2f * (savrulmaSiddeti * 0.3f); 
            
            // Z ekseni: İşte o "öne ve arkaya gitsin" dediğin kısım burası!
            float z = (Mathf.PerlinNoise(Time.time * dalgaHizi, Time.time * dalgaHizi) - 0.5f) * 2f * savrulmaSiddeti;

            // Kamerayı bu dalgalı rotaya yumuşakça oturtuyoruz
            transform.localPosition = new Vector3(orjinalPozisyon.x + x, orjinalPozisyon.y + y, orjinalPozisyon.z + z);

            gecenZaman += Time.deltaTime;
            yield return null;
        }

        // Deprem bitince kamerayı pat diye değil, milim milim süzülerek eski yerine geri getir (böyle daha profesyonel durur)
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
    }
}