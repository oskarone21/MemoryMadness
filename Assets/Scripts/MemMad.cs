using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MemMad : MonoBehaviour
{

    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject[] topPos;
    public GameObject[] middlePos;
    public GameObject[] bottomPos;
    public GameObject[] handPos;


    public static string[] suits = new string[] { "C", "D", "H", "S"};
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    public List<string>[] tops;
    public List<string>[] middles;
    public List<string>[] bottoms;
    public List<string>[] handcards;

    private List<string> top0 = new List<string>();
    private List<string> top1 = new List<string>();
    private List<string> top2 = new List<string>();
    private List<string> middle0 = new List<string>();
    private List<string> middle1 = new List<string>();
    private List<string> middle2 = new List<string>();
    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> hand0 = new List<string>();
    private List<string> hand1 = new List<string>();

    public List<string> deck;

    // Start is called before the first frame update
    void Start()
    {
        tops = new List<string>[] {top0, top1, top2 };
        middles = new List<string>[] {middle0, middle1, middle2};
        bottoms = new List<string>[] {bottom0, bottom1, bottom2};
        handcards = new List<string>[] {hand0, hand1};
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards(){
        deck = GenerateDeck();
        Shuffle(deck);

        // //test the cards in the deck
        // foreach (string card in deck){
        //     print (card);
        // }
        MemMadSort();
        StartCoroutine(MemMadDeal());
    }

    public static List<string> GenerateDeck(){

        List<string> newDeck = new List<string>();
        foreach (string s in suits){
            foreach (string v in values){
                newDeck.Add(s+v);
            }
        }
        return newDeck;
    }

    void Shuffle<T>(List<T> list){
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1){
            int k = random.Next(n);
            n --;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    IEnumerator MemMadDeal(){

        for (int i = 0; i < 3; i++){

            float zOffset = 0.03f;
            foreach (string card in tops[i]){

                yield return new WaitForSeconds(0.05f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(topPos[i].transform.position.x, topPos[i].transform.position.y, topPos[i].transform.position.z - zOffset), Quaternion.identity, topPos[i].transform);
                newCard.name = card;
                newCard.GetComponent<Selectable>().faceUp = true;

                zOffset = zOffset + 0.03f;
            }
        }

        for (int i = 0; i < 3; i++){

            float zOffset = 0.03f;
            foreach (string card in middles[i]){

                yield return new WaitForSeconds(0.05f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(middlePos[i].transform.position.x, middlePos[i].transform.position.y, middlePos[i].transform.position.z - zOffset), Quaternion.identity, middlePos[i].transform);
                newCard.name = card;
                newCard.GetComponent<Selectable>().faceUp = true;

                zOffset = zOffset + 0.03f;
            }
        }

        for (int i = 0; i < 3; i++){

            float zOffset = 0.03f;
            foreach (string card in bottoms[i]){

                yield return new WaitForSeconds(0.05f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                newCard.name = card;
                newCard.GetComponent<Selectable>().faceUp = true;

                zOffset = zOffset + 0.03f;
            }
        }

        for (int i = 0; i < 2; i++){

            float zOffset = 0.03f;
            foreach (string card in handcards[i]){

                yield return new WaitForSeconds(0.05f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(handPos[i].transform.position.x, handPos[i].transform.position.y, handPos[i].transform.position.z - zOffset), Quaternion.identity, handPos[i].transform);
                newCard.name = card;
                newCard.GetComponent<Selectable>().faceUp = true;

                zOffset = zOffset + 0.03f;
            }
        }

    }

    void MemMadSort(){
        for (int i = 0; i < 3; i++ ){
            bottoms[i].Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            
        }

        for (int i = 0; i < 3; i++ ){
            middles[i].Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            
        }

        for (int i = 0; i < 3; i++ ){
            tops[i].Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            
        }

        for (int i = 0; i < 2; i++ ){
            handcards[i].Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            
        }
    }
}
