using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class CardCounterTest
{
    public CardCounter cardCount;
    
    [Test]
    public void UpdateCardCountTest()
    {
        cardCount = new CardCounter();
        cardCount.cardsLeft = 5;
        cardCount.cardCounter.text = "Cards Left: ";

        cardCount.UpdateCardCount();

        Assert.AreEqual("Cards Left: 4", cardCount.cardCounter.text);
        Assert.AreEqual(4,cardCount.cardsLeft);
    }

}
