using UnityEngine;
using UnityEngine.UI;

public class MusicSettings : MonoBehaviour
{
    [Header("Görsel Ayarlari")]
    public Image butonunResmi;
    public Sprite acikGorsel;
    public Sprite kapaliGorsel;
    [Header("Muzik ayari")]
    public AudioSource kontrolEdilecekSes;
    private bool acikMi=true;


    public void ButonaBasildi()
    {
        acikMi=!acikMi;
        if (acikMi)
        {
            butonunResmi.sprite=acikGorsel;
            if(kontrolEdilecekSes != null)
            {
                kontrolEdilecekSes.mute=false;
            }
        }
        else
        {
            butonunResmi.sprite=kapaliGorsel;
            if(kontrolEdilecekSes != null){
                kontrolEdilecekSes.mute=true;
            }
        }

    }
}
