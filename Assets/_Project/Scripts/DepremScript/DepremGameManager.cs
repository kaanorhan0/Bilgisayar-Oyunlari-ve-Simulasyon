using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DepremGameManager : MonoBehaviour
{
    [Header("Süre Ayarları (15s + 15s + 15s)")]
    public float depremOncesiSure = 15f;
    public float depremSuresi = 15f;
    public float depremSonrasiSure = 15f;

    [Header("UI Metinleri (Sadece Sayılar)")]
    public TextMeshProUGUI zamanText;
    public TextMeshProUGUI puanText;

    [Header("UI Aşama Yazıları (Canvasta Tasarlananlar)")]
    public GameObject depremUyariYazisi;
    public GameObject tahliyeUyariYazisi;

    [Header("Bağlantılar")]
    public KameraSallanti kameraSallanti;

    [Header("Bölüm Sonu Paneli Ayarları")]
    public GameObject bolumSonuPaneli;
    public TextMeshProUGUI bitisPuanText;
    public GameObject[] gizlenecekArayuzler;

    private PlayerAnimations playerAnim;
    private float asamaZamanlayici = 0f;
    private int baslangicPuani = 0; // Evden devralınan puan

    public enum OyunAsamasi { DepremOncesi, DepremAni, DepremSonrasi, OyunBitti }
    [HideInInspector] public OyunAsamasi mevcutAsama = OyunAsamasi.DepremOncesi;

    private bool silindirIcindeMi = false;
    private int depremDurusPuani = 0;
    private int tahliyePuani = 0;
    [HideInInspector] public int toplamPuan = 0;

    void Start()
    {
        // Bölüm başında önceki sahnelerin toplam puanını çekiyoruz
        baslangicPuani = PlayerPrefs.GetInt("GenelPuan", 0);

        playerAnim = Object.FindFirstObjectByType<PlayerAnimations>();

        if (kameraSallanti != null)
        {
            kameraSallanti.depremSuresi = depremSuresi;
        }

        if (bolumSonuPaneli != null) bolumSonuPaneli.SetActive(false);

        if (depremUyariYazisi != null) depremUyariYazisi.SetActive(false);
        if (tahliyeUyariYazisi != null) tahliyeUyariYazisi.SetActive(false);

        if (zamanText != null) zamanText.text = "";

        PuanUIYazdir();
    }

    void Update()
    {
        switch (mevcutAsama)
        {
            case OyunAsamasi.DepremOncesi:
                asamaZamanlayici += Time.deltaTime;

                if (asamaZamanlayici >= depremOncesiSure)
                {
                    mevcutAsama = OyunAsamasi.DepremAni;
                    asamaZamanlayici = 0f;
                }
                break;

            case OyunAsamasi.DepremAni:
                asamaZamanlayici += Time.deltaTime;
                int kalanDeprem = Mathf.CeilToInt(depremSuresi - asamaZamanlayici);

                if (depremUyariYazisi != null) depremUyariYazisi.SetActive(true);
                if (zamanText != null) zamanText.text = kalanDeprem.ToString();

                if (silindirIcindeMi && playerAnim != null && playerAnim.isCrouching)
                {
                    if (depremDurusPuani == 0)
                    {
                        if (asamaZamanlayici <= 7f) depremDurusPuani = 10;
                        else if (asamaZamanlayici > 7f && asamaZamanlayici <= 15f) depremDurusPuani = 5;
                    }
                }
                else
                {
                    depremDurusPuani = 0;
                }

                PuanUIYazdir();

                if (asamaZamanlayici >= depremSuresi)
                {
                    mevcutAsama = OyunAsamasi.DepremSonrasi;
                    asamaZamanlayici = 0f;
                }
                break;

            case OyunAsamasi.DepremSonrasi:
                asamaZamanlayici += Time.deltaTime;
                int kalanTahliye = Mathf.CeilToInt(depremSonrasiSure - asamaZamanlayici);

                if (depremUyariYazisi != null) depremUyariYazisi.SetActive(false);
                if (tahliyeUyariYazisi != null) tahliyeUyariYazisi.SetActive(true);
                if (zamanText != null) zamanText.text = kalanTahliye.ToString();

                if (asamaZamanlayici >= depremSonrasiSure)
                {
                    tahliyePuani = 0;
                    OyunSonuHesapla();
                }
                break;
        }
    }

    public void SilindirDurumuGuncelle(bool icerideMi)
    {
        silindirIcindeMi = icerideMi;
    }

    public void BasariliTahliyeYapildi()
    {
        if (mevcutAsama == OyunAsamasi.DepremSonrasi)
        {
            tahliyePuani = 10;
            OyunSonuHesapla();
        }
    }

    void OyunSonuHesapla()
    {
        mevcutAsama = OyunAsamasi.OyunBitti;

        int buBolumPuani = depremDurusPuani + tahliyePuani;
        toplamPuan = baslangicPuani + buBolumPuani;

        // Yeni genel toplamı kaydet
        PlayerPrefs.SetInt("GenelPuan", toplamPuan);
        PlayerPrefs.Save();

        Debug.Log("Bölüm Bitti! Toplam Puan: " + toplamPuan);

        if (zamanText != null) zamanText.gameObject.SetActive(false);
        if (puanText != null) puanText.gameObject.SetActive(false);

        if (depremUyariYazisi != null) depremUyariYazisi.SetActive(false);
        if (tahliyeUyariYazisi != null) tahliyeUyariYazisi.SetActive(false);

        foreach (GameObject arayuz in gizlenecekArayuzler)
        {
            if (arayuz != null) arayuz.SetActive(false);
        }

        if (bitisPuanText != null) bitisPuanText.text = "Kazanılan Toplam Puan: " + toplamPuan;

        if (bolumSonuPaneli != null) bolumSonuPaneli.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    // ARTIK OYUN İÇİ EKRANDA DA TOPLAM PUAN GÖZÜKÜYOR
    void PuanUIYazdir()
    {
        if (puanText != null)
        {
            puanText.text = "Puan: " + (baslangicPuani + depremDurusPuani + tahliyePuani);
        }
    }
}