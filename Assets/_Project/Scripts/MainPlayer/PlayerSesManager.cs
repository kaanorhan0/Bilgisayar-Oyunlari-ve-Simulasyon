using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSesManager : MonoBehaviour
{
    [Header("Ses Dosyalarý")]
    public AudioClip yurumeSesi;
    public AudioClip ziplamaSesi;

    [Header("Ayarlar")]
    public float adimMesafesi = 1.2f;
    public float lazerUzunlugu = 1.2f;

    [Header("Ses Þiddeti Ayarlarý")]
    public float yurumeSesSeviyesi = 0.7f;   // Ýsteðin üzerine 0.7 yapýldý kanka
    public float ziplamaSesSeviyesi = 1.5f;

    private AudioSource asource;
    private Vector3 eskiPozisyon;
    private float birikenMesafe = 0f;

    void Start()
    {
        asource = GetComponent<AudioSource>();
        asource.playOnAwake = false;

        eskiPozisyon = transform.position;
    }

    void Update()
    {
        Vector3 hareketMiktari = transform.position - eskiPozisyon;
        hareketMiktari.y = 0;

        float anlikHiz = hareketMiktari.magnitude / Time.deltaTime;

        if (anlikHiz > 0.5f && YerdeMi())
        {
            birikenMesafe += hareketMiktari.magnitude;

            if (birikenMesafe >= adimMesafesi)
            {
                if (yurumeSesi != null) asource.PlayOneShot(yurumeSesi, yurumeSesSeviyesi);

                birikenMesafe = 0f;
            }
        }
        else if (anlikHiz <= 0.1f)
        {
            birikenMesafe = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && YerdeMi())
        {
            if (ziplamaSesi != null) asource.PlayOneShot(ziplamaSesi, ziplamaSesSeviyesi);
        }

        eskiPozisyon = transform.position;
    }

    private bool YerdeMi()
    {
        return Physics.Raycast(transform.position, Vector3.down, lazerUzunlugu);
    }
}