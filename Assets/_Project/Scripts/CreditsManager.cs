using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [Header("Akiş Ayarlari")]
    public RectTransform yaziRect;
    public float akisHizi = 50f;  
    public float beklemeSuresi = 15f; 

    void Start()
    {
     
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

      
        StartCoroutine(AnaMenuyeDon());
    }

    void Update()
    {
       
        if (yaziRect != null)
        {
            yaziRect.anchoredPosition += new Vector2(0, akisHizi * Time.deltaTime);
        }
    }

    private IEnumerator AnaMenuyeDon()
    {
     
        yield return new WaitForSeconds(beklemeSuresi);
        
     
        SceneManager.LoadScene("0_AnaMenu"); 
    }
}