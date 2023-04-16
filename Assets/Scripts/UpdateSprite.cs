using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateSprite : MonoBehaviour
{
    [SerializeField] private Sprite cardFace;
    [SerializeField] private Sprite cardBack;

    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private MemoryMadnessController memMad;
    private UserInput userInput;

    // Start is called before the first frame update
    private void Start()
    {
        SetCardFace();
        InitializeComponents();
        AddClickListener();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCardSprite();
        UpdateCardColor();
    }

    private void SetCardFace()
    {
        List<string> deck = MemoryMadnessController.GenerateDeck();
        memMad = FindObjectOfType<MemoryMadnessController>();

        int i = 0;
        foreach (string card in deck)
        {
            if (name == card)
            {
                cardFace = memMad.cardFaces[i];
                break;
            }
            i++;
        }
    }

    private void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
        userInput = FindObjectOfType<UserInput>();
    }

    private void AddClickListener()
    {
        // Add event listener for mouse click
        GetComponent<BoxCollider2D>().gameObject.AddComponent<EventTrigger>();
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => { OnCardClick(); });
        trigger.triggers.Add(entry);
    }

    private void UpdateCardSprite()
    {
        spriteRenderer.sprite = selectable.faceUp ? cardFace : cardBack;
    }

    private void UpdateCardColor()
    {
        //coalescing way to do the old if else statement in one line.
        spriteRenderer.color = userInput.selectedHandCard != null && userInput.selectedHandCard.name == name
            ? Color.yellow
            : Color.white;
    }

    private void OnCardClick()
    {
        if (userInput.selectedHandCard == null)
        {
            userInput.selectedHandCard = gameObject;
        }
        else
        {
            //coalescing way to do the old if else statement in one line.
            //if this condition is true ? then return null : otherwise return gameObject
            userInput.selectedHandCard = userInput.selectedHandCard.name == name ? null : gameObject;
        }
    }
}
