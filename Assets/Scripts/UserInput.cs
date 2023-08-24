using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

public class UserInput : MonoBehaviour
{
    private const string HAND_0 = "Hand0";
    private const string HAND_1 = "Hand1";

    private MemoryMadnessController __MemoryMadnessController;

    private CardCounter __CardCounter;
    private PointSystem __PointSys;
    [FormerlySerializedAs("selectedHandCard")] public GameObject __SelectedHandCard;

    private int __MatchedCards = 0;
    
    public bool AreColorsMatching(char handSuit, char suit) =>
        (handSuit == 'C' && suit == 'S') || (handSuit == 'S' && suit == 'C') ||
        (handSuit == 'H' && suit == 'D') || (handSuit == 'D' && suit == 'H');

    
    public void ClickOnHandCard(GameObject selected)
    {
        print("Clicked on hand card");

        if (__MemoryMadnessController.CardsReshuffled && selected != null)
        {
            __SelectedHandCard = selected;
            __MemoryMadnessController.CardsReshuffled = false;
        }
    }

    private void ClickGameCard(GameObject selected)
    {
        int totalGridCards = __MemoryMadnessController.__GridSize * __MemoryMadnessController.__GridSize;
        print("Clicked on game card");

        if (__SelectedHandCard == null) return;
        
        char selectedHandSuit = __SelectedHandCard.name[0];
        char selectedHandNumber = __SelectedHandCard.name[1];

        char selectedSuit = selected.name[0];
        char selectedNumber = selected.name[1];

        if (selectedHandNumber == selectedNumber)
        {
            __PointSys.UpdateScore(3);
            Destroy(selected);
        }
        else if (selectedHandSuit == selectedSuit)
        {
            __PointSys.UpdateScore(2);
            Destroy(selected);
        }
        else if (AreColorsMatching(selectedHandSuit, selectedSuit))
        {
            __PointSys.UpdateScore(1);

            Destroy(selected);
        }
        else
        {
            Destroy(selected);
            __PointSys.UpdateScore(-1);
        }

        selected.SetActive(false);
        __CardCounter.UpdateCardCount();
        __MatchedCards++;

        int _CurrentScore = __PointSys.GetScore();

        if(_CurrentScore < 0)
        {
            ShowEndGameScreen(_CurrentScore);
        }
        
        if (__MatchedCards >= totalGridCards)
        {
            __MatchedCards = 0;
            __MemoryMadnessController.ReplaceCards();
        }

        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        if (cards.Length == 2)
        {
            ShowEndGameScreen(_CurrentScore);
        }
    }

    private void ShowEndGameScreen(int score)
    {
        PlayerPrefs.SetInt("Score", score);

        SceneManager.LoadScene("EndGameScene");
    }
    
    private void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = GetRaycastHit2D();
            if (hit && hit.collider.CompareTag("Card"))
            {
                string parentObjectName = hit.collider.gameObject.transform.parent.gameObject.name;
                if (parentObjectName == HAND_0 || parentObjectName == HAND_1)
                {
                    ClickOnHandCard(hit.collider.gameObject);
                }
                else
                {
                    ClickGameCard(hit.collider.gameObject);
                }
            }
        }
    }

    private RaycastHit2D GetRaycastHit2D()
    {
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }
    
    private void Start()
    {
        __PointSys = FindObjectOfType<PointSystem>();
        __CardCounter = FindObjectOfType<CardCounter>();
        __MemoryMadnessController = FindObjectOfType<MemoryMadnessController>();
    }
    
    // Update is called once per frame
    private void Update() => GetMouseClick();
}
