using UnityEngine;
using TMPro;

public class MPlayerInteraction : MonoBehaviour
{
    [Header("Etkilesim Ayarlari")]
    public float mesafe = 2.0f; // Mesafeyi biraz daha kısalttık
    public float isinKalinligi = 0.1f; // Artık ince bir çizgi değil, ince bir silindir atıyoruz

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

        // Physics.SphereCast: Lazer yerine küçük bir top fırlatır, aralardan sızmaz
        if (Physics.SphereCast(transform.position, isinKalinligi, transform.forward, out hit, mesafe))
        {
            // İLK ÇARPTIĞIMIZ ŞEYİ KONTROL EDİYORUZ
            MMarketItem urun = hit.collider.GetComponent<MMarketItem>();

            // Eğer çarptığımız ilk şey ürünse ve başka bir engel (duvar) önünde değilse
            if (urun != null)
            {
                ekranaYazi.text = "[F] [" + urun.urunAdi.ToUpper() + " AL]";

                if (!etkilesimPaneli.activeSelf)
                {
                    etkilesimPaneli.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    urun.Topla();
                }
            }
            else
            {
                // Eğer ışın önce duvara veya rafa çarparsa buraya girer ve her şeyi kapatır
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