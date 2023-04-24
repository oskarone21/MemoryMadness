using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardCounter : MonoBehaviour
{
    public TextMeshProUGUI cardCounter;
    private int cardsLeft;
    private MemMad memMad;

    // Start is called before the first frame update
    private void Start()
    {
        memMad = FindObjectOfType<MemMad>();
    }

    void Update()
    {
        cardsLeft = memMad.deck.Count;
        cardCounter.text = "Cards Left: " + cardsLeft;
    }
}
