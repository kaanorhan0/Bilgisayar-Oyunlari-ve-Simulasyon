using UnityEngine;
using TMPro;
using DoorScript;

public class PlayerInteraction : MonoBehaviour
{
    public float mesafe = 2f;
    public LayerMask urunKatmani;
    public TextMeshProUGUI ekranaYazi;
    private Cabinet currentSecuringCabinet;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, mesafe, urunKatmani))
        {
            Door kapi = hit.transform.GetComponent<Door>();
            if (kapi != null)
            {
                ekranaYazi.text = kapi.open ? "Kapý Kapat [F]" : "Kapý Aç [F]";
                ekranaYazi.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F)) kapi.OpenDoor();
                return;
            }

            Cabinet cabinet = hit.transform.GetComponent<Cabinet>();
            if (cabinet != null)
            {
                if (cabinet.isSecured) { CloseUI(); return; }
                ekranaYazi.text = cabinet.isSecuring ? cabinet.securingText : cabinet.interactText;
                ekranaYazi.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F) && !cabinet.isSecuring)
                {
                    cabinet.Secure();
                    currentSecuringCabinet = cabinet;
                }
                return;
            }
        }

        if (currentSecuringCabinet != null && currentSecuringCabinet.isSecuring)
        {
            currentSecuringCabinet.StopSecure();
            currentSecuringCabinet = null;
        }
        CloseUI();
    }

    void CloseUI() { if (ekranaYazi != null) ekranaYazi.gameObject.SetActive(false); }
}