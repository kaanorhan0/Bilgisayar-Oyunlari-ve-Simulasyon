using UnityEngine;
using UnityEngine.SceneManagement;

public class OyunSonuButonlari : MonoBehaviour
{
    // Yeniden Başlat butonuna basılınca çalışacak
    public void OyunuYenidenBaslat()
    {
        // Zamanın ve seslerin normal aktığından emin oluyoruz
        Time.timeScale = 1f; 
        AudioListener.pause = false;
        
        // Oyunun en başına döndürüyor
        SceneManager.LoadScene("1_market");
    }

    // Jenerik butonuna basılınca çalışacak
    public void JenerigeGit()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        
        // Credits sahnesine gönderiyor (Büyük/küçük harfe dikkat)
        SceneManager.LoadScene("5_Credits"); 
    }
}