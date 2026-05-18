using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Audio Mixer'ı kullanmak için bu kütüphane şart!

public class MusicSettings : MonoBehaviour
{
    [Header("Görsel Ayarlari")]
    public Image butonunResmi;
    public Sprite acikGorsel;
    public Sprite kapaliGorsel;

    [Header("Ses Ayarları (Mixer)")]
    public AudioMixer anaMixer; // Az önce oluşturduğumuz AnaSesMikseri
    public bool buButonMuzikIcinMi = true; // Tikliyse Müziği, değilse Efekti kontrol eder

    private bool acikMi = true;

    // Hangi kanalı kontrol edeceğimizi tutan değişken
    private string kanalAdi; 

    void Start()
    {
        // Inspector'daki tike göre kanal adını belirliyoruz
        kanalAdi = buButonMuzikIcinMi ? "MuzikSesi" : "EfektSesi";
    }

    public void ButonaBasildi()
    {
        acikMi = !acikMi;

        if (acikMi)
        {
            butonunResmi.sprite = acikGorsel;
            // Sesi normal seviyesine (0 desibel) getir
            if(anaMixer != null) anaMixer.SetFloat(kanalAdi, 0f); 
        }
        else
        {
            butonunResmi.sprite = kapaliGorsel;
            // Sesi kökünden kıs (-80 desibel Unity'de tamamen sessizlik demektir)
            if(anaMixer != null) anaMixer.SetFloat(kanalAdi, -80f);
        }
    }
}