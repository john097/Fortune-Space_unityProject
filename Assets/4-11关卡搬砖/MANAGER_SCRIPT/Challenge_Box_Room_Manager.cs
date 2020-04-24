using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Challenge_Box_Room_Manager : MonoBehaviour
{ 

    public GameObject[] Boxes;
    private Flowchart flowchart;
    public bool A=false;

    // Start is called before the first frame update
    void Start()
    {
  
  
        GameObject[] Boxes = new GameObject[3];

        if (GameObject.Find("Flowchart1"))
        {
            flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();
        }

        for (int i = 0; i < 3; i++)
        {
            Boxes[i] = gameObject.transform.GetChild(i).gameObject;
            Boxes[i].GetComponent<Challenge_Box_Room_Battle>().SetBoxType(i);
        }

        


    }

    // Update is called once per frame
    void Update()
    {
        if (flowchart.GetBooleanVariable("Box_Choosed")&&!A)
        {
            Choose_Finish();
            A = true;
        }
    }

    public void Choose_Finish()
    {
        for (int j = 0; j < 3; j++)
        {
            if (!Boxes[j].GetComponent<Challenge_Box_Room_Battle>().Choose)
            {
                Boxes[j].GetComponent<Challenge_Box_Room_Battle>().Fadeout();
            }
        }
    }
    
}
