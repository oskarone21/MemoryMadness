using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Diagnostics;

public class SceneChangingTest
{
    public Stopwatch stopwatch;
    public long maxTime;

    [SetUp]
    public void Setup()
    {
        stopwatch = new Stopwatch();
        maxTime = 1500;
    }

    [Test]
    public void MainMenuSceneTest()
    {
        stopwatch.Start();
        EditorSceneManager.OpenScene("Assets/MainMenuScene.unity");
        stopwatch.Stop();
        //Sets Active Scene to Current Loaded Scene
        Scene activeScene = SceneManager.GetActiveScene();
        //Checks Active Scene is Correct Scene
        Assert.AreEqual("MainMenuScene", activeScene.name);
        UnityEngine.Debug.Log($"Scene load time: {stopwatch.ElapsedMilliseconds} ms");
        Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, maxTime, $"Scene load time exceeded{maxTime} ms.");
    }

    [Test]
    public void RulesSceneTest()
    {
        stopwatch.Start();
        EditorSceneManager.OpenScene("Assets/RulesScene.unity");
        stopwatch.Stop();
        //Sets Active Scene to Current Loaded Scene
        Scene activeScene = SceneManager.GetActiveScene();
        //Checks Active Scene is Correct Scene
        Assert.AreEqual("RulesScene", activeScene.name);
        UnityEngine.Debug.Log($"Scene load time: {stopwatch.ElapsedMilliseconds} ms");
        Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, maxTime, $"Scene load time exceeded{maxTime} ms.");
    }

    [Test]
    public void FirstSceneTest()
    {
        stopwatch.Start();
        EditorSceneManager.OpenScene("Assets/FirstScene.unity");
        stopwatch.Stop();
        //Sets Active Scene to Current Loaded Scene
        Scene activeScene = SceneManager.GetActiveScene();
        //Checks Active Scene is Correct Scene
        Assert.AreEqual("FirstScene", activeScene.name);
        UnityEngine.Debug.Log($"Scene load time: {stopwatch.ElapsedMilliseconds} ms");
        Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, maxTime, $"Scene load time exceeded{maxTime} ms.");
    }

    [Test]
    public void LeadershipSceneTest()
    {
        stopwatch.Start();
        EditorSceneManager.OpenScene("Assets/LeadershipScene.unity");
        stopwatch.Stop();
        //Sets Active Scene to Current Loaded Scene
        Scene activeScene = SceneManager.GetActiveScene();
        //Checks Active Scene is Correct Scene
        Assert.AreEqual("LeadershipScene", activeScene.name);
        UnityEngine.Debug.Log($"Scene load time: {stopwatch.ElapsedMilliseconds} ms");
        Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, maxTime, $"Scene load time exceeded{maxTime} ms.");
    }
}
