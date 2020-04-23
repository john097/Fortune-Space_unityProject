using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge_Box_Room_Manager : MonoBehaviour
{

    public GameObject[] Boxes;
 


    // Start is called before the first frame update
    void Start()
    {
  
  
        GameObject[] Boxes = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            Boxes[i] = gameObject.transform.GetChild(i).gameObject;
            Boxes[i].GetComponent<Challenge_Box_Room_Battle>().SetBoxType(i);
        }


       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
