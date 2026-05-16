using UnityEngine;
using UnityEngine.SceneManagement;

public class KapiCikisi : MonoBehaviour
{
    public GameObject cikisPanosu;
    public string gidilecekSahneAdi = "5_Credits";

    [Header("Ana Oyun Yöneticisi")]
    public DepremGameManager depremGM;

    private bool icerideMi = false;

    void Start()
    {
        if (cikisPanosu != null) cikisPanosu.SetActive(false);
    }

    void Update()
    {
        if (depremGM == null) return;

        // Sadece ve sadece 30. saniye ile 45. saniye arasındaysa (DepremSonrasi) TRUE olur
        bool kacisVaktiMi = (depremGM.mevcutAsama == DepremGameManager.OyunAsamasi.DepremSonrasi);

        // Eğer zaman doğruysa ve karakter kapı alanının içindeyse panoyu zorla aç!
        if (icerideMi && kacisVaktiMi)
        {
            if (cikisPanosu != null && !cikisPanosu.activeSelf)
            {
                cikisPanosu.SetActive(true);
            }

            // Oyuncu E'ye basarsa tahliye puanını ver ve çıkış yap
            if (Input.GetKeyDown(KeyCode.E))
            {
                depremGM.BasariliTahliyeYapildi();
                SceneManager.LoadScene(gidilecekSahneAdi);
            }
        }
        else
        {
            // Zamanı değilse veya karakter alanın dışındaysa panoyu kilitli/gizli tut
            if (cikisPanosu != null && cikisPanosu.activeSelf)
            {
                cikisPanosu.SetActive(false);
            }
        }
    }

    // Karakter içerde durduğu sürece her saniye tetiklenir (Unity'nin uykuya dalmasını engeller)
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            icerideMi = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            icerideMi = false;
        }
    }
}