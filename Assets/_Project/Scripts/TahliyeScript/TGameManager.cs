using UnityEngine;
using TMPro;

public class TGameManager : MonoBehaviour
{
    [Header("Puan Ayarlarý")]
    [Tooltip("Oyuncunun bu bölüme baþladýðý ham puan")]
    public int baslangicPuani = 0;

    [Header("UI Elemanlarý")]
    public TextMeshProUGUI oyunIciPuanText; // Oyun oynanýrken ekranda sabit duracak puan yazýsý
    public TextMeshProUGUI bitisPuanText;    // Tebrikler panelindeki toplam puan yazýsý

    void Start()
    {
        // Oyun baþladýðýnda ekranda sadece neyle baþladýysan o yazar (Örn: Puan: 0)
        // Tahliye olmadan üzerine hiçbir þey eklenmez!
        if (oyunIciPuanText != null)
        {
            oyunIciPuanText.text = "Puan: " + baslangicPuani;
        }
    }

    // Süreye göre gelecek ek puaný hesaplayan gizli fonksiyon
    private int PuanHesapla(float gecenSure)
    {
        if (gecenSure >= 0f && gecenSure <= 45f) return 20;
        if (gecenSure > 45f && gecenSure <= 60f) return 15;
        if (gecenSure > 60f && gecenSure <= 75f) return 10;
        if (gecenSure > 75f && gecenSure <= 90f) return 5;
        return 0;
    }

    // YENÝ: Sadece ve sadece tahliye noktasýna ulaþýp E'ye basýldýðýnda çalýþýr!
    public void PuanHesaplaVeGoster(float finalGecenSure)
    {
        // 1. Süreye göre hak edilen puaný bul
        int surePuani = PuanHesapla(finalGecenSure);

        // 2. Bu puaný baþlangýç puanýnýn üstüne EKLE
        int finalToplamPuan = baslangicPuani + surePuani;

        // 3. Sadece bitiþ panelindeki yazýya bu eklenmiþ toplam skoru yazdýr
        if (bitisPuanText != null)
        {
            bitisPuanText.text = "Kazanýlan Toplam Puan: " + finalToplamPuan;
        }

        // Ýstersen oyun içi yazýyý da bitiþ anýnda güncelleyebilirsin (Ýsteðe baðlý)
        if (oyunIciPuanText != null)
        {
            oyunIciPuanText.text = "Puan: " + finalToplamPuan;
        }
    }
}