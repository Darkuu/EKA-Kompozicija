using UnityEngine;

public class SubmissionZone : MonoBehaviour
{
    [SerializeField] private float correctArtworkTimeBonus = 20f;
    [SerializeField] private float incorrectArtworkTimePenalty = -5f;

    public MoneyManager moneyManager;
    public GameManager gameManager;
    
    [Header("Submission sounds")]
    public AudioSource submitSoundPositive;
    public AudioSource submitSoundNegative;

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
            submitSoundPositive.Play();
        }
        else
        {
            gameManager.ModifyTimer(incorrectArtworkTimePenalty);
            submitSoundNegative.Play();
        }
        gameManager.SubmitArtwork();
        Destroy(artwork.gameObject);
    }
}