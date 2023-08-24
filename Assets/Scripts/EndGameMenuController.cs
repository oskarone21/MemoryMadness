using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EndGameMenuController : MonoBehaviour
{
    [FormerlySerializedAs("endGameMenu")] public GameObject __EndGameMenu;
    [FormerlySerializedAs("scoreText")] public TMP_Text __ScoreText;
    public TMP_InputField __InputField;
    public LeaderboardManager __LeaderboardManager;
    private int __Score;

    private void Start()
    {
        __Score = PlayerPrefs.GetInt("Score");

        if(__Score < 0)
        {
            __ScoreText.text = $"You lost! Your score is: {__Score}";
        }
        else
        {
            __ScoreText.text += $" {__Score}";
        }
    }

    public void AddUpdateLeaderboard()
    {
        if (__LeaderboardManager != null && __InputField != null)
        {
            __LeaderboardManager.AddEntry(__InputField.text, __Score);

            GameObject _UsernameButton = GameObject.Find("UsernameButton");
            if (_UsernameButton != null)
            {
                _UsernameButton.SetActive(false);
            }
        }
        else
        {
            if (__LeaderboardManager == null) Debug.LogError("LeaderboardManager not found");
            if (__InputField == null) Debug.LogError("InputField not assigned");
        }
    }

    public void Quit() =>
        SceneManager.LoadScene("MainMenuScene");

    public void Restart() =>
        SceneManager.LoadScene("FirstScene");
}
