using UnityEngine;
using TMPro;

public class TGameManager : MonoBehaviour
{
    [Header("Puan Ayarları")]
    [Tooltip("Oyuncunun bu bölüme başladığı ham puan")]
    public int baslangicPuani = 0;

    [Header("UI Elemanları")]
    public TextMeshProUGUI oyunIciPuanText;
    public TextMeshProUGUI bitisPuanText;

    [Header("Rozet Sistemi UI (YENİ)")]
    public TextMeshProUGUI rozetMesajText; // "Tebrikler ... rozet kazandınız" yazacak text

    [Header("Panel Ayarları")]
    public GameObject bolumSonuPaneli;

    [Header("Rozet Görselleri (YENİ)")]
    public GameObject elmasRozet;
    public GameObject altinRozet;
    public GameObject gumusRozet;
    public GameObject bronzRozet;

    void Start()
    {
        baslangicPuani = PlayerPrefs.GetInt("GenelPuan", 0);

        if (oyunIciPuanText != null)
        {
            oyunIciPuanText.text = "Puan: " + baslangicPuani;
        }

        if (bolumSonuPaneli != null)
        {
            bolumSonuPaneli.SetActive(false);
        }

        // Oyun başında tüm rozetleri ve mesajı gizle
        RozetleriKapat();
        if (rozetMesajText != null) rozetMesajText.text = "";
    }

    private void OnEnable()
    {
        // Sahnedeki tüm NPC'leri buluyoruz
        NPCYapayZeka[] tumNPCler = FindObjectsByType<NPCYapayZeka>(FindObjectsSortMode.None);

        // Hepsine tek tek "Harekete geç" emrini gönderiyoruz
        foreach (NPCYapayZeka npc in tumNPCler)
        {
            npc.HareketeGec();
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

    public void PuanHesaplaVeGoster(float finalGecenSure)
    {
        int surePuani = PuanHesapla(finalGecenSure);
        int finalToplamPuan = baslangicPuani + surePuani;

        PlayerPrefs.SetInt("GenelPuan", finalToplamPuan);
        PlayerPrefs.Save();

        if (bitisPuanText != null)
        {
            bitisPuanText.text = "Kazanılan Toplam Puan: " + finalToplamPuan;
        }

        if (oyunIciPuanText != null)
        {
            oyunIciPuanText.text = "Puan: " + finalToplamPuan;
        }

        // Final puanına göre rozeti ve mesajı belirle
        RozetDegerlendir(finalToplamPuan);

        if (bolumSonuPaneli != null)
        {
            bolumSonuPaneli.SetActive(true);
        }
    }

    private void RozetleriKapat()
    {
        if (elmasRozet != null) elmasRozet.SetActive(false);
        if (altinRozet != null) altinRozet.SetActive(false);
        if (gumusRozet != null) gumusRozet.SetActive(false);
        if (bronzRozet != null) bronzRozet.SetActive(false);
    }

    private void RozetDegerlendir(int toplamPuan)
    {
        RozetleriKapat(); // Önce hepsini kapattığımızdan emin olalım

        if (toplamPuan >= 100)
        {
            if (elmasRozet != null) elmasRozet.SetActive(true);
            if (rozetMesajText != null) rozetMesajText.text = "Elmas rozet kazandınız!";
        }
        else if (toplamPuan >= 85) // 85 - 99 arası
        {
            if (altinRozet != null) altinRozet.SetActive(true);
            if (rozetMesajText != null) rozetMesajText.text = "Altın rozet kazandınız!";
        }
        else if (toplamPuan >= 60) // 60 - 84 arası
        {
            if (gumusRozet != null) gumusRozet.SetActive(true);
            if (rozetMesajText != null) rozetMesajText.text = "Gümüs rozet kazandınız!";
        }
        else // 0 - 59 arası
        {
            if (bronzRozet != null) bronzRozet.SetActive(true);
            if (rozetMesajText != null) rozetMesajText.text = "Bronz rozet kazandınız!";
        }
    }
}

