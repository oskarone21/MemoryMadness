using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardCounter : MonoBehaviour
{

    public TextMeshProUGUI cardCounter;
    private int cardsLeft;
    private MemoryMadnessController memoryMadness;

    // Start is called before the first frame update
    void Start()
    {
        memoryMadness = FindObjectOfType<MemoryMadnessController>();
    }

    void Update()
    {
        cardsLeft = memoryMadness.deck.Count;
        cardCounter.text = "Cards Left: " + cardsLeft;
    }
}
