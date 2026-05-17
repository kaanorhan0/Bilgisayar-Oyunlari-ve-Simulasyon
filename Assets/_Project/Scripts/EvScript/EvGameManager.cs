using UnityEngine;
using TMPro;

public class EvGameManager : MonoBehaviour
{
    public static EvGameManager Instance;
    public TextMeshProUGUI sureYazisi, puanYazisi, finalPuanYazisi;
    public GameObject bolumSonuPaneli;
    public float kalanSure = 60f;
    public int mevcutPuan = 0, hedefPuan = 30;

    private int baslangicPuani = 0;
    public bool oyunBitti = false; // <-- PUBLIC yaptık, Cabinet buraya bakacak
    private bool oyunBasladi = false;

    [Header("Müzik Ayarları")]
    public AudioSource arkaPlanMuzigi;

    void Awake() { if (Instance == null) Instance = this; Time.timeScale = 1f; }

    void Start()
    {
        baslangicPuani = PlayerPrefs.GetInt("GenelPuan", 0);
        PuanYazisiniGuncelle();
    }

    void Update()
    {
        if (!oyunBasladi || oyunBitti) return;

        if (kalanSure > 0)
        {
            kalanSure -= Time.deltaTime;
            sureYazisi.text = "SÜRE: " + Mathf.Ceil(kalanSure).ToString();
        }
        else OyunBitti();
    }

    public void OyunuVeMuzigiBaslat()
    {
        if (oyunBasladi) return;
        oyunBasladi = true;
        if (arkaPlanMuzigi != null && !arkaPlanMuzigi.isPlaying) arkaPlanMuzigi.Play();
    }

    public void PuanEkle(int miktar)
    {
        mevcutPuan += miktar;
        PuanYazisiniGuncelle();
        if (mevcutPuan >= hedefPuan) OyunBitti();
    }

    void PuanYazisiniGuncelle()
    {
        if (puanYazisi != null) puanYazisi.text = "PUAN: " + (baslangicPuani + mevcutPuan);
    }

    void OyunBitti()
    {
        oyunBitti = true;
        if (arkaPlanMuzigi != null) arkaPlanMuzigi.Stop();

        int genelToplam = baslangicPuani + mevcutPuan;
        PlayerPrefs.SetInt("GenelPuan", genelToplam);
        PlayerPrefs.Save();

        if (finalPuanYazisi != null) finalPuanYazisi.text = "TOPLAM PUANIN: " + genelToplam;

        bolumSonuPaneli.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}