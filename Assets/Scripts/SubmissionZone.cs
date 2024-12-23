using UnityEngine;

public class SubmissionZone : MonoBehaviour
{
    [SerializeField] private float correctArtworkTimeBonus = 20f;  // Bonus time for correct submission
    [SerializeField] private float incorrectArtworkTimePenalty = -5f; // Time penalty for incorrect submission
    public MoneyManager moneyManager; // Reference to the MoneyManager
    public GameManager gameManager;   // Reference to the GameManager

    private void OnTriggerEnter2D(Collider2D collision) // Use Collider2D for 2D physics
    {
        Artwork artwork = collision.GetComponent<Artwork>();
        if (artwork != null && artwork.isGraded)
        {
            SubmitArtwork(artwork);
        }
    }

    private void SubmitArtwork(Artwork artwork)
    {
        if (artwork.wasGradedCorrectly)
        {
            // Add money and bonus time for correct submissions
            moneyManager.AddMoney(100f); // Adjust the value as necessary
            gameManager.ModifyTimer(correctArtworkTimeBonus);
            Debug.Log("Correct submission! Added money and bonus time.");
        }
        else
        {
            // Deduct time for incorrect submissions
            gameManager.ModifyTimer(incorrectArtworkTimePenalty);
            Debug.Log("Incorrect submission! Time penalty applied.");
        }

        // Destroy artwork after submission
        Destroy(artwork.gameObject);
    }
}