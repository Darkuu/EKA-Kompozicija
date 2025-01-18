using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RatingManager : MonoBehaviour
{
    public static RatingManager instance;

    [Header("Valid Style Settings")]
    [Tooltip("Current valid style for the day")]
    public string currentValidStyle;

    [Tooltip("TextMeshProUGUI component displaying the current valid style")]
    public TextMeshProUGUI validStyleText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Set the current valid style and update the display
    public void SetCurrentValidStyle(string newStyle)
    {
        currentValidStyle = newStyle;
        UpdateValidStyleDisplay();
    }

    private void UpdateValidStyleDisplay()
    {
        if (validStyleText != null)
        {
            validStyleText.text = $"Current Style: {currentValidStyle}";
        }
    }
}