using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DepremGameManager : MonoBehaviour
{
    [Header("Süre Ayarlarý (15s + 15s + 15s)")]
    public float depremOncesiSure = 15f;
    public float depremSuresi = 15f;
    public float depremSonrasiSure = 15f;

    [Header("UI Metinleri")]
    public TextMeshProUGUI zamanText;
    public TextMeshProUGUI puanText;

    [Header("Baðlantýlar")]
    public KameraSallanti kameraSallanti;
    public string bitisSahnesi = "4_tahliye";

    private PlayerAnimations playerAnim;
    private float asamaZamanlayici = 0f;

    public enum OyunAsamasi { DepremOncesi, DepremAni, DepremSonrasi, OyunBitti }
    [HideInInspector] public OyunAsamasi mevcutAsama = OyunAsamasi.DepremOncesi;

    private bool silindirIcindeMi = false;
    private int depremDurusPuani = 0;
    private int tahliyePuani = 0;
    [HideInInspector] public int toplamPuan = 0;

    void Start()
    {
        playerAnim = Object.FindFirstObjectByType<PlayerAnimations>();

        if (kameraSallanti != null)
        {
            kameraSallanti.depremSuresi = depremSuresi;
        }

        PuanUIYazdir();
    }

    void Update()
    {
        switch (mevcutAsama)
        {
            case OyunAsamasi.DepremOncesi:
                asamaZamanlayici += Time.deltaTime;

                if (zamanText != null) zamanText.text = "";

                if (asamaZamanlayici >= depremOncesiSure)
                {
                    mevcutAsama = OyunAsamasi.DepremAni;
                    asamaZamanlayici = 0f;
                }
                break;

            case OyunAsamasi.DepremAni:
                asamaZamanlayici += Time.deltaTime;
                int kalanDeprem = Mathf.CeilToInt(depremSuresi - asamaZamanlayici);

                if (zamanText != null) zamanText.text = "DEPREM OLUYOR! ÇÖK-KAPAN-TUTUN: " + kalanDeprem;

                if (silindirIcindeMi && playerAnim != null && playerAnim.isCrouching)
                {
                    if (depremDurusPuani == 0)
                    {
                        if (asamaZamanlayici <= 7f)
                        {
                            depremDurusPuani = 10;
                        }
                        else if (asamaZamanlayici > 7f && asamaZamanlayici <= 15f)
                        {
                            depremDurusPuani = 5;
                        }
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

                if (zamanText != null) zamanText.text = "EVDEN KAÇMAK ÝÇÝN KALAN SÜRE: " + kalanTahliye;

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
        toplamPuan = depremDurusPuani + tahliyePuani;

        Debug.Log("Bölüm Bitti! Toplam Puan: " + toplamPuan);

        SceneManager.LoadScene(bitisSahnesi);
    }

    // BENÝM UNUTTUÐUM, EKRANA PUANI YAZDIRAN KISIM BURASIYDI :)
    void PuanUIYazdir()
    {
        if (puanText != null)
        {
            puanText.text = "Puan: " + (depremDurusPuani + tahliyePuani);
        }
    }
}