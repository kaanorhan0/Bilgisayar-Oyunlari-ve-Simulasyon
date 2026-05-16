using UnityEngine;
using TMPro;
using DoorScript;

public class PlayerInteractionEv : MonoBehaviour
{
    public float mesafe = 2f;
    public LayerMask urunKatmani;

    [Header("Arayüz (UI) Ayarlarý")]
    public TextMeshProUGUI ekranaYazi;
    public GameObject arkaplanPaneli;

    void Update()
    {
        RaycastHit hit;
        // Kameradan ileriye doðru ýþýn yolluyoruz
        if (Physics.Raycast(transform.position, transform.forward, out hit, mesafe, urunKatmani))
        {
            // Eðer ýþýn bir kapýya çarparsa
            Door kapi = hit.transform.GetComponent<Door>();
            if (kapi != null)
            {
                ekranaYazi.text = kapi.open ? "Kapýyý Kapat [F]" : "Kapýyý Aç [F]";
                UiAc();

                if (Input.GetKeyDown(KeyCode.F)) kapi.OpenDoor();
                return;
            }
        }

        // Iþýn kapýya çarpmýyorsa yazýlarý gizle
        UiKapat();
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