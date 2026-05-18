using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuToGame : MonoBehaviour
{
    [SerializeField] GameObject anaMenu;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject controls;
    
public void OyunaBasla()
    {
        SceneManager.LoadScene("1_market");
    }
public void OyundanCik()
    {
        Application.Quit();
        Debug.Log("Oyundan Cikildi");
    }
    public void MainToSettings()
    {
        anaMenu.SetActive(false);
        settings.SetActive(true);
    }
    public void SettingsToMain()
    {
        anaMenu.SetActive(true);
        settings.SetActive(false);
    }
    public void MainMenuToControls()
    {
        anaMenu.SetActive(false);
        controls.SetActive(true);
    }
    public void ControlsToMainMenu()
    {
         anaMenu.SetActive(true);
        controls.SetActive(false);
    }


}