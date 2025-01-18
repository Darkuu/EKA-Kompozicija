using UnityEngine;

public class StampingTool : MonoBehaviour
{
    public enum Rating { Yes, No }

    [Header("Stamp Settings")]
    [Tooltip("Specify if the stamp rates Yes or No")]
    public Rating ratingType;

    [Tooltip("The trigger zone that the stamp evaluates")]
    public GameObject triggerZoneObject;

    [Header("Audio Settings")]
    [Tooltip("Sound to play when the stamp is clicked")]
    public AudioSource stampSound;

    [Header("Sprite Settings")]
    [Tooltip("Sprite to display when the stamp is clicked")]
    public Sprite clickedSprite;

    private Collider2D triggerZone;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    private bool isCooldownActive = false;

    private void Start()
    {
        triggerZone = triggerZoneObject.GetComponent<Collider2D>();

        if (triggerZone == null)
        {
            Debug.LogError("Trigger zone object does not have a Collider2D component attached!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the StampingTool object!");
        }
        else
        {
            defaultSprite = spriteRenderer.sprite;
        }

        if (stampSound == null)
        {
            Debug.LogError("No AudioSource assigned to the stampSound field!");
        }
    }

    private void OnMouseDown()
    {
        if (isCooldownActive)
        {
            Debug.Log("Cooldown active. Please wait before clicking again.");
            return;
        }

        Debug.Log("Stamp clicked! Grading artworks in the zone.");

        isCooldownActive = true;
        Invoke(nameof(ResetCooldown), 0.5f); 
        stampSound.Play();
        spriteRenderer.sprite = clickedSprite;
        
        Invoke(nameof(RevertSprite), 0.2f); 
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

    private void RevertSprite()
    {
            spriteRenderer.sprite = defaultSprite;
    }

    private void ResetCooldown()
    {
        isCooldownActive = false;
    }
}
