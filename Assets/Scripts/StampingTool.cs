using UnityEngine;

public class StampingTool : MonoBehaviour
{
    public enum Rating { Yes, No }

    [Header("Stamp Settings")]
    [Tooltip("Specify if the stamp rates Yes or No")]
    public Rating ratingType;

    [Tooltip("The trigger zone that the stamp evaluates")]
    public GameObject triggerZoneObject;  // Reference to a separate GameObject that acts as the trigger zone

    private Collider2D triggerZone;  // Reference to the Collider2D of the trigger zone

    private void Start()
    {
        // Get the Collider2D component from the trigger zone object
        triggerZone = triggerZoneObject.GetComponent<Collider2D>();

        if (triggerZone == null)
        {
            Debug.LogError("Trigger zone object does not have a Collider2D component attached!");
        }
    }

    private void OnMouseDown()
    {
        // Check if the GameObject is clicked (assuming it's 2D or 3D with collider)
        Debug.Log("Stamp clicked! Grading artworks in the zone.");
        GradeArtworksInZone();
    }

    private void GradeArtworksInZone()
    {
        // Get all the colliders in the trigger zone
        Collider2D[] collidersInZone = Physics2D.OverlapBoxAll(triggerZone.bounds.center, triggerZone.bounds.size, 0f);

        foreach (Collider2D collider in collidersInZone)
        {
            // Check if the object has the "Artwork" tag
            if (collider.CompareTag("Artwork"))
            {
                Artwork artwork = collider.GetComponent<Artwork>();
                if (artwork != null)
                {
                    EvaluateArtwork(artwork);
                }
            }
        }
    }

    private void EvaluateArtwork(Artwork artwork)
    {
        // Set the artwork as graded
        artwork.isGraded = true;  // This marks the artwork as graded.

        // Check if the artwork is valid based on the stamp's rating type
        bool isCorrect = (ratingType == Rating.Yes && artwork.isValid) ||
                         (ratingType == Rating.No && !artwork.isValid);

        if (isCorrect)
        {
            Debug.Log("Correct rating for " + artwork.gameObject.name);
        }
        else
        {
            Debug.Log("Incorrect rating for " + artwork.gameObject.name);
        }
    }
}
