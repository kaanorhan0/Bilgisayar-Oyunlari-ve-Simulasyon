using UnityEngine;
using TMPro;
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
        if (sayac != null)
        {
            sayac.SayaciDurdur();
        }
      
       
        if (tebriklerPaneli != null)
        {
            bg.SetActive(true);
            tebriklerPaneli.SetActive(true);
        }
   
        yield return new WaitForSeconds(3f);

       
        float gecenZaman = 0f;
        float fadeSuresi = 2f; 

        while (gecenZaman < fadeSuresi)
        {
            gecenZaman += Time.deltaTime;
            siyahEkran.alpha = gecenZaman / fadeSuresi; 
            yield return null; 
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        SceneManager.LoadScene("5_Credits"); 
    }
}

