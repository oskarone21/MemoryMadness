using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LeaderboardController : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }
}

