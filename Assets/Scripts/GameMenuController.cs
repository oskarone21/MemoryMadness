using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public GameObject gameMenuPanel;
    public bool isMusicOn = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameMenu();
        }
    }

    public void ToggleGameMenu()
    {
        gameMenuPanel.SetActive(!gameMenuPanel.activeSelf);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleMusic()
    {
        // Add your logic to turn the music on/off here
        isMusicOn = !isMusicOn;
    }

    public void Exit()
    {
        Application.Quit();
    }
}