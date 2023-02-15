using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{

    public GameObject slot1;

    // Start is called before the first frame update
    void Start()
    {
        slot1 = this.gameObject;
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
                    Card(hit.collider.gameObject);
                }

            }
        }
    }

    void Card(GameObject selected){
        print("Clicked on card");

        //if card clicked on, select it
        if (slot1 == this.gameObject){
            slot1 = selected;
        }
        else if (slot1 != selected){
            Destroy(selected);
        }

    }
}
