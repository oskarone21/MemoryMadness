using UnityEngine;
using UnityEngine.UI;

public class GameMenuButtonController : MonoBehaviour
{
    public GameObject gameMenuPanel;
    public MemoryMadnessController memoryMadnessController;
    
    public void Reshuffle()
    {
        memoryMadnessController.ReplaceCards();
    }

    private void Update()
    {
        if (!memoryMadnessController.GetLastDeal())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Reshuffle();
            }   
        }
        else
        {
            GameObject _ReshuffleButton = GameObject.Find("ReshuffleButton");
            if (_ReshuffleButton != null)
            {
                _ReshuffleButton.GetComponent<Button>().gameObject.SetActive(false);
            }
            gameMenuPanel.SetActive(false);
        }
    }

    private void Start()
    {
        memoryMadnessController = FindObjectOfType<MemoryMadnessController>();
    }
}