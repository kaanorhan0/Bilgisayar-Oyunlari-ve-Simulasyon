using UnityEngine;
using TMPro;

public class PlayerPosterOkuyucu : MonoBehaviour
{
    public float okumaMesafesi = 2f; 

    [Header("SADECE Posterin Katmanını Seç")]
    public LayerMask posterKatmani;

    [Header("Küçük Uyarı UI (Postere Bak [F])")]
    public TextMeshProUGUI kucukUyariYazisi;
    public GameObject kucukUyariArkaplani;

    [Header("Senin Özel Büyük 112 Panelin")]
    public GameObject ozelPanel;
    public TextMeshProUGUI panelMetni;

    [Header("--- BİLGİLENDİRME SESİ ---")]
    public AudioSource sesKaynagi;
    public AudioClip bilgilendirmeSesi;

    private bool panelAcikMi = false;

    void Update()
    {
        RaycastHit hit;

        // Işın her frame posteri ve mesafeyi (2f) kontrol eder
        if (Physics.Raycast(transform.position, transform.forward, out hit, okumaMesafesi, posterKatmani, QueryTriggerInteraction.Ignore))
        {
            BilgiPosteri poster = hit.transform.GetComponent<BilgiPosteri>();
            if (poster != null)
            {
                // DURUM 1: PANEL KAPALIYSA
                if (!panelAcikMi)
                {
                    UyariAc("Postere Bak [F]"); // Ekrana küçük yazıyı getir

                    if (Input.GetKeyDown(KeyCode.F)) // F'ye basınca büyük paneli aç
                    {
                        UyariKapat();
                        if (panelMetni != null) panelMetni.text = poster.posterMetni;
                        ozelPanel.SetActive(true);
                        panelAcikMi = true;

                        // ---- DEĞİŞEN KISIM: SES BURADA ÇALIYOR ----
                        if (sesKaynagi != null && bilgilendirmeSesi != null)
                        {
                            sesKaynagi.clip = bilgilendirmeSesi;
                            sesKaynagi.Play();
                        }
                        // -------------------------------------------
                    }
                }
                // DURUM 2: PANEL ZATEN AÇIKSA VE HALA ALANDAYSAK
                else
                {
                    UyariKapat(); // Büyük panel açıkken küçük uyarı yazısı gözükmesin

                    if (Input.GetKeyDown(KeyCode.F)) // Tekrar F'ye basarsa paneli kapat
                    {
                        ozelPanel.SetActive(false);
                        panelAcikMi = false;
                        
                        // ---- SESİ DURDUR ----
                        SesiDurdur();
                    }
                }
                return; // Karakter postere 2 metre yakınlıkta baktığı sürece aşağıdaki kapatma kodlarını es geç
            }
        }

        // DURUM 3: ALANDAN ÇIKINCA VEYA KAFASINI ÇEVİRİNCE (OTOMATİK KAPATMA)
        UyariKapat();

        if (panelAcikMi)
        {
            if (ozelPanel != null) ozelPanel.SetActive(false); // Büyük paneli otomatik söndür
            panelAcikMi = false;
            
            // ---- SESİ DURDUR ----
            SesiDurdur();
        }
    }

    // --- ARAYÜZ (UI) AÇMA KAPATMA FONKSİYONLARI ---
    void UyariAc(string metin)
    {
        if (kucukUyariYazisi != null)
        {
            kucukUyariYazisi.text = metin;
            kucukUyariYazisi.gameObject.SetActive(true);
        }
        if (kucukUyariArkaplani != null) kucukUyariArkaplani.SetActive(true);
    }

    void UyariKapat()
    {
        if (kucukUyariYazisi != null) kucukUyariYazisi.gameObject.SetActive(false);
        if (kucukUyariArkaplani != null) kucukUyariArkaplani.SetActive(false);
    }

    // --- SES DURDURMA FONKSİYONU ---
    void SesiDurdur()
    {
        // Eğer ses kaynağı boş değilse ve o an bir ses çalıyorsa, sesi bıçak gibi kes.
        if (sesKaynagi != null && sesKaynagi.isPlaying)
        {
            sesKaynagi.Stop();
        }
    }
}