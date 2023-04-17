using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private MemoryMadnessController memoryMadness;
    private UserInput userInput;

    // Start is called before the first frame update
    private void Start()
    {
        memoryMadness = FindObjectOfType<MemoryMadnessController>();
        userInput = FindObjectOfType<UserInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();

        AssignCardFace();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCardSprite();
        HighlightSelectedCard();
    }

    private void AssignCardFace()
    {
        List<string> deck = MemoryMadnessController.GenerateDeck();
        int i = 0;

        foreach (string card in deck)
        {
            if (name == card)
            {
                cardFace = memoryMadness.cardFaces[i];
                break;
            }
            i++;
        }
    }

    private void UpdateCardSprite()
    {
        spriteRenderer.sprite = selectable.faceUp ? cardFace : cardBack;
    }

    private void HighlightSelectedCard()
    {
        if (userInput.selectedHandCard)
        {
            spriteRenderer.color = (name == userInput.selectedHandCard.name) ? Color.yellow : Color.white;
        }
    }
}