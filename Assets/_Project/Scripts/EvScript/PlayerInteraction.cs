using UnityEngine;

using TMPro;

using DoorScript;



public class PlayerInteraction : MonoBehaviour

{

    public float mesafe = 2f;

    public LayerMask urunKatmani;

   

    [Header("Arayüz (UI) Ayarları")]

    public TextMeshProUGUI ekranaYazi;

    public GameObject arkaplanPaneli; // <-- Arkadaki siyah görseli buraya bağlayacağız

   

    private Cabinet currentSecuringCabinet;



    void Update()

    {

        RaycastHit hit;

        // İleriye doğru görünmez bir ışın yolluyoruz

        if (Physics.Raycast(transform.position, transform.forward, out hit, mesafe, urunKatmani))

        {

            // Eğer ışın bir kapıya çarparsa

            Door kapi = hit.transform.GetComponent<Door>();

            if (kapi != null)

            {

                ekranaYazi.text = kapi.open ? "Kapıyı Kapat [F]" : "Kapıyı Aç [F]";

                UiAc(); // Hem yazıyı hem arkaplanı açar

               

                if (Input.GetKeyDown(KeyCode.F)) kapi.OpenDoor();

                return;

            }



            // Eğer ışın bir dolaba (Cabinet) çarparsa

            Cabinet cabinet = hit.transform.GetComponent<Cabinet>();

            if (cabinet != null)

            {

                if (cabinet.isSecured) { UiKapat(); return; }

               

                ekranaYazi.text = cabinet.isSecuring ? cabinet.securingText : cabinet.interactText;

                UiAc(); // Hem yazıyı hem arkaplanı açar

               

                if (Input.GetKeyDown(KeyCode.F) && !cabinet.isSecuring)

                {

                    cabinet.Secure();

                    currentSecuringCabinet = cabinet;

                }

                return;

            }

        }



        // Işın hiçbir şeye çarpmıyorsa veya sabitleme iptal olduysa

        if (currentSecuringCabinet != null && currentSecuringCabinet.isSecuring)

        {

            currentSecuringCabinet.StopSecure();

            currentSecuringCabinet = null;

        }

       

        UiKapat(); // Hiçbir şeye bakmıyorsak her şeyi gizle

    }



    // Kod kalabalığı olmasın diye açma kapatma işlemlerini buraya topladık

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