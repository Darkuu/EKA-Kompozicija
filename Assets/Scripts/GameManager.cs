using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float initialTime = 60f; // Starting time in seconds
    [SerializeField] private float spawnInterval = 10f; // Interval for spawning artwork

    [Header("UI Elements")]
    public GameObject[] artworkPrefabs; // Array of artwork prefabs
    public Transform spawnLocation; // Where to spawn the artwork
    public GameObject gameOverPanel; // Panel to display when the game ends
    public TextMeshProUGUI scoreText; // TMP Text for displaying the score
    public Slider timerSlider; // UI Slider for the timer

    [Header("References")]
    public MoneyManager moneyManager; // Reference to the MoneyManager

    [SerializeField] private float timer; // Game timer, editable in the Editor during runtime
    private bool gameRunning = true;

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

        StartCoroutine(SpawnArtwork());
        StartCoroutine(GameTimer());
    }

    private IEnumerator SpawnArtwork()
    {
        while (gameRunning)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Choose a random artwork from the array
            GameObject selectedArtwork = artworkPrefabs[Random.Range(0, artworkPrefabs.Length)];
            Instantiate(selectedArtwork, spawnLocation.position, Quaternion.identity);
        }
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
}
