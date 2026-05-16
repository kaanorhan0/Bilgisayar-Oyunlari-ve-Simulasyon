using UnityEngine;
using System.Collections;

public class YesilSilindir : MonoBehaviour
{
    private DepremGameManager depremGM;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        depremGM = Object.FindFirstObjectByType<DepremGameManager>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(YesilYanipSonEfekti());
    }

    IEnumerator YesilYanipSonEfekti()
    {
        if (meshRenderer == null) yield break;

        while (true)
        {
            float pingPong = Mathf.PingPong(Time.time * 3f, 1f);
            meshRenderer.material.color = Color.Lerp(Color.black, Color.green, pingPong);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && depremGM != null)
        {
            depremGM.SilindirDurumuGuncelle(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && depremGM != null)
        {
            depremGM.SilindirDurumuGuncelle(false);
        }
    }
}