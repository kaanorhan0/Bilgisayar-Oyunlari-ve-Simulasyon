using UnityEngine;
using UnityEngine.SceneManagement;

public class KapiCikisi : MonoBehaviour
{
    public GameObject cikisPanosu;

    [Header("Ana Oyun Yöneticisi")]
    public DepremGameManager depremGM;

    [Header("Bölüm Sonu Paneli Ayarları (HIZLI GEÇİŞ)")]
    public GameObject bolumSonuPaneli; // İçinde butonların olduğu ana panel
    public GameObject[] gizlenecekArayuzler; // Oyun bitince anında gizlenecek görev yazıları vs.

    private bool icerideMi = false;
    private bool oyunBitti = false; 

    void Start()
    {
        if (cikisPanosu != null) cikisPanosu.SetActive(false);
        if (bolumSonuPaneli != null) bolumSonuPaneli.SetActive(false);
    }

    void Update()
    {
        // Eğer oyun bittiyse (panel açıldıysa) veya GM yoksa kodlar çalışmasın
        if (depremGM == null || oyunBitti) return; 

        // Sadece 30. saniye ile 45. saniye arasındaysa (DepremSonrasi) TRUE olur
        bool kacisVaktiMi = (depremGM.mevcutAsama == DepremGameManager.OyunAsamasi.DepremSonrasi);

        // Zaman doğruysa ve karakter kapı alanının içindeyse panoyu aç
        if (icerideMi && kacisVaktiMi)
        {
            if (cikisPanosu != null && !cikisPanosu.activeSelf)
            {
                cikisPanosu.SetActive(true);
            }

            // Oyuncu E'ye basarsa beklemeden anında bitiş fonksiyonunu tetikle
            if (Input.GetKeyDown(KeyCode.E))
            {
                AnindaBitisYap();
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

    // --- YENİ: ANINDA ÇALIŞAN BİTİŞ FONKSİYONU ---
    private void AnindaBitisYap()
    {
        oyunBitti = true;

        // 1. Etkileşim (E'ye bas) panosunu ve gereksiz UI'ları kapat
        if (cikisPanosu != null) cikisPanosu.SetActive(false);
        
        foreach (GameObject arayuz in gizlenecekArayuzler)
        {
            if (arayuz != null) arayuz.SetActive(false);
        }

        // 2. Arka plandaki manager'a tahliye yapıldığını bildir
        if (depremGM != null)
        {
            depremGM.BasariliTahliyeYapildi();
        }

        // 3. Asıl butonlu ANA PANELİ hiç beklemeden aç
        if (bolumSonuPaneli != null) bolumSonuPaneli.SetActive(true);

        // 4. Fareyi serbest bırak ve oyunu tamamen dondur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; 
    }
    // ---------------------------------------------

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