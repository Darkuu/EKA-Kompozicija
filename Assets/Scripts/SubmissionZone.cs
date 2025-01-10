using UnityEngine;

public class SubmissionZone : MonoBehaviour
{
    [SerializeField] private float correctArtworkTimeBonus = 20f;
    [SerializeField] private float incorrectArtworkTimePenalty = -5f;

    public MoneyManager moneyManager;
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
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
            moneyManager.AddMoney(100f);
            gameManager.ModifyTimer(correctArtworkTimeBonus);
        }
        else
        {
            gameManager.ModifyTimer(incorrectArtworkTimePenalty);
        }

        Destroy(artwork.gameObject);
    }
}