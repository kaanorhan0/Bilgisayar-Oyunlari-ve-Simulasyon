using UnityEngine;
using UnityEngine.Playables; // Timeline'ı kontrol etmek için bu kütüphane şart!

public class SinematikAtla : MonoBehaviour
{
    [Header("Bağlantılar")]
    public PlayableDirector director; // Hangi Timeline'ı atlayacağız?
    public GameObject skipYazisi; // Ekranda kalmasın diye gizleyeceğimiz metin

    void Update()
    {
        // 1. Oyuncu klavyeden 'Return' (Enter) tuşuna bastı mı?
        // 2. Sinematik hala devam ediyor mu? (Şu anki zaman, toplam süreden küçük mü?)
        if (Input.GetKeyDown(KeyCode.Return) && director.time < director.duration)
        {
            // Zaman Çizelgesini (Timeline) anında en son saniyeye ışınla
            director.time = director.duration;

            // Bu ani zaman değişimini Unity'ye zorla kabul ettir ve çalıştır
            director.Evaluate();

            // İşlevi biten "Space'e basın" yazısını ekrandan tamamen gizle
            if (skipYazisi != null)
            {
                skipYazisi.SetActive(false);
            }
        }
    }
}