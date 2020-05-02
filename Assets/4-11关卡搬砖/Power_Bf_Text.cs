using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power_Bf_Text : MonoBehaviour
{
    private Actor actor;
    private Text A_TEXT;
    Text text;
    private string thisname;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Actor"))
        {
            actor = GameObject.Find("Actor").GetComponent<Actor>();
        }
        thisname = gameObject.name;
        A_TEXT = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (thisname)
        {
            case "ATK_Text":
                A_TEXT.text= "x"+(actor.attack/2);
                break;

            case "HPR_Text":
                A_TEXT.text = "x" + (actor.healRecover / 2);
                break;

            case "MHP_Text":
                A_TEXT.text = "x" + ((actor.maxHeal - 500) / 50);
                break;

            case "SPD_Text":
                A_TEXT.text = "x" +(int)(((actor.speed - 8) / 0.2f)+0.1);
                break;
        }
    }
}
