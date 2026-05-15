using UnityEngine;

public class GuvenliAlanSensoru : MonoBehaviour
{
    [Header("Kamera Bağlantısı")]
    public KameraSallanti kameraSallantiScripti; 

    private void OnTriggerEnter(Collider other)
    {
        // İçeri giren obje Player ise kameraya haber ver
        if (other.CompareTag("Player") && kameraSallantiScripti != null)
        {
            kameraSallantiScripti.GuvenliAlanaGirdi();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player alandan çıkarsa kameraya haber ver
        if (other.CompareTag("Player") && kameraSallantiScripti != null)
        {
            kameraSallantiScripti.GuvenliAlandanCikti();
        }
    }
}