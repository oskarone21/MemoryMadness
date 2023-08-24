using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public MemoryMadnessController __MemoryMadnessController;
    
    public void PlayGame()
    {
        SceneManager.LoadScene("FirstScene");
    }

    public void SetGridSize(int value) => PlayerPrefs.SetInt("GridSize", value);

    public void OpenRules()
    {
        SceneManager.LoadScene("RulesScene");
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("LeadershipScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        __MemoryMadnessController = FindObjectOfType<MemoryMadnessController>();
        
        SetGridSize(3);
    }
}
