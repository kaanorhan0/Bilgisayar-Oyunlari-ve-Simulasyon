using UnityEngine;
using UnityEngine.Playables; 

public class SinematikAtla : MonoBehaviour
{
    [Header("Bağlantılar")]
    public PlayableDirector director; 
    public GameObject skipArayuzu; 
    public GameObject bg1;
    public GameObject bg2;

    void Start()
    {
        // Sinematik kendi kendine bittiğinde veya durdurulduğunda çalışacak fonksiyonu bağlıyoruz
        if (director != null)
        {
            director.stopped += SinematikBittiKontrol;
        }
    }

    void OnDestroy()
    {
        // Hafıza sızıntısı olmaması için sahne kapanırken bağlantıyı koparıyoruz
        if (director != null)
        {
            director.stopped -= SinematikBittiKontrol;
        }
    }

    void Update()
    {
        // Oyuncu Enter'a basarsa
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && director.time < director.duration)
        {
            director.time = director.duration;
            director.Evaluate();
            director.Stop(); // <-- Sinematiği durdurduğumuz an Start'taki 'SinematikBittiKontrol' tetiklenir!
        }
    }

    // --- YENİ: Sinematik her halükarda bittiğinde burası çalışır ---
    void SinematikBittiKontrol(PlayableDirector obj)
    {
        // EvGameManager'a haber ver, müzik ve süre başlasın!
        if (EvGameManager.Instance != null)
        {
            EvGameManager.Instance.OyunuVeMuzigiBaslat();
        }

        // Skip yazısını kapat
        if (skipArayuzu != null)
        {
            skipArayuzu.SetActive(false);
            bg1.SetActive(false);
            bg2.SetActive(false);
        }
    }
}