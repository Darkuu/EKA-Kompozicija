using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private float currentMoney = 0f; // Player's current money
    [SerializeField] private TextMeshProUGUI moneyText; // Reference to the UI text displaying the money

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize the money text with the starting value
        UpdateMoneyText();
    }

    // Method to add money to the player
    public void AddMoney(float amount)
    {
        currentMoney += amount;
        UpdateMoneyText();
    }

    // Method to update the money text in the UI
    private void UpdateMoneyText()
    {
            moneyText.text = currentMoney.ToString(); 
    }

    // Optionally, you can also get the current money
    public float GetCurrentMoney()
    {
        return currentMoney;
    }
}