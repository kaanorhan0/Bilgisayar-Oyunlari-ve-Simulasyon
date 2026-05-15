using System.Collections;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    public string interactText = "Dolabý Sabitle [F]", securingText = "Sabitleniyor...";
    public AudioClip drillClip;
    public float sabitlemeSuresi = 3f;
    public bool isSecured = false, isSecuring = false;
    private AudioSource audioSource;
    private Coroutine secureCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && drillClip != null) audioSource.clip = drillClip;
    }

    public void Secure() { if (!isSecured && !isSecuring) secureCoroutine = StartCoroutine(SecureRoutine()); }

    public void StopSecure()
    {
        if (isSecuring)
        {
            if (secureCoroutine != null) StopCoroutine(secureCoroutine);
            if (audioSource != null) audioSource.Stop();
            isSecuring = false;
        }
    }

    private IEnumerator SecureRoutine()
    {
        isSecuring = true;
        if (audioSource != null) audioSource.Play();
        yield return new WaitForSeconds(sabitlemeSuresi);
        if (audioSource != null) audioSource.Stop();
        isSecured = true;
        isSecuring = false;
        if (EvGameManager.Instance != null) EvGameManager.Instance.PuanEkle(5);
    }
}