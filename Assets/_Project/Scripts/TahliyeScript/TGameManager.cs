using UnityEngine;
using TMPro;

public class TGameManager : MonoBehaviour
{
    [Header("Puan Ayarları")]
    [Tooltip("Oyuncunun bu bölüme başladığı ham puan")]
    public int baslangicPuani = 0;

    [Header("UI Elemanları")]
    public TextMeshProUGUI oyunIciPuanText; // Oyun içi sabit duran puan
    public TextMeshProUGUI bitisPuanText;    // Paneldeki toplam puan yazısı

    [Header("Panel Ayarları (YENİ)")]
    public GameObject bolumSonuPaneli; // <-- Butonların ve puanın olduğu o BÜTÜN PANEL

    void Start()
    {
        if (oyunIciPuanText != null)
        {
            oyunIciPuanText.text = "Puan: " + baslangicPuani;
        }

        // Oyun başında bütün bölüm sonu paneli kapalı başlasın
        if (bolumSonuPaneli != null)
        {
            bolumSonuPaneli.SetActive(false);
        }
    }

    private int PuanHesapla(float gecenSure)
    {
        if (gecenSure >= 0f && gecenSure <= 45f) return 20;
        if (gecenSure > 45f && gecenSure <= 60f) return 15;
        if (gecenSure > 60f && gecenSure <= 75f) return 10;
        if (gecenSure > 75f && gecenSure <= 90f) return 5;
        return 0;
    }

    // Bu fonksiyon artık hem puanı hesaplayacak hem de BÜTÜN PANELİ açacak!
    public void PuanHesaplaVeGoster(float finalGecenSure)
    {
        int surePuani = PuanHesapla(finalGecenSure);
        int finalToplamPuan = baslangicPuani + surePuani;

        if (bitisPuanText != null)
        {
            bitisPuanText.text = "Kazanılan Toplam Puan: " + finalToplamPuan;
        }

        if (oyunIciPuanText != null)
        {
            oyunIciPuanText.text = "Puan: " + finalToplamPuan;
        }

        // --- İŞTE ARADIĞIMIZ VURUŞ: Sadece yazı değil, bütün panel açılıyor ---
        if (bolumSonuPaneli != null)
        {
            bolumSonuPaneli.SetActive(true);
        }
    }
}