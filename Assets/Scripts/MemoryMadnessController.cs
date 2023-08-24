using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MemoryMadnessController : MonoBehaviour
{
    private const int HAND_SIZE = 2;
    
    private CardCounter __CardCounter;
    private List<string>[][] __CardPositions;
    public List<string> __Deck;
    public int __GridSize;
    private bool __LastDeal = false;

    private GameObject[][] __Cards;
    [FormerlySerializedAs("cardFaces")]
    public Sprite[] __CardFaces;
    [FormerlySerializedAs("cardPrefab")]
    public GameObject __CardPrefab;
    [FormerlySerializedAs("handPos")]
    public GameObject[] __HandPos;

    [FormerlySerializedAs("rows")]
    public GameObject[] __Rows;
    
    private static readonly string[] __Suits = { "C", "D", "H", "S" };
    private static readonly string[] __Values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    
    public bool GetLastDeal() =>
        __LastDeal;

    public void SetGridSize(int size) =>
        __GridSize = size;
    
    private void AddCardTo(ICollection<string> cardList)
    {
        cardList.Add(__Deck.Last());
        __Deck.RemoveAt(__Deck.Count - 1);
    }
     
    private static IEnumerator DelayInitialDealFlag()
    {
        // Wait for 3 seconds to ensure the cards have been dealt
        yield return new WaitForSeconds(3);
    }
    
    private GameObject[] GetActiveCards() => GameObject.FindGameObjectsWithTag("Card");

    public void ReplaceCards()
    {
        CardsReshuffled = true;
        GameObject[] _Cards = GetActiveCards();
        int _CardsLeft = 0;
        foreach (GameObject card in _Cards)
        {
            card.SetActive(false);
        }

        SetGridSize();
        
        int _GridSize = __GridSize * __GridSize + HAND_SIZE;
        
        __CardCounter.UpdateCardCount(__Deck.Count - 2);
        
        if (__Deck.Count >= _GridSize)
        {
            MemMadSort();
            StartCoroutine(MemMadDeal());
        }
        else
        {
            __LastDeal = true;
            switch (__GridSize)
            {
                case 4:
                    _CardsLeft = 14;
                    MemMadSort(true, _CardsLeft);
                    break;
                case 3:
                    _CardsLeft = 6;
                    MemMadSort(true, _CardsLeft);
                    break;
                case 2:
                    _CardsLeft = 4;
                    MemMadSort(true, _CardsLeft);
                    break;
            }
            
            // Repopulate the grid and hand with the remaining cards
            StartCoroutine(MemMadDeal(true, _CardsLeft));
        }
    }

    private IEnumerator DealToPositions(IReadOnlyList<List<string>> cardLists, IReadOnlyList<GameObject> positions, bool isHandCard = false)
    {
        if (positions.Count > 0)
        {
            // int _GridSize = isHandCard ? HAND_SIZE : __GridSize;
            
            int gridSizeToPopulate = Mathf.Min(cardLists.Count, positions.Count);

            // Loop through the rows/columns based on the size calculated
            for (int i = 0; i < gridSizeToPopulate; i++)
            {
                float zOffset = 0.03f;
                foreach (string card in cardLists[i])
                {
                    yield return new WaitForSeconds(0.05f);
                
                    GameObject newCard = Instantiate(__CardPrefab, positions[i].transform.position + new Vector3(0, 0, -zOffset), Quaternion.identity, positions[i].transform);
                    newCard.name = card;
                    newCard.GetComponent<Selectable>().faceUp = true;
                    zOffset += 0.03f;
                }
            }
        }
    }

    public static List<string> GenerateDeck() => 
        __Suits.SelectMany(_ => __Values, (suit, value) => suit + value).ToList();

    private  IEnumerator MemMadDeal(bool lastDeal = false, int cardsLeft = 0)
    {
        if (lastDeal)
        {
            int rowsToPopulate = cardsLeft / __GridSize,
                columnsToPopulate = cardsLeft % __GridSize;
            
            for (int i = 0; i < rowsToPopulate; i++)
            {
                yield return StartCoroutine(DealToPositions(__CardPositions[i], __Rows[i].GetComponentsInChildren<Transform>().Skip(1).Select(t => t.gameObject).ToArray()));
            }
            
            if (columnsToPopulate > 0)
            {
                yield return StartCoroutine(DealToPositions(__CardPositions[rowsToPopulate], __Rows[rowsToPopulate].GetComponentsInChildren<Transform>().Skip(1).Take(columnsToPopulate).Select(t => t.gameObject).ToArray()));
            }
        }
        else
        {
            for (int i = 0; i < __GridSize; i++)
            {
                yield return StartCoroutine(DealToPositions(__CardPositions[i], __Rows[i].GetComponentsInChildren<Transform>().Skip(1).Select(t => t.gameObject).ToArray()));
            }
        }
        
        yield return StartCoroutine(DealToPositions(__CardPositions[__GridSize], __HandPos, true));
    }
    
    private void MemMadSort(bool lastDeal = false, int cardsLeft = 0)
    {
        if (lastDeal)
        {
            int rowsToPopulate = cardsLeft / __GridSize,
                columnsToPopulate = cardsLeft % __GridSize;
            
            for (int _row = 0; _row < rowsToPopulate; _row++)
            {
                for (int _col = 0; _col < __GridSize; _col++)
                {
                    AddCardTo(__CardPositions[_row][_col]);
                }
            }

            // Populate columns in the last row based on remaining cards
            for (int _col = 0; _col < columnsToPopulate; _col++)
            {
                AddCardTo(__CardPositions[rowsToPopulate][_col]);
            }
        }
        else
        {
            for (int _row = 0; _row < __GridSize; _row++)
            {
                for (int _col = 0; _col < __GridSize; _col++)
                {
                    AddCardTo(__CardPositions[_row][_col]);
                }
            }
        }

        for (int i = 0; i < 2; i++)
        {
            AddCardTo(__CardPositions[__GridSize][i]);
        }
    }

    public void PlayCards()
    {
        Shuffle(__Deck);
        MemMadSort();
        StartCoroutine(MemMadDeal());
    }

    private static void Shuffle<T>(IList<T> list)
    {
        System.Random random = new();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n--);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private void SetGridSize()
    {
        __GridSize = Mathf.Clamp(__GridSize, 2, 4); // Ensure the grid size is within the valid range

        // Initialize the cardPositions array to match the gridSize
        __CardPositions = new List<string>[__GridSize + 1][];
        for (int i = 0; i < __GridSize; i++)
        {
            __CardPositions[i] = new List<string>[__GridSize];
            for (int j = 0; j < __GridSize; j++)
            {
                __CardPositions[i][j] = new List<string>();
            }
        }

        // Initialize the hand positions separately
        __CardPositions[__GridSize] = new List<string>[2]; 
        for (int i = 0; i < HAND_SIZE; i++)
        {
            __CardPositions[__GridSize][i] = new List<string>();
        }

        // Initialize the cards array to match the gridSize
        __Cards = new GameObject[__GridSize][]; 
        for (int i = 0; i < __GridSize; i++)
        {
            __Cards[i] = new GameObject[__GridSize]; 
        }

        // Assign the card objects to the 2D array
        for (int i = 0; i < __GridSize; i++)
        {
            for (int j = 0; j < __GridSize; j++)
            {
                __Cards[i][j] = __Rows[i].transform.GetChild(j).gameObject;
            }
        }

        // Hide all rows and cards not in use
        for (int row = 0; row < __Rows.Length; row++)
        {
            __Rows[row].SetActive(false);
            for (int col = 0; col < __Rows[row].transform.childCount; col++)
            {
                __Rows[row].transform.GetChild(col).gameObject.SetActive(false);
            }
        }

        // Show the rows and cards in use
        for (int row = 0; row < __GridSize; row++)
        {
            __Rows[row].SetActive(true);
            for (int col = 0; col < __GridSize; col++)
            {
                __Cards[row][col].SetActive(true);
            }
        }
    }
    
    private void Start()
    {
        __CardCounter = FindObjectOfType<CardCounter>();
        __GridSize = PlayerPrefs.GetInt("GridSize", 3);

        __Deck = GenerateDeck();
        
        SetGridSize();
        PlayCards();
        __CardCounter.UpdateCardCount(52);
        StartCoroutine(DelayInitialDealFlag()); // start the game with the set grid size
    }
    
    public bool CardsReshuffled { get; set; } = true;
}