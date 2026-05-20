using UnityEngine;
using TMPro;
using DoorScript;

public class PlayerInteraction : MonoBehaviour
{
    public float mesafe = 2f;
    // LayerMask (urunKatmani) değişkenini tamamen sildik, gerek kalmadı!

    [Header("Arayüz (UI) Ayarları")]
    public TextMeshProUGUI ekranaYazi;
    public GameObject arkaplanPaneli;

    private Cabinet currentSecuringCabinet;

    void Update()
    {
        RaycastHit hit;

        // Işın önüne çıkan İLK şeye çarpacak (Duvar, kapı, dolap fark etmez)
        if (Physics.Raycast(transform.position, transform.forward, out hit, mesafe))
        {
            // 1. İhtimal: Çarptığımız objede 'Door' kodu var mı?
            Door kapi = hit.transform.GetComponent<Door>();
            if (kapi != null)
            {
                ekranaYazi.text = kapi.open ? "Kapıyı Kapat [F]" : "Kapıyı Aç [F]";
                UiAc();

                if (Input.GetKeyDown(KeyCode.F)) kapi.OpenDoor();
                return;
            }

            // 2. İhtimal: Çarptığımız objede 'Cabinet' kodu var mı?
            Cabinet cabinet = hit.transform.GetComponent<Cabinet>();
            if (cabinet != null)
            {
                if (cabinet.isSecured) { UiKapat(); return; }

                ekranaYazi.text = cabinet.isSecuring ? cabinet.securingText : cabinet.interactText;
                UiAc();

                if (Input.GetKeyDown(KeyCode.F) && !cabinet.isSecuring)
                {
                    cabinet.Secure();
                    currentSecuringCabinet = cabinet;
                }
                return;
            }
        }

        // 3. İhtimal: Işın duvara çarptıysa (üstteki if'lere girmediyse) veya boşluğa bakıyorsak
        if (currentSecuringCabinet != null && currentSecuringCabinet.isSecuring)
        {
            currentSecuringCabinet.StopSecure();
            currentSecuringCabinet = null;
        }

        UiKapat(); // Araya duvar girdiyse veya boşluğa bakıyorsan yazıları gizle
    }

    void UiAc()
    {
        if (ekranaYazi != null) ekranaYazi.gameObject.SetActive(true);
        if (arkaplanPaneli != null) arkaplanPaneli.SetActive(true);
    }

    void UiKapat()
    {
        if (ekranaYazi != null) ekranaYazi.gameObject.SetActive(false);
        if (arkaplanPaneli != null) arkaplanPaneli.SetActive(false);
    }
}