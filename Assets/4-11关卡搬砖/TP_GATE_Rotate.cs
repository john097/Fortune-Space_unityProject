using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class TP_GATE_Rotate : MonoBehaviour
{
    private Transform Actor;
    private PlayerArrowScript Arrow;
 
    // Start is called before the first frame update
    void Start()
    {


        if (PlayerPrefs.GetInt("Current_State") != -2)
        {
            Actor = GameObject.Find("Actor").transform;
            Arrow = GameObject.Find("BattleManager").GetComponent<PlayerArrowScript>();
            transform.LookAt(Actor);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Arrow.StartArrow();
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
            
        //}

        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    Arrow.DestroyArrow();
        //}

        
    }
}
