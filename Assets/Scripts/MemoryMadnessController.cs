using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Burst.CompilerServices;
using System;

public class MemoryMadnessController : MonoBehaviour
{
    private CardCounter cardCounter;
    public int gridSize = 4;

    public Sprite[] cardFaces;
    public GameObject cardPrefab;

    public GameObject[] topPos;
    public GameObject[] upperMiddlePos;
    public GameObject[] lowerMiddlePos;
    public GameObject[] bottomPos;
    public GameObject[] handPos;

    private static readonly string[] suits = { "C", "D", "H", "S" };
    private static readonly string[] values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string> cardsInPlay = new List<string>();

    private List<string>[][] cardPositions;
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
        StartCoroutine(MemMadDeal(autoReshuffle: false));
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
        for (int i = 0; i < cardLists.Count; i++)
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

    private IEnumerator MemMadDeal(bool lastDeal = false, bool autoReshuffle = true)
    {
        //could create tempTopPos and do for loop to populate with records until reach gridsize
        //else would have to re-do DealToPositions
        GameObject[] tempTopPos = new GameObject[gridSize];
        GameObject[] tempUpperMiddlePos = new GameObject[gridSize];
        GameObject[] tempLowerMiddlePos = new GameObject[gridSize];
        GameObject[] tempBottomPos = new GameObject[gridSize];
        
        for (int i = 0; i < gridSize; i++){
            tempTopPos[i] = topPos[i];
            tempUpperMiddlePos[i] = upperMiddlePos[i];
            tempLowerMiddlePos[i] = lowerMiddlePos[i];
            tempBottomPos[i] = bottomPos[i];
        }

        if (lastDeal)
        {
            yield return StartCoroutine(DealToPositions(cardPositions[0], topPos));
            yield return StartCoroutine(DealToPositions(cardPositions[4], handPos));
        }
        else
        {
            //yield return StartCoroutine(DealToPositions(cardPositions[0], topPos));
            //yield return StartCoroutine(DealToPositions(cardPositions[1], upperMiddlePos));
            //yield return StartCoroutine(DealToPositions(cardPositions[2], bottomPos));

            //if (gridSize >= 2)
            //{
            //    yield return StartCoroutine(DealToPositions(cardPositions[0], tempTopPos));
            //    yield return StartCoroutine(DealToPositions(cardPositions[1], tempUpperMiddlePos));
            //} 
            //if (gridSize >= 3)
            //{
            //    yield return StartCoroutine(DealToPositions(cardPositions[2], tempLowerMiddlePos));
            //}
            if (gridSize == 4)
            {
                yield return StartCoroutine(DealToPositions(cardPositions[0], tempTopPos));
                yield return StartCoroutine(DealToPositions(cardPositions[1], tempUpperMiddlePos));
                yield return StartCoroutine(DealToPositions(cardPositions[2], tempLowerMiddlePos));
                yield return StartCoroutine(DealToPositions(cardPositions[3], tempBottomPos));
            }

            //if (autoReshuffle)
            //{
            //    yield return StartCoroutine(DealToPositions(cardPositions[4], handPos, true));
            //}
        }
    }


    private void MemMadSort()
    {
        for (int i = 0; i < 4; i++)
        {
            AddCardTo(cardPositions[3][i]); // Bottom
            AddCardTo(cardPositions[2][i]); // LowerMiddle
            AddCardTo(cardPositions[1][i]); // UpperMiddle
            AddCardTo(cardPositions[0][i]); // Top
        }

        for (int i = 0; i < 2; i++)
        {
            AddCardTo(cardPositions[4][i]); // Hand
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

    private void SetCardPositions()
    {
        new[] { new List<string>(), new List<string>(), new List<string>() }, // Tops
        new[] { new List<string>(), new List<string>(), new List<string>() }, // upperMiddles
        new[] { new List<string>(), new List<string>(), new List<string>() }, // lowerMiddles
        new[] { new List<string>(), new List<string>(), new List<string>() }, // Bottoms
        new[] { new List<string>(), new List<string>() } // Hands

        // cardPositions = new List<string>[gridSize][];

        // for (int i = 0; i < gridSize; i++){
        //     cardPositions[i] = new List<string>[gridSize];
        //     for (int j = 0; j < gridSize; j++){
        //         cardPositions[i][j] = new List<string>();
        //     }
        // }
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
}


