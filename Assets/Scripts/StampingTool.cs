using UnityEngine;

public class StampingTool : MonoBehaviour
{
    public enum Rating { Yes, No }

    [Header("Stamp Settings")]
    [Tooltip("Specify if the stamp rates Yes or No")]
    public Rating ratingType;

    [Tooltip("The trigger zone that the stamp evaluates")]
    public GameObject triggerZoneObject;

    private Collider2D triggerZone;

    private void Start()
    {
        triggerZone = triggerZoneObject.GetComponent<Collider2D>();

        if (triggerZone == null)
        {
            Debug.LogError("Trigger zone object does not have a Collider2D component attached!");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Stamp clicked! Grading artworks in the zone.");
        GradeArtworksInZone();
    }

    private void GradeArtworksInZone()
    {
        Collider2D[] collidersInZone = Physics2D.OverlapBoxAll(triggerZone.bounds.center, triggerZone.bounds.size, 0f);

        foreach (Collider2D collider in collidersInZone)
        {
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
        artwork.isGraded = true;

        // Check grading correctness
        artwork.wasGradedCorrectly = (ratingType == Rating.Yes && artwork.isValid) ||
                                     (ratingType == Rating.No && !artwork.isValid);

        if (artwork.wasGradedCorrectly)
        {
            Debug.Log("Correct rating for " + artwork.gameObject.name);
        }
        else
        {
            Debug.Log("Incorrect rating for " + artwork.gameObject.name);
        }
    }
}