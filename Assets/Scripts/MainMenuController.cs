using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("FirstScene");
    }

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
}
