using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseMenuUI; // Pause menu UI to be shown when the game is paused
    public GameObject gameUI; // Main UI to be hidden when the game is paused

    [Header("Scene Names")]
    public string mainMenuSceneName = "MainMenu"; // Name of the main menu scene
    public string MainMenu = "Main_Game"; // Name of the game scene

    [Header("Audio")]
    public AudioSource pauseSound; // Sound to play when the game is paused
    public AudioSource resumeSound; // Sound to play when the game is resumed
    public AudioSource backgroundMusic; // Background music AudioSource that will be paused/resumed

    [Header("Fade Settings")]
    public Animator fadeAnimator; // Animator for fade-to-black effect

    private bool isPaused = false;

    private void Update()
    {
        // Listen for the ESC key to toggle pause state
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

    // Pauses the game
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Stop game time
        pauseMenuUI.SetActive(true); // Show pause menu
        gameUI.SetActive(false); // Hide the main game UI
        
        if (pauseSound != null)
        {
            pauseSound.Play(); // Play pause sound
        }

        // Pause the background music
        if (backgroundMusic != null)
        {
            backgroundMusic.Pause(); // Pause the background music
        }
    }

    // Resumes the game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        pauseMenuUI.SetActive(false); // Hide pause menu
        gameUI.SetActive(true); // Show the main game UI
        
        if (resumeSound != null)
        {
            resumeSound.Play(); // Play resume sound
        }

        // Resume the background music
        if (backgroundMusic != null)
        {
            backgroundMusic.Play(); // Resume the background music
        }
    }

    // Restarts the level
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Resume game time before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Goes to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale in case the game was paused
        SceneManager.LoadScene(mainMenuSceneName); // Load main menu scene
    }

    // Quit the game (only works in a built game, not in the editor)
    public void QuitGame()
    {
        Application.Quit();
    }

    // Starts the game with a fade-to-black effect
    public void StartGameWithFade()
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.gameObject.SetActive(true); // Ensure the fade canvas is active
            fadeAnimator.SetTrigger("FadeOut"); // Trigger the fade-out animation
        }

        // Start the coroutine to wait for the fade and load the scene
        StartCoroutine(FadeAndStartGame());
    }


    private IEnumerator FadeAndStartGame()
    {
        if (fadeAnimator != null)
        {
            // Wait for the animation to complete (match the animation duration)
            yield return new WaitForSeconds(1f);
        }

        // Load the game scene
        SceneManager.LoadScene(MainMenu);
    }

}