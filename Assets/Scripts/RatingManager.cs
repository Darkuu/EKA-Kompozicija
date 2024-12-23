using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class RatingManager : MonoBehaviour
{
    public static RatingManager instance;

    [Header("Valid Color Settings")]
    [Tooltip("Current valid color for the day")]
    public string currentValidColor;

    [Tooltip("TextMeshProUGUI component displaying the current valid color")]
    public TextMeshProUGUI validColorText;

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
    /// Changes the requirements by selecting a new valid color and updating the UI.
    /// </summary>
    private void ChangeRequirements()
    {
        currentValidColor = GetRandomValidColor(); // Set a new valid color
        UpdateValidColorDisplay(); // Update the UI
    }

    /// <summary>
    /// Selects a random valid color from the predefined list.
    /// </summary>
    /// <returns>A random valid color</returns>
    private string GetRandomValidColor()
    {
        string[] validColors = { "red", "green", "blue", "purple" }; // Add or modify colors as needed
        return validColors[Random.Range(0, validColors.Length)];
    }

    /// <summary>
    /// Updates the TextMeshProUGUI component to display the current valid color.
    /// </summary>
    private void UpdateValidColorDisplay()
    {
        if (validColorText != null)
        {
            validColorText.text = $"Today's Paintings must be mainly made of the color <color={currentValidColor}>{currentValidColor.ToUpper()}</color>";
        }
        else
        {
            Debug.LogWarning("ValidColorText is not assigned in the RatingManager!");
        }
    }
}
