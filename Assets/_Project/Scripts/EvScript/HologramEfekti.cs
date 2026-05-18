using UnityEngine;

public class HologramEfekti : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform kameraTransform;

    [Header("Görsel Efektler")]
    public Color efektRengi = Color.yellow; // Inspector'dan bunu neon sarý yapabilirsin
    public float yanipSonmeHizi = 3f;
    public float minGorunurluk = 0.3f; // Tamamen yok olmasýn

    [Header("Hareket Efektleri")]
    public float suzulmeHizi = 2f;
    public float suzulmeMiktari = 0.1f; // Ne kadar yukarý aþaðý gitsin
    public float buyumeHizi = 4f;
    public float buyumeMiktari = 0.15f; // Kalp atýþý gibi ne kadar þiþsin

    private Vector3 baslangicPozisyonu;
    private Vector3 baslangicScale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baslangicPozisyonu = transform.localPosition;
        baslangicScale = transform.localScale;

        // Ana kamerayý otomatik bulur
        if (Camera.main != null) kameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (kameraTransform == null) return;

        // 1. Yanýp Sönme Efekti (Saydamlýk ile oynayarak)
        float alpha = minGorunurluk + Mathf.PingPong(Time.time * yanipSonmeHizi, 1f - minGorunurluk);
        Color yeniRenk = efektRengi;
        yeniRenk.a = alpha;
        spriteRenderer.color = yeniRenk;

        // 2. Havada Süzülme Efekti
        float yeniY = baslangicPozisyonu.y + Mathf.Sin(Time.time * suzulmeHizi) * suzulmeMiktari;
        transform.localPosition = new Vector3(baslangicPozisyonu.x, yeniY, baslangicPozisyonu.z);

        // 3. Nabýz/Pulsing Efekti (Büyüyüp Küçülme)
        float scaleCarpani = 1f + Mathf.Sin(Time.time * buyumeHizi) * buyumeMiktari;
        transform.localScale = baslangicScale * scaleCarpani;

        // 4. Billboard Efekti (Oyuncu nereden bakarsa baksýn ikon kameraya döner)
        // transform.LookAt(transform.position + kameraTransform.rotation * Vector3.forward, kameraTransform.rotation * Vector3.up);
    }
}