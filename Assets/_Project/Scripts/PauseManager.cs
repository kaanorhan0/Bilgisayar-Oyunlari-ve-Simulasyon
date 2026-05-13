using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Ayarları")]
    public GameObject pauseMenuUI; 
    public GameObject settingsMenuUI; 
    public GameObject panelMenuUI;
    public GameObject settingsMenuUI2;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); 
        settingsMenuUI.SetActive(false); 
        Time.timeScale = 0f;         
        isPaused = true;
        
        
        AudioListener.pause = true; 

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); 
        settingsMenuUI.SetActive(false); 
        Time.timeScale = 1f;          
        isPaused = false;
        
        // Sesleri tekrar akıtmaya başla
        AudioListener.pause = false; 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        
       
        AudioListener.pause = false; 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        
       
        AudioListener.pause = false; 
        
        SceneManager.LoadScene("0_AnaMenu"); 
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);    
        settingsMenuUI.SetActive(true);  
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false); 
        pauseMenuUI.SetActive(true);     
    }
    public void ClosePanel()
    {
        panelMenuUI.SetActive(true);
        settingsMenuUI2.SetActive(false);
    }
    public void OpenPanel()
    {
        panelMenuUI.SetActive(false);
        settingsMenuUI2.SetActive(true);
    }
}