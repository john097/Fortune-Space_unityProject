using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power_Bf_Text : MonoBehaviour
{
    private Actor actor;
    public Text text;
    private string name;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Actor"))
        {
            actor = GameObject.Find("Actor").GetComponent<Actor>();
        }
        name = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        switch (name)
        {
            case "ATK_Text":

                break;
            case "HPR_Text":

                break;
            case "MHP_Text":

                break;
            case "SPD_Text":

                break;
        }
    }
}
