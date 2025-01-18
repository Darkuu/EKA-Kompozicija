using UnityEngine;

public class ClickMoveInOut : MonoBehaviour
{
    [Header("Positions")]
    public Vector3 inPosition;  
    public Vector3 outPosition; 
    public float moveSpeed = 5f;

    [Header("Sounds")]
    public AudioSource moveSoundIn;
    public AudioSource moveSoundOut;

    [Header("Settings")]
    public bool hoverMode = false; // Toggle between hover or click mode

    [Header("Other Object (Optional)")]
    public GameObject otherObject; // Another object that can trigger the movement

    private bool isMovingOut = false; 
    private bool isHovered = false; // Tracks whether the object is being hovered over
    private bool isPlayingSound = false; // Tracks if a sound is already playing

    private void Update()
    {
        if (hoverMode)
        {
            // If in hover mode, slide based on hover state
            if (isHovered)
            {
                MoveTo(outPosition, moveSoundOut);
            }
            else
            {
                MoveTo(inPosition, moveSoundIn);
            }
        }
        else
        {
            // If in click mode, toggle position on click
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // Raycast to check if the click is on the object
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                // Check if the raycast hit this object or the otherObject
                if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject == otherObject))
                {
                    isMovingOut = !isMovingOut; // Toggle the movement state
                    isPlayingSound = false; // Reset the sound playing state when toggling
                }
            }

            // Move based on the current state
            if (isMovingOut)
            {
                MoveTo(outPosition, moveSoundOut);
            }
            else
            {
                MoveTo(inPosition, moveSoundIn);
            }
        }
    }

    private void MoveTo(Vector3 targetPosition, AudioSource sound)
    {
        // Move the object toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Play sound only once at the start of movement
        if (!isPlayingSound && transform.position != targetPosition)
        {
            sound.Play();
            isPlayingSound = true; // Prevent the sound from playing repeatedly
        }

        // Reset sound state when reaching the target position
        if (transform.position == targetPosition)
        {
            isPlayingSound = false;
        }
    }

    private void OnMouseEnter()
    {
        // When the mouse enters the object, set hover state
        if (hoverMode)
        {
            isHovered = true;
        }
    }

    private void OnMouseExit()
    {
        // When the mouse exits the object, reset hover state
        if (hoverMode)
        {
            isHovered = false;
        }
    }
}
