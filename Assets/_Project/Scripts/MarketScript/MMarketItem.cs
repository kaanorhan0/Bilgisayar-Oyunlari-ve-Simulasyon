using UnityEngine;

public class MMarketItem : MonoBehaviour
{
    [Header("Ürün Bilgileri")]
    public string urunAdi; // Örn: SU, FENER, KONSERVE
    public bool depremIcinGerekliMi;

    public void Topla()
    {
        MGameManager manager = Object.FindFirstObjectByType<MGameManager>();

        if (manager != null)
        {
            // GameManager'a ürün adýný ve gereklilik durumunu gönderiyoruz
            manager.UrunAlindi(depremIcinGerekliMi, urunAdi);
        }

        // Bir ürün bir kez alýnabilsin diye scripti kapatýyoruz
        this.enabled = false;
    }
}