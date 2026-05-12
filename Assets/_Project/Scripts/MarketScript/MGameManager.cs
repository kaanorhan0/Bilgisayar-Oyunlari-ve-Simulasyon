using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MGameManager : MonoBehaviour
{
    [Header("Puan Ayarlar�")]
    public int toplamPuan = 0;
    public int dogruUrunPuani = 5;
    public int yanlisUrunPuani = 5;

    [Header("Zaman Ayarlar�")]
    public float kalanSure = 60f;
    private bool oyunBitti = false;

    [Header("UI Ba�lant�lar� (S�r�kle-B�rak)")]
    public TextMeshProUGUI puanYazisi;   // Sa� �st Puan Text'i
    public TextMeshProUGUI listeYazisi;  // Sa� �st Liste Text'i
    public TextMeshProUGUI sureYazisi;   // Sol �st S�re Text'i
    public GameObject bitisPaneli;       // S�re bitince a��lacak panel (Canvas alt�nda Image)
    public TextMeshProUGUI bitisPuanYazisi; // Paneldeki son puan yaz�s�

    [Header("Al�nacaklar Listesi")]
    // Buraya Inspector'dan SU, FENER, KONSERVE gibi isimleri ekle
    public List<string> alinacaklarListesi = new List<string>();

    // Al�nanlar� takip etmek i�in (Tekrar puan al�m�n� engeller)
    private List<string> alinanlarLogu = new List<string>();

    void Start()
    {
        // Oyun ba�larken zaman� normal h�z�na getir (�nceki elden donuk kalmas�n)
        Time.timeScale = 1f;

        // Biti� panelini oyun ba��nda gizle
        if (bitisPaneli != null) bitisPaneli.SetActive(false);

        ArayuzGuncelle();
    }

    void Update()
    {
        if (oyunBitti) return;

        // S�re sayac� geri say�m
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

    // �r�n topland���nda MMarketItem scripti taraf�ndan �a�r�l�r
    public void UrunAlindi(bool isGerekli, string urunIsmi)
    {
        if (oyunBitti) return;

        // �simdeki bo�luklar� sil ve b�y�k harfe �evir (Hata pay�n� azalt�r)
        string kontrolIsmi = urunIsmi.ToUpper().Trim();

        if (isGerekli)
        {
            // E�er �r�n listede varsa (Yani bu isimde bir �r�n ilk defa al�n�yorsa)
            if (alinacaklarListesi.Contains(kontrolIsmi))
            {
                toplamPuan += dogruUrunPuani;
                alinacaklarListesi.Remove(kontrolIsmi); // Listeden sil
                alinanlarLogu.Add(kontrolIsmi);         // Ar�ive ekle
                Debug.Log("<color=green>Yeni �r�n!</color> " + kontrolIsmi + " al�nd�. +5 Puan.");
            }
            else if (alinanlarLogu.Contains(kontrolIsmi))
            {
                // Zaten al�nm�� �r�n
                Debug.Log("<color=yellow>Zaten Var:</color> " + kontrolIsmi + " i�in tekrar puan verilmedi.");
            }
        }
        else
        {
            // Yanl�� �r�n puan d���r�r ama 0'�n alt�na inmez
            toplamPuan -= yanlisUrunPuani;
            if (toplamPuan < 0) toplamPuan = 0;
            Debug.Log("<color=red>Yanl�� Se�im!</color> " + kontrolIsmi + " puan d���rd�. -5 Puan.");
        }

        ArayuzGuncelle();
    }

    void SureyiGuncelleUI()
    {
        if (sureYazisi != null)
        {
            sureYazisi.text = "SÜRE: " + Mathf.CeilToInt(kalanSure).ToString();

            // Son 10 saniye kala yaz�y� k�rm�z� yap (Heyecan katmak i�in)
            if (kalanSure <= 10f)
                sureYazisi.color = Color.red;
        }
    }

    void SureBitti()
    {
        oyunBitti = true;
        Time.timeScale = 0f; // D�NYAYI DURDURUR: Karakter hareket edemez, fizik i�lemez.

        // Mouse'u serbest b�rak ki butona t�klayabilelim
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (bitisPaneli != null)
        {
            bitisPaneli.SetActive(true); // Biti� ekran�n� a�
            if (bitisPuanYazisi != null)
                bitisPuanYazisi.text = "TOPLAM PUANIN: " + toplamPuan;
        }

        Debug.Log("Zaman doldu. Oyun durduruldu.");
    }

    // Aray�z� tazeleyen fonksiyon
    void ArayuzGuncelle()
    {
        if (puanYazisi != null)
            puanYazisi.text = "PUAN: " + toplamPuan;

        if (listeYazisi != null && !oyunBitti)
        {
            listeYazisi.text = "ALINMASI GEREKENLER\n\n";

            if (alinacaklarListesi.Count == 0)
            {
                listeYazisi.text += "<color=green>L�STE TAMAMLANDI!</color>";
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

    // Butonlar i�in yard�mc� fonksiyonlar
    public void OyunuYenidenBaslat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}