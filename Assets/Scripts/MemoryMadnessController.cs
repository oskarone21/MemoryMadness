using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoryMadnessController : MonoBehaviour
{
    private CardCounter cardCounter;
    public int gridSize;

    public Sprite[] cardFaces;
    public GameObject cardPrefab;

    public GameObject[] firstRow;
    public GameObject[] secondRow;
    public GameObject[] thirdRow;
    public GameObject[] fourthRow;
    public GameObject[] handPos;

    public GameObject[] rows;
    public GameObject[][] cards;

    private static readonly string[] suits = { "C", "D", "H", "S" };
    private static readonly string[] values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string> cardsInPlay = new List<string>();

    private List<string>[][] cardPositions;
    private List<string>[] handPositions;

    public List<string> deck;

    private void AddCardTo(ICollection<string> cardList)
    {
        cardList.Add(deck.Last());
        deck.RemoveAt(deck.Count - 1);
    }

    public void ReshuffleOnButtonClick()
    {
        // Add cards currently in play back to the deck.
        deck.AddRange(cardsInPlay);

        // Now, clear the cards in play.
        cardsInPlay.Clear();

        // Now shuffle the deck again.
        Shuffle(deck);

        // Proceed to reshuffle and deal the cards as before.
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject card in cards)
        {
            string cardName = card.transform?.parent?.name ?? string.Empty;

            if (!cardName.Equals(Constants.HAND_0, StringComparison.OrdinalIgnoreCase) && !cardName.Equals(Constants.HAND_1, StringComparison.OrdinalIgnoreCase))
            {
                Destroy(card);
            }
        }

        SetCardPositions();
        MemMadSort();
        StartCoroutine(MemMadDeal());
    }

    public void ReplaceCards()
    {
        // Clear the grid
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject card in cards)
        {
            Destroy(card);
        }

        SetCardPositions();

        cardCounter.UpdateCardCount();

        if (deck.Count > 8)
        {
            MemMadSort();
            StartCoroutine(MemMadDeal());
        }
        else
        {
            AddCardTo(cardPositions[0][0]); // Add card to the first position in the top row
            AddCardTo(cardPositions[3][0]); // Add card to the first position in the hand

            // Update the card count
            cardCounter.UpdateCardCount();

            // Repopulate the grid and hand with the remaining cards
            StartCoroutine(MemMadDeal(true));
        }
    }

    private IEnumerator DealToPositions(IReadOnlyList<List<string>> cardLists, IReadOnlyList<GameObject> positions, bool isHandCard = false)
    {
        for (int i = 0; i < gridSize; i++)
        {
            float zOffset = 0.03f;
            foreach (string card in cardLists[i])
            {
                if (!isHandCard)
                {
                    cardsInPlay.Add(card);
                }
                yield return new WaitForSeconds(0.05f);
                GameObject newCard = Instantiate(cardPrefab, positions[i].transform.position + new Vector3(0, 0, -zOffset), Quaternion.identity, positions[i].transform);
                newCard.name = card;
                newCard.GetComponent<Selectable>().faceUp = true;
                zOffset += 0.03f;
            }
        }
    }

    public static List<string> GenerateDeck() => 
        suits.SelectMany(_ => values, (suit, value) => suit + value).ToList();

    private IEnumerator MemMadDeal(bool lastDeal = false)
    {
        if (lastDeal)
        {
            yield return StartCoroutine(DealToPositions(cardPositions[0], rows[0].GetComponentsInChildren<Transform>().Skip(1).Select(t => t.gameObject).ToArray()));
            yield return StartCoroutine(DealToPositions(cardPositions[gridSize], handPos));
        }
        else
        {
            for (int i = 0; i < gridSize; i++)
            {
                yield return StartCoroutine(DealToPositions(cardPositions[i], rows[i].GetComponentsInChildren<Transform>().Skip(1).Select(t => t.gameObject).ToArray()));
            }

            yield return StartCoroutine(DealToPositions(cardPositions[gridSize], handPos, true));
        }
    }


    private void MemMadSort()
    {
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                AddCardTo(cardPositions[row][col]);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            AddCardTo(cardPositions[gridSize][i]); // Hand
        }
    }


    private void PlayCards()
    {
        deck = GenerateDeck();
        Shuffle(deck);
        MemMadSort();
        StartCoroutine(MemMadDeal());
    }

    private void Start()
    {
        gridSize = Mathf.Clamp(gridSize, 2, 4); // ensure the grid size is within valid range

        cardPositions = new List<string>[gridSize + 1][];
        for (int i = 0; i < gridSize; i++)
        {
            cardPositions[i] = new List<string>[gridSize];
            for (int j = 0; j < gridSize; j++)
            {
                cardPositions[i][j] = new List<string>();
            }
        }

        cardPositions[gridSize] = new List<string>[2]; // this is for the hand
        for (int i = 0; i < 2; i++)
        {
            cardPositions[gridSize][i] = new List<string>();
        }

        cardCounter = FindObjectOfType<CardCounter>();

        cards = new GameObject[gridSize][]; // adjust the cards array to match gridSize
        for (int i = 0; i < gridSize; i++)
        {
            cards[i] = new GameObject[gridSize]; // adjust the card row to match gridSize
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                cards[i][j] = rows[i].transform.GetChild(j).gameObject;
            }
        }

        foreach (GameObject row in rows.Take(gridSize))
        {
            row.SetActive(false);
        }
        foreach (GameObject[] cardRow in cards)
        {
            foreach (GameObject card in cardRow)
            {
                card.SetActive(false);
            }
        }

        SetGridSize(gridSize); // set up the grid size
        StartCoroutine(DelayInitialDealFlag()); // start the game with the set grid size
    }


    private void SetCardPositions()
    {
        cardPositions = new List<string>[gridSize][];

        for (int i = 0; i < gridSize; i++)
        {
            cardPositions[i] = new List<string>[gridSize];

            for (int j = 0; j < gridSize; j++)
            {
                cardPositions[i][j] = new List<string>();
            }
        }

        // For hand positions
        cardPositions[gridSize] = new List<string>[2];

        for (int j = 0; j < 2; j++)
        {
            cardPositions[gridSize][j] = new List<string>();
        }
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
    
    private IEnumerator DelayInitialDealFlag()
    {
        // Wait for 3 seconds to ensure the cards have been dealt
        yield return new WaitForSeconds(3);
    }

    public void SetGridSize(int size)
{
    gridSize = Mathf.Clamp(size, 2, 4); // Ensure the grid size is within the valid range

    // Initialize the cardPositions array to match the gridSize
    cardPositions = new List<string>[gridSize + 1][];
    for (int i = 0; i < gridSize; i++)
    {
        cardPositions[i] = new List<string>[gridSize];
        for (int j = 0; j < gridSize; j++)
        {
            cardPositions[i][j] = new List<string>();
        }
    }

    // Initialize the hand positions separately
    cardPositions[gridSize] = new List<string>[2]; 
    for (int i = 0; i < 2; i++)
    {
        cardPositions[gridSize][i] = new List<string>();
    }

    // Initialize the cards array to match the gridSize
    cards = new GameObject[gridSize][]; 
    for (int i = 0; i < gridSize; i++)
    {
        cards[i] = new GameObject[gridSize]; 
    }

    // Assign the card objects to the 2D array
    for (int i = 0; i < gridSize; i++)
    {
        for (int j = 0; j < gridSize; j++)
        {
            cards[i][j] = rows[i].transform.GetChild(j).gameObject;
        }
    }

    // Hide all rows and cards not in use
    for (int row = 0; row < rows.Length; row++)
    {
        rows[row].SetActive(false);
        for (int col = 0; col < rows[row].transform.childCount; col++)
        {
            rows[row].transform.GetChild(col).gameObject.SetActive(false);
        }
    }

    // Show the rows and cards in use
    for (int row = 0; row < gridSize; row++)
    {
        rows[row].SetActive(true);
        for (int col = 0; col < gridSize; col++)
        {
            cards[row][col].SetActive(true);
        }
    }

    // Play the cards to fill the grid
    PlayCards();
}

}


