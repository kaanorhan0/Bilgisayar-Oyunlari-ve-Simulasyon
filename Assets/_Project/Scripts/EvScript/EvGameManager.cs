using UnityEngine;
using TMPro;

public class EvGameManager : MonoBehaviour
{
    public static EvGameManager Instance;
    public TextMeshProUGUI sureYazisi, puanYazisi, finalPuanYazisi;
    public GameObject bolumSonuPaneli;
    public float kalanSure = 60f;
    public int mevcutPuan = 0, hedefPuan = 30;
    private bool oyunBitti = false;

    void Awake() { if (Instance == null) Instance = this; Time.timeScale = 1f; }
    void Start() { PuanYazisiniGuncelle(); }

    void Update()
    {
        if (oyunBitti) return;
        if (kalanSure > 0)
        {
            kalanSure -= Time.deltaTime;
            sureYazisi.text = "SÜRE: " + Mathf.Ceil(kalanSure).ToString();
        }
        else OyunBitti();
    }

    public void PuanEkle(int miktar)
    {
        mevcutPuan += miktar;
        PuanYazisiniGuncelle();
        if (mevcutPuan >= hedefPuan) OyunBitti();
    }

    void PuanYazisiniGuncelle() { puanYazisi.text = "PUAN: " + mevcutPuan; }

    void OyunBitti()
    {
        oyunBitti = true;
        if (finalPuanYazisi != null) finalPuanYazisi.text = "TOPLAM PUANIN: " + mevcutPuan;
        bolumSonuPaneli.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}