using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class PointSystemTest
{

    public PointSystem ps;

    [Test]
    public void GetScoreTest()
    {
        ps = new PointSystem();
        ps.score = 0;

        Assert.AreEqual(0, ps.GetScore());

    }

}
