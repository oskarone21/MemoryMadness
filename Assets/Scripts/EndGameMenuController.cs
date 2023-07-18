using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenuController : MonoBehaviour
{
    public GameObject endGameMenu;
    public TMP_Text scoreText;

    public void Show(int score)
    {
        if(score < 0)
        {
            scoreText.text = $"You lost! Your score is: {score}";
        }
        else
        {
            scoreText.text += $" {score}";
        }

        endGameMenu.SetActive(true);
    }

    public void Quit() =>
        SceneManager.LoadScene("MainMenuScene");

    public void Restart() =>
        SceneManager.LoadScene("FirstScene");
}
