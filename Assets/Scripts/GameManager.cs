using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float initialTime = 300f; // Total game time in seconds (5 minutes)
    [SerializeField] private float initialSpawnInterval = 10f; // Initial spawn interval
    [SerializeField] private float minSpawnInterval = 1f; // Minimum spawn interval
    [SerializeField] private float spawnIntervalReduction = 0.5f; // Reduction in spawn interval per unlock
    [SerializeField] private float timerSpeedIncrease = 0.1f; // Increase in timer speed multiplier per unlock
    [SerializeField] private int submissionsPerUnlock = 10; // Number of submissions needed to unlock a style

    [Header("UI Elements")]
    public GameObject[] artworkPrefabs;
    public Transform spawnLocation;
    public AudioSource spawnSound;
    public AudioSource newStyleUnlockAudio;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI styleUnlockText;
    public Slider timerSlider;

    [Header("References")]
    public MoneyManager moneyManager;

    [Header("Valid Styles")]
    public string[] validStyles = { "StillLife", "Cubic", "Jugendstil", "Futurism" }; // Example of styles


    private float timer; // Game timer
    private bool gameRunning = true;
    private float timerSpeedMultiplier = 1f; // Multiplier to make the timer go faster over time
    private List<GameObject> shuffledArtworkPrefabs = new List<GameObject>();
    private int currentIndex = 0;
    private int submissionsCount = 0; // Track the number of submissions

    private List<string> unlockedStyles = new List<string>(); // List to track unlocked styles
    private int unlockedStylesCount = 0; // Track how many styles have been unlocked
    private float spawnInterval; // Current spawn interval

    private void Start()
    {
        timer = initialTime; // Set the starting time
        spawnInterval = initialSpawnInterval; // Initialize the spawn interval
        gameOverPanel.SetActive(false); // Hide the game over panel

        // Initialize the slider
        if (timerSlider != null)
        {
            timerSlider.maxValue = initialTime;
            timerSlider.value = initialTime;
        }

        // Shuffle the artwork array before starting the game
        ShuffleArtwork();

        // Begin spawning artworks and running the game timer
        StartCoroutine(SpawnArtwork());
        StartCoroutine(GameTimer());
    }

    private void ShuffleArtwork()
    {
        shuffledArtworkPrefabs = new List<GameObject>(artworkPrefabs);
        for (int i = 0; i < shuffledArtworkPrefabs.Count; i++)
        {
            GameObject temp = shuffledArtworkPrefabs[i];
            int randomIndex = Random.Range(i, shuffledArtworkPrefabs.Count);
            shuffledArtworkPrefabs[i] = shuffledArtworkPrefabs[randomIndex];
            shuffledArtworkPrefabs[randomIndex] = temp;
        }
    }

    private IEnumerator SpawnArtwork()
    {
        while (gameRunning)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Select artwork from the shuffled list
            GameObject selectedArtwork = shuffledArtworkPrefabs[currentIndex];
            spawnSound.Play();
            Instantiate(selectedArtwork, spawnLocation.position, Quaternion.identity);

            currentIndex++;
            if (currentIndex >= shuffledArtworkPrefabs.Count)
            {
                currentIndex = 0;
                ShuffleArtwork(); // Reshuffle artwork for the next loop
            }
        }
    }

    private IEnumerator GameTimer()
    {
        while (timer > 0 && gameRunning)
        {
            timer -= Time.deltaTime * timerSpeedMultiplier;

            if (timerSlider != null)
            {
                timerSlider.value = timer;
            }

            yield return null;
        }

        if (gameRunning)
        {
            EndGame();
        }
    }

    public void SubmitArtwork()
    {
        if (!gameRunning) return;

        submissionsCount++;

        if (submissionsCount % submissionsPerUnlock == 0)
        {
            UnlockStyle();
            IncreaseGameSpeed();
        }
    }

    private void UnlockStyle()
    {
        // Ensure we don't unlock more styles than are available
        if (unlockedStylesCount < validStyles.Length) // Use .Length instead of .Count for arrays
        {
            // Get the next style to unlock
            string newStyle = validStyles[unlockedStylesCount];
            unlockedStyles.Add(newStyle);
            unlockedStylesCount++;

            // Display the new style unlock message
            if (styleUnlockText != null)
            {
                styleUnlockText.text = $"New style unlocked: {newStyle}";
                styleUnlockText.gameObject.SetActive(true);

                if (newStyleUnlockAudio != null)
                {
                    newStyleUnlockAudio.Play();
                }

                StartCoroutine(HideStyleUnlockNotification());
            }
        }
        else
        {
            // Optionally, handle the case where all styles are unlocked
            Debug.Log("All styles have been unlocked.");
        }
    }

    private void IncreaseGameSpeed()
    {
        // Reduce spawn interval but ensure it doesn't go below the minimum
        spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - spawnIntervalReduction);

        // Increase timer speed multiplier
        timerSpeedMultiplier += timerSpeedIncrease;
    }

    private IEnumerator HideStyleUnlockNotification()
    {
        yield return new WaitForSeconds(3f);
        styleUnlockText.gameObject.SetActive(false);
    }

    private void EndGame()
    {
        gameRunning = false;

        StopAllCoroutines(); // Stop all active coroutines

        gameOverPanel.SetActive(true);
        float totalMoney = moneyManager.GetCurrentMoney();
        scoreText.text = $"Final Score: ${totalMoney:F2}";

        Time.timeScale = 0f; // Pause the game
    }

    public void ModifyTimer(float amount)
    {
        // Increase the timer by 10 seconds
        timer += 10f;

        // Cap the timer to ensure it doesn't exceed initialTime
        if (timer > initialTime) timer = initialTime;
        if (timer < 0) timer = 0;

        // Update the timer slider if it's assigned
        if (timerSlider != null)
        {
            timerSlider.value = timer;
        }
    }


    public bool IsGameRunning()
    {
        return gameRunning;
    }
}
