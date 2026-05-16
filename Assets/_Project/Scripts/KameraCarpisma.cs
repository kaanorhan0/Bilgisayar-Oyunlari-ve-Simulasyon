using UnityEngine;

public class KameraCarpisma : MonoBehaviour
{
    [Header("Hedef Ayarları")]
    public Transform karakter;
    public Transform kameraPivot;

    [Header("Mesafe Ayarları")]
    public float maksimumMesafe = 7.5f;
    public float minimumMesafe = 0.5f;
    public float yumusamaHizi = 10f;
    public float kameraYaricapi = 0.2f;

    [Header("Fizik Ayarı")]
    public LayerMask duvarKatmani;

    private Vector3 baslangicLokalPozisyon;
    private float guncelMesafe;

    void Start()
    {
        baslangicLokalPozisyon = transform.localPosition;
        guncelMesafe = maksimumMesafe;
    }

    void LateUpdate()
    {
        if (karakter == null || kameraPivot == null) return;

        Vector3 idealYon = transform.parent.TransformDirection(baslangicLokalPozisyon.normalized);
        Vector3 idealPozisyon = kameraPivot.position + idealYon * maksimumMesafe;

        RaycastHit vurus;

        if (Physics.SphereCast(kameraPivot.position, kameraYaricapi, idealYon, out vurus, maksimumMesafe, duvarKatmani))
        {
            guncelMesafe = Mathf.Clamp(vurus.distance, minimumMesafe, maksimumMesafe);
        }
        else
        {
            guncelMesafe = maksimumMesafe;
        }

        Vector3 hedefLokalPozisyon = baslangicLokalPozisyon.normalized * guncelMesafe;
        transform.localPosition = Vector3.Lerp(transform.localPosition, hedefLokalPozisyon, Time.deltaTime * yumusamaHizi);
    }
}