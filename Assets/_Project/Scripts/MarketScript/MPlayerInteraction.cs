using UnityEngine;
using TMPro;

public class MPlayerInteraction : MonoBehaviour
{
    [Header("Etkilesim Ayarlari")]
    public float mesafe = 2.0f;
    public float isinKalinligi = 0.1f;

    [Header("UI Elemanlari")]
    public TextMeshProUGUI ekranaYazi;

    private GameObject etkilesimPaneli;

    void Start()
    {
        if (ekranaYazi != null)
        {
            etkilesimPaneli = ekranaYazi.transform.parent.gameObject;
            etkilesimPaneli.SetActive(false);
        }
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, isinKalinligi, transform.forward, out hit, mesafe))
        {
            MMarketItem urun = hit.collider.GetComponent<MMarketItem>();
            MKasa kasa = hit.collider.GetComponent<MKasa>(); // Kasayı arıyoruz

            // 1. DURUM: EĞER BAKTIĞIMIZ ŞEY BİR ÜRÜNSE VE DAHA ÖNCE ALINMAMIŞSA (enabled ise)
            if (urun != null && urun.enabled)
            {
                ekranaYazi.text = "[F] [" + urun.urunAdi.ToUpper() + " AL]";

                if (!etkilesimPaneli.activeSelf)
                {
                    etkilesimPaneli.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    urun.Topla();
                    PanelKapat(); // Bastığımız an paneli kapat ki takılı kalmasın
                }
            }
            // 2. DURUM: EĞER BAKTIĞIMIZ ŞEY YENİ EKLEDİĞİMİZ KASAYSA
            else if (kasa != null)
            {
                ekranaYazi.text = "[F] [ALIŞVERİŞİ TAMAMLA]";

                if (!etkilesimPaneli.activeSelf)
                {
                    etkilesimPaneli.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    kasa.AlisverisiBitir();
                    PanelKapat();
                }
            }
            // 3. DURUM: DUVARA, RAFA VEYA ALINMIŞ BİR ÜRÜNE BAKIYORSAK
            else
            {
                PanelKapat();
            }
        }
        else
        {
            PanelKapat();
        }
    }

    void PanelKapat()
    {
        if (etkilesimPaneli != null && etkilesimPaneli.activeSelf)
        {
            etkilesimPaneli.SetActive(false);
        }
    }
}