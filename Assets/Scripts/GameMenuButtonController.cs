using UnityEngine;

public class GameMenuButtonController : MonoBehaviour
{
    public GameObject gameMenuPanel;

    public void LoadGameMenu()
    {
        gameMenuPanel.SetActive(true);
    }

    public void Reshuffle()
    {
        MemoryMadnessController memoryMadnessController = FindObjectOfType<MemoryMadnessController>();
        memoryMadnessController.ReshuffleOnButtonClick();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Reshuffle();
        }
    }
}