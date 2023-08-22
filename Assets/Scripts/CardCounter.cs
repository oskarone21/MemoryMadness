using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardCounter : MonoBehaviour
{
    [FormerlySerializedAs("cardCounter")] public TextMeshProUGUI __CardCounter;
    public int __CardsLeft;
    public MemoryMadnessController __MemoryMadnessController;
    
    public CardCounter(int cardsLeft)
    {
        __CardsLeft = cardsLeft;
        __CardCounter.text = "Cards Left: " + __CardsLeft;
    }
    
    public void UpdateCardCount(int cardsLeft = 0)
    {
        __CardsLeft--;
        if(__CardsLeft == 0)
        {
            __CardsLeft = 1;
        }

        if (cardsLeft > 0)
        {
            __CardsLeft = cardsLeft;
        }
        
        __CardCounter.text = $"Cards Left: {__CardsLeft}";
    }
}
