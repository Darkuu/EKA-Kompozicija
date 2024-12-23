using UnityEngine;

public class Artwork : MonoBehaviour
{
    [Header("User Defined Properties")]
    [Tooltip("The color name of the artwork")]
    public string colorName;

    [Tooltip("The style of the artwork")]
    public string style;

    [Tooltip("Indicates whether the artwork meets the current valid color criteria")]
    public bool isValid;

    [Header("Grading Properties")]
    [Tooltip("Tracks if the artwork has been graded")]
    public bool isGraded = false;

    [Tooltip("Tracks if the artwork was graded correctly")]
    public bool wasGradedCorrectly = false;

    private bool isDragging = false;
    private Vector3 offset;

    private void Update()
    {
        CheckValidity();
        HandleDrag();
    }

    /// <summary>
    /// Checks the validity of the artwork based on the current valid color.
    /// </summary>
    private void CheckValidity()
    {
        if (RatingManager.instance != null)
        {
            string currentValidColor = RatingManager.instance.currentValidColor;
            isValid = !string.IsNullOrEmpty(colorName) && colorName.Equals(currentValidColor, System.StringComparison.OrdinalIgnoreCase);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void HandleDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}