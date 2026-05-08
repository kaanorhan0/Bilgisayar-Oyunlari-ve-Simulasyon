using UnityEngine;
using UnityEngine.EventSystems; // Fare etkileşimlerini algılamak için şart!

public class ButonSesi : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Ses Ayarlari")]
    public AudioSource sfxHoparloru;     // AudioManager'daki 2. AudioSource'u buraya atacağız
    public AudioClip uzerineGelmeSesi;   // Fareyle üstüne gelince çıkacak ses (Örn: woosh)
    public AudioClip tiklamaSesi;        // Tıklayınca çıkacak ses (Örn: click)


    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (sfxHoparloru != null && uzerineGelmeSesi != null && !sfxHoparloru.mute)
        {
            sfxHoparloru.PlayOneShot(uzerineGelmeSesi);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (sfxHoparloru != null && tiklamaSesi != null && !sfxHoparloru.mute)
        {
            sfxHoparloru.PlayOneShot(tiklamaSesi);
        }
    }
}
