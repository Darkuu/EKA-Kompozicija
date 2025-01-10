using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float initialTime = 60f; // Starting time in seconds
    [SerializeField] private float spawnInterval = 10f; // Interval for spawning artwork
    [SerializeField] private float styleUnlockInterval = 30f; // Time in seconds to unlock a new style

    [Header("UI Elements")]
    public GameObject[] artworkPrefabs; // Array of artwork prefabs
    public Transform spawnLocation; // Where to spawn the artwork
    public GameObject gameOverPanel; // Panel to display when the game ends
    public TextMeshProUGUI scoreText; // TMP Text for displaying the score
    public TextMeshProUGUI styleUnlockText; // TMP Text for style unlock notifications
    public Slider timerSlider; // UI Slider for the timer

    [Header("References")]
    public MoneyManager moneyManager; // Reference to the MoneyManager

    [SerializeField] private float timer; // Game timer, editable in the Editor during runtime
    private bool gameRunning = true;

    private string[] validStyles = { "Realistic", "Cartoon", "Cubic", "Jugendstil" };
    private List<string> unlockedStyles = new List<string>(); // Keep track of unlocked styles
    private int currentIndex = 0; // Index to track the current artwork

    private void Start()
    {
        timer = initialTime; // Set the starting time
        gameOverPanel.SetActive(false); // Hide the game over panel

        // Initialize the slider
        if (timerSlider != null)
        {
            timerSlider.maxValue = initialTime;
            timerSlider.value = initialTime;
        }

        // Begin unlocking styles as the game progresses
        StartCoroutine(UnlockStyles());

        // Begin spawning artworks and running the game timer
        StartCoroutine(SpawnArtwork());
        StartCoroutine(GameTimer());
    }

    private IEnumerator SpawnArtwork()
    {
        while (gameRunning)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Select artwork, no locking of artworks, just focus on styles
            GameObject selectedArtwork = artworkPrefabs[currentIndex];
            Instantiate(selectedArtwork, spawnLocation.position, Quaternion.identity);

            currentIndex++;
            if (currentIndex >= artworkPrefabs.Length) currentIndex = 0;
        }
    }

    private IEnumerator UnlockStyles()
    {
        // Unlock each style progressively after the interval
        for (int i = 0; i < validStyles.Length; i++)
        {
            yield return new WaitForSeconds(styleUnlockInterval);

            string newStyle = validStyles[i];
            unlockedStyles.Add(newStyle); // Add the style to the unlocked list

            if (styleUnlockText != null)
            {
                styleUnlockText.text = $"New style unlocked: {newStyle}";
                styleUnlockText.gameObject.SetActive(true);

                // Hide the notification after 3 seconds
                StartCoroutine(HideStyleUnlockNotification());
            }
        }
    }

    private IEnumerator HideStyleUnlockNotification()
    {
        yield return new WaitForSeconds(3f);
        styleUnlockText.gameObject.SetActive(false);
    }

    private IEnumerator GameTimer()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // Update the timer UI slider
            if (timerSlider != null)
            {
                timerSlider.value = timer;
            }

            yield return null;
        }

        // Ensure timer doesn't go below zero
        timer = 0;

        EndGame();
    }

    public void ModifyTimer(float amount)
    {
        timer += amount;

        // Cap the timer to ensure it stays within bounds
        if (timer > initialTime) timer = initialTime;
        if (timer < 0) timer = 0;

        // Update the slider value immediately
        if (timerSlider != null)
        {
            timerSlider.value = timer;
        }
    }

    private void EndGame()
    {
        gameRunning = false;

        // Stop spawning artwork
        StopAllCoroutines();

        // Display the final score
        gameOverPanel.SetActive(true);
        float totalMoney = moneyManager.GetCurrentMoney();
        scoreText.text = $"Final Score: ${totalMoney:F2}";

        // Pause the game if necessary (optional)
        Time.timeScale = 0f;
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }

    // Method to check if a style is unlocked
    public bool IsStyleUnlocked(string style)
    {
        return unlockedStyles.Contains(style);
    }
}
