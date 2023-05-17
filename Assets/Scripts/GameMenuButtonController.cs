using UnityEngine;

public class GameMenuButtonController : MonoBehaviour
{
    public GameObject gameMenuPanel;

    public void LoadGameMenu()
    {
        gameMenuPanel.SetActive(true);
    }
}