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

    [Header("Panel Ayarları (YENİ)")]
    public GameObject bolumSonuPaneli;

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
    }

    private void OnEnable()
    {
        // Sahnedeki tüm NPC'leri buluyoruz
        NPCYapayZeka[] tumNPCler = FindObjectsOfType<NPCYapayZeka>();

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

        if (bolumSonuPaneli != null)
        {
            bolumSonuPaneli.SetActive(true);
        }
    }
}