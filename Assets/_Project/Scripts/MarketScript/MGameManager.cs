using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MGameManager : MonoBehaviour
{
    [Header("Puan Ayarlarý")]
    public int toplamPuan = 0;
    public int dogruUrunPuani = 5;
    public int yanlisUrunPuani = 5;

    [Header("Zaman Ayarlarý")]
    public float kalanSure = 60f;
    private bool oyunBitti = false;

    [Header("UI Baðlantýlarý (Sürükle-Býrak)")]
    public TextMeshProUGUI puanYazisi;   // Sað üst Puan Text'i
    public TextMeshProUGUI listeYazisi;  // Sað üst Liste Text'i
    public TextMeshProUGUI sureYazisi;   // Sol üst Süre Text'i
    public GameObject bitisPaneli;       // Süre bitince açýlacak panel (Canvas altýnda Image)
    public TextMeshProUGUI bitisPuanYazisi; // Paneldeki son puan yazýsý

    [Header("Alýnacaklar Listesi")]
    // Buraya Inspector'dan SU, FENER, KONSERVE gibi isimleri ekle
    public List<string> alinacaklarListesi = new List<string>();

    // Alýnanlarý takip etmek için (Tekrar puan alýmýný engeller)
    private List<string> alinanlarLogu = new List<string>();

    void Start()
    {
        // Oyun baþlarken zamaný normal hýzýna getir (Önceki elden donuk kalmasýn)
        Time.timeScale = 1f;

        // Bitiþ panelini oyun baþýnda gizle
        if (bitisPaneli != null) bitisPaneli.SetActive(false);

        ArayuzGuncelle();
    }

    void Update()
    {
        if (oyunBitti) return;

        // Süre sayacý geri sayým
        if (kalanSure > 0)
        {
            kalanSure -= Time.deltaTime;
            SureyiGuncelleUI();
        }
        else
        {
            kalanSure = 0;
            SureBitti();
        }
    }

    // Ürün toplandýðýnda MMarketItem scripti tarafýndan çaðrýlýr
    public void UrunAlindi(bool isGerekli, string urunIsmi)
    {
        if (oyunBitti) return;

        // Ýsimdeki boþluklarý sil ve büyük harfe çevir (Hata payýný azaltýr)
        string kontrolIsmi = urunIsmi.ToUpper().Trim();

        if (isGerekli)
        {
            // Eðer ürün listede varsa (Yani bu isimde bir ürün ilk defa alýnýyorsa)
            if (alinacaklarListesi.Contains(kontrolIsmi))
            {
                toplamPuan += dogruUrunPuani;
                alinacaklarListesi.Remove(kontrolIsmi); // Listeden sil
                alinanlarLogu.Add(kontrolIsmi);         // Arþive ekle
                Debug.Log("<color=green>Yeni Ürün!</color> " + kontrolIsmi + " alýndý. +5 Puan.");
            }
            else if (alinanlarLogu.Contains(kontrolIsmi))
            {
                // Zaten alýnmýþ ürün
                Debug.Log("<color=yellow>Zaten Var:</color> " + kontrolIsmi + " için tekrar puan verilmedi.");
            }
        }
        else
        {
            // Yanlýþ ürün puan düþürür ama 0'ýn altýna inmez
            toplamPuan -= yanlisUrunPuani;
            if (toplamPuan < 0) toplamPuan = 0;
            Debug.Log("<color=red>Yanlýþ Seçim!</color> " + kontrolIsmi + " puan düþürdü. -5 Puan.");
        }

        ArayuzGuncelle();
    }

    void SureyiGuncelleUI()
    {
        if (sureYazisi != null)
        {
            sureYazisi.text = "SÜRE: " + Mathf.CeilToInt(kalanSure).ToString();

            // Son 10 saniye kala yazýyý kýrmýzý yap (Heyecan katmak için)
            if (kalanSure <= 10f)
                sureYazisi.color = Color.red;
        }
    }

    void SureBitti()
    {
        oyunBitti = true;
        Time.timeScale = 0f; // DÜNYAYI DURDURUR: Karakter hareket edemez, fizik iþlemez.

        // Mouse'u serbest býrak ki butona týklayabilelim
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (bitisPaneli != null)
        {
            bitisPaneli.SetActive(true); // Bitiþ ekranýný aç
            if (bitisPuanYazisi != null)
                bitisPuanYazisi.text = "TOPLAM PUANIN: " + toplamPuan;
        }

        Debug.Log("Zaman doldu. Oyun durduruldu.");
    }

    // Arayüzü tazeleyen fonksiyon
    void ArayuzGuncelle()
    {
        if (puanYazisi != null)
            puanYazisi.text = "PUAN: " + toplamPuan;

        if (listeYazisi != null && !oyunBitti)
        {
            listeYazisi.text = "ALINMASI GEREKENLER\n\n";

            if (alinacaklarListesi.Count == 0)
            {
                listeYazisi.text += "<color=green>LÝSTE TAMAMLANDI!</color>";
            }
            else
            {
                foreach (string urun in alinacaklarListesi)
                {
                    listeYazisi.text += "- " + urun.ToUpper() + "\n";
                }
            }
        }
    }

    // Butonlar için yardýmcý fonksiyonlar
    public void OyunuYenidenBaslat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}