using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{

    public GameObject selectedHandCard;

    private PointSystem pointSys;

    // Start is called before the first frame update
    void Start()
    {
        selectedHandCard = null;

        pointSys = FindObjectOfType<PointSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick(){
        if (Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10 ));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit){
                //clicked on a card
                if (hit.collider.CompareTag("Card")){
                    string parentObjectName = hit.collider.gameObject.transform.parent.gameObject.name;
                    if(parentObjectName == "Hand0" || parentObjectName == "Hand1") {
                        ClickOnHandCard(hit.collider.gameObject);
                    } else {
                        ClickGameCard(hit.collider.gameObject);
                    }
                }

            }
        }
    }

    void ClickOnHandCard(GameObject selected){
        print("Clicked on hand card");

        //if card clicked on, select it
        if (selectedHandCard == null){
            selectedHandCard = selected;
        }

    }

    void ClickGameCard(GameObject selected){
        print("Clicked on game card");

        if (selectedHandCard != null){
            char selectedHandSuit = selectedHandCard.name[0];
            char selectedHandNumber = selectedHandCard.name[1];

            char selectedSuit = selected.name[0];
            char selectedNumber = selected.name[1];

            //if the number matches
                //score is 3
            //if suit matches
                //score is 2
            //if colour matches
                //score is 1
            if (selectedHandNumber == selectedNumber){
                pointSys.UpdateScore(3);
                Destroy(selected);
            }
            else if (selectedHandSuit == selectedSuit){
                pointSys.UpdateScore(2);
                Destroy(selected);
            } 
            else if ((selectedHandSuit == 'C' && selectedSuit == 'S') || (selectedHandSuit == 'S' && selectedSuit == 'C') || (selectedHandSuit == 'H' && selectedSuit == 'D') || (selectedHandSuit == 'D' && selectedSuit == 'H'))
            {
                pointSys.UpdateScore(1);
                Destroy(selected);
            }
            else {
                pointSys.UpdateScore(-1);
            }
        }

    }

}
