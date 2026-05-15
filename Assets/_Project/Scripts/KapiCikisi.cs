using UnityEngine;
using UnityEngine.SceneManagement;

public class KapiCikisi : MonoBehaviour
{
    public GameObject cikisPanosu; 
    public string gidilecekSahneAdi = "5_Credits"; 

    private bool kapidaMi = false;

    void Start()
    {
        if (cikisPanosu != null) cikisPanosu.SetActive(false);
    }

    void Update()
    {
        // ÖNEMLİ: Sahne geçişi sadece burada, E tuşu kontrolüyle yapılır.
        if (kapidaMi && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(gidilecekSahneAdi);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            kapidaMi = true;
            if (cikisPanosu != null) cikisPanosu.SetActive(true);
            Debug.Log("Kapıya gelindi, E bekleniyor..."); // Konsoldan kontrol etmek için
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