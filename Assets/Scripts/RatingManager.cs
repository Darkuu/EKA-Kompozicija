using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class RatingManager : MonoBehaviour
{
    public static RatingManager instance;

    [Header("Valid Style Settings")]
    [Tooltip("Current valid style for the day")]
    public string currentValidStyle;

    [Tooltip("TextMeshProUGUI component displaying the current valid style")]
    public TextMeshProUGUI validStyleText;

    [Header("Day Timer Settings")]
    [Tooltip("Duration of each day in seconds")]
    public float dayDuration = 120f;

    private void Awake()
    {
        // Singleton pattern to ensure a single instance
        if (instance == null)
        {
            instance = this;
            
            // Start the recurring requirement change timer
            InvokeRepeating(nameof(ChangeRequirements), 0f, dayDuration);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Changes the requirements by selecting a new valid style and updating the UI.
    /// </summary>
    private void ChangeRequirements()
    {
        currentValidStyle = GetRandomValidStyle(); // Set a new valid style
        UpdateValidStyleDisplay(); // Update the UI
    }

    /// <summary>
    /// Selects a random valid style from the predefined list.
    /// </summary>
    /// <returns>A random valid style</returns>
    private string GetRandomValidStyle()
    {
        string[] validStyles = { "Realistic", "Cartoon", "Cubic", "Jugendstil" }; 
        return validStyles[Random.Range(0, validStyles.Length)];
    }

    /// <summary>
    /// Updates the TextMeshProUGUI component to display the current valid style.
    /// </summary>
    private void UpdateValidStyleDisplay()
    {
        if (validStyleText != null)
        {
            validStyleText.text = $"Painting's main style must be {currentValidStyle.ToUpper()}";
        }
        else
        {
            Debug.LogWarning("ValidStyleText is not assigned in the RatingManager!");
        }
    }
}
