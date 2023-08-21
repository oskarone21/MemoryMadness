using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UserInput : MonoBehaviour
{
    private const string HAND_0 = "Hand0";
    private const string HAND_1 = "Hand1";

    private MemoryMadnessController memoryMadnessController;
    public EndGameMenuController endGameMenuController;

    private CardCounter cardCounter;
    private PointSystem pointSys;
    public GameObject selectedHandCard;

    private int matchedCards = 0;
    private const int totalGridCards = 9;
    
    public bool AreColorsMatching(char handSuit, char suit)
    {
        return (handSuit == 'C' && suit == 'S') || (handSuit == 'S' && suit == 'C') ||
               (handSuit == 'H' && suit == 'D') || (handSuit == 'D' && suit == 'H');
    }
    
    public void ClickOnHandCard(GameObject selected)
    {
        print("Clicked on hand card");

        if (selectedHandCard == null)
        {
            selectedHandCard = selected;
        }
    }

    private void ClickGameCard(GameObject selected)
    {
        print("Clicked on game card");

        if (selectedHandCard == null) return;
        
        char selectedHandSuit = selectedHandCard.name[0];
        char selectedHandNumber = selectedHandCard.name[1];

        char selectedSuit = selected.name[0];
        char selectedNumber = selected.name[1];

        if (selectedHandNumber == selectedNumber)
        {
            pointSys.UpdateScore(3);
            Destroy(selected);
        }
        else if (selectedHandSuit == selectedSuit)
        {
            pointSys.UpdateScore(2);
            Destroy(selected);
        }
        else if (AreColorsMatching(selectedHandSuit, selectedSuit))
        {
            pointSys.UpdateScore(1);

            Destroy(selected);
        }
        else
        {
            Destroy(selected);
            pointSys.UpdateScore(-1);
        }

        selected.SetActive(false);
        cardCounter.UpdateCardCount();
        matchedCards++;

        int _CurrentScore = pointSys.GetScore();

        if(_CurrentScore < 0)
        {
            endGameMenuController.Show(_CurrentScore);
        }
        
        if (matchedCards >= totalGridCards)
        {
            matchedCards = 0;
            memoryMadnessController.ReplaceCards();
        }

        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        if (cards.Length == 1)
        {
            endGameMenuController.Show(_CurrentScore);
            return;
        }
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
        pointSys = FindObjectOfType<PointSystem>();
        cardCounter = FindObjectOfType<CardCounter>();
        memoryMadnessController = FindObjectOfType<MemoryMadnessController>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        GetMouseClick();
    }
}
