using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UpdateSprite : MonoBehaviour
{
    [FormerlySerializedAs("cardFace")] public Sprite _CardFace;
    [FormerlySerializedAs("cardBack")] public Sprite __CardBack;

    private SpriteRenderer __SpriteRenderer;
    private Selectable __Selectable;
    private MemoryMadnessController __MemoryMadness;
    private UserInput __UserInput;

    // Start is called before the first frame update
    private void Start()
    {
        __MemoryMadness = FindObjectOfType<MemoryMadnessController>();
        __UserInput = FindObjectOfType<UserInput>();
        __SpriteRenderer = GetComponent<SpriteRenderer>();
        __Selectable = GetComponent<Selectable>();

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
                _CardFace = __MemoryMadness.__CardFaces[i];
                break;
            }
            i++;
        }
    }

    private void UpdateCardSprite()
    {
        __SpriteRenderer.sprite = __Selectable.faceUp ? _CardFace : __CardBack;
    }

    private void HighlightSelectedCard()
    {
        if (__UserInput.__SelectedHandCard)
        {
            __SpriteRenderer.color = (name == __UserInput.__SelectedHandCard.name) ? Color.yellow : Color.white;
        }
    }
}