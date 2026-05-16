using UnityEngine;
using UnityEngine.SceneManagement;

public class KapiCikisi : MonoBehaviour
{
    public GameObject cikisPanosu; 
    public string gidilecekSahneAdi = "5_Credits"; 
    
    [Header("Deprem Kontrolü")]
    public KameraSallanti depremSistemi; // Kameradaki scripti buraya bağlayacağız

    private bool kapidaMi = false;

    void Start()
    {
        if (cikisPanosu != null) cikisPanosu.SetActive(false);
    }

    void Update()
    {
        // GÜVENLİK: Deprem sistemi bağlıysa ve deprem HENÜZ BİTMEDİYSE E tuşu çalışmasın
        if (depremSistemi != null && !depremSistemi.depremBitti) return;

        // OYUNCU DOSTU DETAY: Deprem bittiği an oyuncu zaten kapıdaysa panoyu otomatik aç
        if (kapidaMi && cikisPanosu != null && !cikisPanosu.activeSelf)
        {
            cikisPanosu.SetActive(true);
        }

        // Geçiş kontrolü
        if (kapidaMi && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(gidilecekSahneAdi);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            kapidaMi = true; // Oyuncu kapı alanına girdiğini hafızaya al

            // Deprem bitmediyse panoyu gizli tut (ekranda belirip oyuncunun kafasını karıştırmasın)
            if (depremSistemi != null && !depremSistemi.depremBitti) return;

            if (cikisPanosu != null) cikisPanosu.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            kapidaMi = false;
            if (cikisPanosu != null) cikisPanosu.SetActive(false);
        }
    }
}