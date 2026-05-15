using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MGameManager : MonoBehaviour
{
    [Header("Puan Ayarları")]
    public int toplamPuan = 0;
    public int dogruUrunPuani = 5;
    public int yanlisUrunPuani = 5;

    [Header("Zaman Ayarları")]
    public float kalanSure = 60f;
    private bool oyunBitti = false;

    [Header("UI Bağlantıları (Sürükle-Bırak)")]
    public TextMeshProUGUI puanYazisi;
    public TextMeshProUGUI listeYazisi;
    public TextMeshProUGUI sureYazisi;
    public GameObject bitisPaneli;
    public TextMeshProUGUI bitisPuanYazisi;

    [Header("Alınacaklar Listesi")]
    public List<string> alinacaklarListesi = new List<string>();
    private List<string> alinanlarLogu = new List<string>();

    void Start()
    {
        Time.timeScale = 1f;
        if (bitisPaneli != null) bitisPaneli.SetActive(false);
        ArayuzGuncelle();
    }

    void Update()
    {
        if (oyunBitti) return;

        if (kalanSure > 0)
        {
            kalanSure -= Time.deltaTime;
            SureyiGuncelleUI();
        }
        else
        {
            kalanSure = 0;
            OyunBitir();
        }
    }

    public void UrunAlindi(bool isGerekli, string urunIsmi)
    {
        if (oyunBitti) return;

        string kontrolIsmi = urunIsmi.ToUpper().Trim();

        if (isGerekli)
        {
            if (alinacaklarListesi.Contains(kontrolIsmi))
            {
                toplamPuan += dogruUrunPuani;
                alinacaklarListesi.Remove(kontrolIsmi);
                alinanlarLogu.Add(kontrolIsmi);

                // LİSTE BİTTİ Mİ KONTROLÜ
                if (alinacaklarListesi.Count == 0)
                {
                    OyunBitir();
                }
            }
        }
        else
        {
            toplamPuan -= yanlisUrunPuani;
            if (toplamPuan < 0) toplamPuan = 0;
        }

        ArayuzGuncelle();
    }

    void SureyiGuncelleUI()
    {
        if (sureYazisi != null)
        {
            sureYazisi.text = "SÜRE: " + Mathf.CeilToInt(kalanSure).ToString();
            if (kalanSure <= 10f) sureYazisi.color = Color.red;
        }
    }

    void OyunBitir()
    {
        oyunBitti = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (bitisPaneli != null)
        {
            bitisPaneli.SetActive(true);
            if (bitisPuanYazisi != null)
                bitisPuanYazisi.text = "TOPLAM PUANIN: " + toplamPuan;
        }
    }

    void ArayuzGuncelle()
    {
        if (puanYazisi != null)
            puanYazisi.text = "PUAN: " + toplamPuan;

        if (listeYazisi != null && !oyunBitti)
        {
            listeYazisi.text = "ALINMASI GEREKENLER\n\n";

            foreach (string urun in alinacaklarListesi)
            {
                listeYazisi.text += "- " + urun.ToUpper() + "\n";
            }
        }
    }

    public void OyunuYenidenBaslat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}