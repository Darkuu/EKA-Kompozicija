using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public string[] validStyles = { "StillLife", "Cubic", "Jugendstil", "Futurism", "Animal" };

    private float timer; // Game timer
    private bool gameRunning = true;
    private float timerSpeedMultiplier = 1f;
    private List<GameObject> shuffledArtworkPrefabs = new List<GameObject>();
    private int currentIndex = 0;
    private int submissionsCount = 0;
    private List<string> unlockedStyles = new List<string>();
    private int unlockedStylesCount = 0;
    private float spawnInterval;
    private RatingManager ratingManager;
    private float styleChangeTimer = 20f; // Time interval to change styles (in seconds)

    private List<string> usedStyles = new List<string>(); // List to track used styles in the current cycle

    private void Start()
    {
        timer = initialTime;
        spawnInterval = initialSpawnInterval;
        gameOverPanel.SetActive(false);

        if (timerSlider != null)
        {
            timerSlider.maxValue = initialTime;
            timerSlider.value = initialTime;
        }

        ratingManager = FindObjectOfType<RatingManager>();

        UnlockRandomStyleAndSetActive();
        ShuffleArtwork();
        
        StartCoroutine(SpawnArtwork());
        StartCoroutine(GameTimer());
    }

    private void Update()
    {
        // Decrement the style change timer and change style when it reaches 0
        styleChangeTimer -= Time.deltaTime;

        if (styleChangeTimer <= 0f)
        {
            ChangeValidStyle();
            styleChangeTimer = 20f; // Reset the timer
        }
    }

    private void UnlockRandomStyleAndSetActive()
    {
        if (validStyles.Length > 0 && unlockedStylesCount < validStyles.Length)
        {
            string randomStyle = GetRandomUnlockedStyle();

            unlockedStyles.Add(randomStyle);
            unlockedStylesCount++;

            SetNewValidStyle(randomStyle);
            ShowStyleUnlockNotification(randomStyle);
        }
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

            if (shuffledArtworkPrefabs.Count > 0)
            {
                GameObject selectedArtwork = shuffledArtworkPrefabs[currentIndex];
                spawnSound.Play();
                Instantiate(selectedArtwork, spawnLocation.position, Quaternion.identity);

                currentIndex++;
                if (currentIndex >= shuffledArtworkPrefabs.Count)
                {
                    currentIndex = 0;
                    ShuffleArtwork();
                }
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
        if (unlockedStylesCount < validStyles.Length)
        {
            string newStyle = validStyles[unlockedStylesCount];
            unlockedStyles.Add(newStyle);
            unlockedStylesCount++;

            SetNewValidStyle(newStyle);
            ShowStyleUnlockNotification(newStyle);
        }
    }

    private void SetNewValidStyle(string newStyle)
    {
        if (ratingManager != null)
        {
            ratingManager.SetCurrentValidStyle(newStyle);
        }
    }

    private void ShowStyleUnlockNotification(string style)
    {
        if (styleUnlockText != null)
        {
            styleUnlockText.text = $"New style! : {style}";
            styleUnlockText.gameObject.SetActive(true);

            if (newStyleUnlockAudio != null)
            {
                newStyleUnlockAudio.Play();
            }

            StartCoroutine(HideStyleUnlockNotification());
        }
    }

    private void IncreaseGameSpeed()
    {
        spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - spawnIntervalReduction);
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
        StopAllCoroutines();
        gameOverPanel.SetActive(true);

        float totalMoney = moneyManager.GetCurrentMoney();
        scoreText.text = $"{totalMoney}";

        Time.timeScale = 0f; 
    }

    public void ModifyTimer(float amount)
    {
        timer += amount;

        if (timer > initialTime) timer = initialTime;
        if (timer < 0) timer = 0;

        if (timerSlider != null)
        {
            timerSlider.value = timer;
        }
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }

    public int GetSubmissionsCount()
    {
        return submissionsCount;
    }

    private string GetRandomUnlockedStyle()
    {
        if (unlockedStyles.Count >= validStyles.Length)
        {
            return unlockedStyles[Random.Range(0, unlockedStyles.Count)];
        }

        string randomStyle;
        do
        {
            randomStyle = validStyles[Random.Range(0, validStyles.Length)];
        } while (unlockedStyles.Contains(randomStyle));

        return randomStyle;
    }

    private void ChangeValidStyle()
    {
        if (unlockedStyles.Count == 0) return;

        if (usedStyles.Count >= unlockedStyles.Count)
        {
            usedStyles.Clear();
        }

        string newStyle;
        int attempts = 0;

        do
        {
            newStyle = unlockedStyles[Random.Range(0, unlockedStyles.Count)];
            attempts++;
        } while ((usedStyles.Contains(newStyle) || newStyle == ratingManager.currentValidStyle) && attempts < 100);

        if (attempts >= 100)
        {
            newStyle = unlockedStyles[0];
        }

        usedStyles.Add(newStyle);
        SetNewValidStyle(newStyle);
    }
}
