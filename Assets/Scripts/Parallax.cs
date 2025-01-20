using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private float parallaxIntensity = 10f; // How much the object moves
    [SerializeField] private Camera mainCamera; // Reference to the main camera
    [SerializeField] private Vector2 maxOffset = new Vector2(20f, 20f); // Maximum movement bounds

    private Vector3 initialPosition;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Auto-assign the main camera if not set
        }

        // Store the initial position of the object
        initialPosition = transform.position;
    }

    private void Update()
    {
        ApplyParallaxEffect();
    }

    private void ApplyParallaxEffect()
    {
        if (mainCamera == null) return;

        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Normalize the mouse position relative to the screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector2 normalizedMousePosition = new Vector2(
            (mousePosition.x / screenWidth) - 0.5f,
            (mousePosition.y / screenHeight) - 0.5f
        );

        // Calculate the parallax offset
        Vector2 parallaxOffset = new Vector2(
            normalizedMousePosition.x * parallaxIntensity,
            normalizedMousePosition.y * parallaxIntensity
        );

        // Clamp the offset within the max bounds
        parallaxOffset = Vector2.ClampMagnitude(parallaxOffset, Mathf.Max(maxOffset.x, maxOffset.y));

        // Apply the parallax offset to the object's position
        transform.position = new Vector3(
            initialPosition.x + parallaxOffset.x,
            initialPosition.y + parallaxOffset.y,
            initialPosition.z
        );
    }
}