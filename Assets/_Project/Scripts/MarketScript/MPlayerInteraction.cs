using UnityEngine;
using TMPro;

public class MPlayerInteraction : MonoBehaviour
{
    [Header("Etkilesim Ayarlari")]
    public float mesafe = 3f;
    public LayerMask urunKatmani; // Inspector'dan "Item" katmanini secmeyi unutma!

    [Header("UI Elemanlari")]
    public TextMeshProUGUI ekranaYazi; // Buraya ArkaPlan'in ICINDEKI yaziyi sürükle

    private GameObject etkilesimPaneli; // ArkaPlan objesini kodla bulacagiz

    void Start()
    {
        // Yazinin icinde bulundugu ArkaPlan objesini (Parent) buluyoruz
        if (ekranaYazi != null)
        {
            etkilesimPaneli = ekranaYazi.transform.parent.gameObject;
            etkilesimPaneli.SetActive(false); // Oyun basinda komple gizle
        }
    }

    void Update()
    {
        RaycastHit hit;

        // Kameradan ileriye dogru isin firlat
        if (Physics.Raycast(transform.position, transform.forward, out hit, mesafe, urunKatmani))
        {
            MMarketItem urun = hit.collider.GetComponent<MMarketItem>();

            if (urun != null)
            {
                // Yaziyi güncelle (Büyük harf ve F [ÜREÜN AL] formati)
                ekranaYazi.text = "F [" + urun.urunAdi.ToUpper() + " AL]";

                // Paneli (Yesil kutuyu) aktif et
                if (!etkilesimPaneli.activeSelf)
                {
                    etkilesimPaneli.SetActive(true);
                }

                // F tusuna basilirsa üründeki Topla fonksiyonunu tetikle
                if (Input.GetKeyDown(KeyCode.F))
                {
                    urun.Topla();
                }
            }
        }
        else
        {
            // Hicbir seye bakmiyorken paneli gizle
            if (etkilesimPaneli != null && etkilesimPaneli.activeSelf)
            {
                etkilesimPaneli.SetActive(false);
            }
        }
    }
}