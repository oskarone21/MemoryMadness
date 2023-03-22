using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{

    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private MemMad memMad;
    private UserInput userInput;



    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = MemMad.GenerateDeck();
        memMad = FindObjectOfType<MemMad>();
        userInput = FindObjectOfType<UserInput>();

        int i = 0;
        foreach (string card in deck){
            if (this.name == card){
                cardFace = memMad.cardFaces[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
    }


    // Update is called once per frame
    void Update()
    {
        if (selectable.faceUp == true){
            spriteRenderer.sprite = cardFace;
        }
        else {
            spriteRenderer.sprite = cardBack;
        }
        if (userInput.selectedHandCard){
            if (name == userInput.selectedHandCard.name){
                spriteRenderer.color = Color.yellow;
            }
            else {
                spriteRenderer.color = Color.white;
            }
        }
    }
}
