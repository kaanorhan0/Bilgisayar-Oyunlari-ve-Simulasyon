using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ZamanYonetici : MonoBehaviour
{
    [Header("Zaman Ayarlari")]
    public float kalanZaman = 90f;
    private bool zamanCalisiyor = true;
    private float baslangicZamani = 90f;

    [Header("UI Tasarimi")]
    public TextMeshProUGUI zamanText;
    public GameObject zamanTukendiPaneli;

    [Header("Ses Ayarlari")]
    public AudioSource sirenSesi;

    void Update()
    {
        if (zamanCalisiyor)
        {
            if (kalanZaman > 0)
            {
                kalanZaman -= Time.deltaTime;
                ZamaniGuncelle(kalanZaman);

                if (kalanZaman < 10f)
                {
                    zamanText.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time * 5, 1));
                    float pulse = 1f + Mathf.PingPong(Time.time * 2f, 0.15f);
                    zamanText.transform.localScale = new Vector3(pulse, pulse, pulse);
                }
            }
            else
            {
                kalanZaman = 0;
                zamanCalisiyor = false;
                OyunBitti();
            }
        }
    }

    void ZamaniGuncelle(float zaman)
    {
        float dakikalar = Mathf.FloorToInt(zaman / 60);
        float saniyeler = Mathf.FloorToInt(zaman % 60);
        zamanText.text = string.Format("{0:00}:{1:00}", dakikalar, saniyeler);
    }

    void OyunBitti()
    {
        zamanTukendiPaneli.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (sirenSesi != null) sirenSesi.Stop();

        // YENİ: Süre tamamen bittiğinde TGameManager'a "Süre bitti, puanı sıfırla" emri gönderiyoruz
        TGameManager gameManager = Object.FindFirstObjectByType<TGameManager>();
        if (gameManager != null)
        {
            // 90 saniye geçtiği için direkt 90 paslıyoruz ki puanı 0 yapsın
            gameManager.PuanHesaplaVeGoster(90f);
        }
    }

    public void SayaciDurdur()
    {
        zamanCalisiyor = false;
        if (sirenSesi != null) sirenSesi.Stop();
    }

    public float GetGecenZaman()
    {
        return baslangicZamani - kalanZaman;
    }
}