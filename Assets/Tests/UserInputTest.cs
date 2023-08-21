using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UserInputTest 
{

    public UserInput userinput;    
    public GameObject selectedHandCard;

    
    [Test]
    public void AreColorsMatchingTest()
    {
        userinput = new UserInput();
        char handSuit = 'C';
        char suit = 'S';
        Assert.IsTrue(userinput.AreColorsMatching(handSuit, suit));
    }

    [Test]
    public void ClickGameCard()
    {}

}
