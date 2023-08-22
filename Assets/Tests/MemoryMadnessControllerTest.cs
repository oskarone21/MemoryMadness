using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MemoryMadnessControllerTest
{
    public MemoryMadnessController mmc;
    public List<string> __Deck;

    [Test]
    public void GenerateDeckTest()
    {
        mmc = new MemoryMadnessController();
        __Deck = MemoryMadnessController.GenerateDeck();

        Assert.AreEqual(52, __Deck.Count);

    }

    [Test]
    public void GetActiveCardsTest()
    {
        GameObject Card = new GameObject();
        Card.tag = "Card";
        mmc = new MemoryMadnessController();

        Assert.AreEqual(1, mmc.GetActiveCards().Length);
    }

}
