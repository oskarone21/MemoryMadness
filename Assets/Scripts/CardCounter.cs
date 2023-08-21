using TMPro;
using UnityEngine;

public class CardCounter : MonoBehaviour
{
    public TextMeshProUGUI cardCounter;
    public int cardsLeft;
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
        if(cardsLeft == 0)
        {
            cardsLeft = 1;
        }
        cardCounter.text = $"Cards Left: {cardsLeft}";
    }
}
