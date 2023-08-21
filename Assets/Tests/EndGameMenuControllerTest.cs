using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EndGameMenuControllerTest
{
    public EndGameMenuController egmc;

    [Test]
    public void ShowTest()
    {
        egmc = new EndGameMenuController();
        egmc.scoreText.text = "You lost! Your score is: ";
        egmc.Show(1);

        Assert.AreEqual("You lost! Your score is: 1", egmc.scoreText.text);
    }

}
