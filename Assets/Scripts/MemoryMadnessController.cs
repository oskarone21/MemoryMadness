using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MemoryMadnessController : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;

    public GameObject[] topPos;
    public GameObject[] middlePos;
    public GameObject[] bottomPos;
    public GameObject[] handPos;

    private static readonly string[] suits = { "C", "D", "H", "S" };
    private static readonly string[] values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    private CardCounter cardCounter;
    private List<string>[][] cardPositions;
    public List<string> deck;

    private void AddCardTo(ICollection<string> cardList)
    {
        cardList.Add(deck.Last());
        deck.RemoveAt(deck.Count - 1);
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
            SetCardPositions();

            AddCardTo(cardPositions[0][0]); // Add card to the first position in the top row
            AddCardTo(cardPositions[3][0]); // Add card to the first position in the hand

            // Update the card count
            cardCounter.UpdateCardCount();

            // Repopulate the grid and hand with the remaining cards
            StartCoroutine(MemMadDeal(true));
        }
    }

    private IEnumerator DealToPositions(IReadOnlyList<List<string>> cardLists, IReadOnlyList<GameObject> positions)
    {
        for (int i = 0; i < cardLists.Count; i++)
        {
            float zOffset = 0.03f;
            foreach (string card in cardLists[i])
            {
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
            yield return StartCoroutine(DealToPositions(cardPositions[0], topPos));
            yield return StartCoroutine(DealToPositions(cardPositions[3], handPos));
        }
        else
        {
            yield return StartCoroutine(DealToPositions(cardPositions[0], topPos));
            yield return StartCoroutine(DealToPositions(cardPositions[1], middlePos));
            yield return StartCoroutine(DealToPositions(cardPositions[2], bottomPos));
            yield return StartCoroutine(DealToPositions(cardPositions[3], handPos));
        }
    }

    private void MemMadSort()
    {
        for (int i = 0; i < 3; i++)
        {
            AddCardTo(cardPositions[2][i]); // Bottom
            AddCardTo(cardPositions[1][i]); // Middle
            AddCardTo(cardPositions[0][i]); // Top
        }

        for (int i = 0; i < 2; i++)
        {
            AddCardTo(cardPositions[3][i]); // Hand
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
        SetCardPositions();
        PlayCards();
        StartCoroutine(DelayInitialDealFlag());
        cardCounter = FindObjectOfType<CardCounter>();
    }

    private void SetCardPositions() => cardPositions = new[]
        {
            new[] { new List<string>(), new List<string>(), new List<string>() }, // Tops
            new[] { new List<string>(), new List<string>(), new List<string>() }, // Middles
            new[] { new List<string>(), new List<string>(), new List<string>() }, // Bottoms
            new[] { new List<string>(), new List<string>() } // Hands
        };


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

    public void ShowEndGameMenu()
    {
        // Display the end game menu with the total score
        // Implement your end game menu logic here
    }

}

