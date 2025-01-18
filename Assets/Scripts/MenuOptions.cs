using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlManager : MonoBehaviour
{
    private string mainMenuSceneName = "MainMenu";

    // Restarts the current level
    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Goes to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(mainMenuSceneName);
    }
}