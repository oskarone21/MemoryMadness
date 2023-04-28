using TMPro;
using UnityEngine;

public class CardCounter : MonoBehaviour
{
    public TextMeshProUGUI cardCounter;
    private int cardsLeft;
    private MemoryMadnessController memMad;

    // Start is called before the first frame update
    private void Start()
    {
        memMad = FindObjectOfType<MemoryMadnessController>();
        cardsLeft = memMad.deck.Count;
        cardCounter.text = "Cards Left: " + cardsLeft;
    }

    public void UpdateCardCount()
    {
        cardsLeft--;
        cardCounter.text = $"Cards Left: {cardsLeft}";
    }
}
