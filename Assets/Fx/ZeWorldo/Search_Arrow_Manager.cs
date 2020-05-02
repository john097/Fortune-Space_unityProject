using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search_Arrow_Manager : MonoBehaviour
{
    public int target_type;
    public bool search_done;
    private PlayerArrowScript arrow_ctr;
    // Start is called before the first frame update
    void Start()
    {
        search_done = false;
        if (GameObject.Find("BattleManager"))
        {
            arrow_ctr = GameObject.Find("BattleManager").GetComponent<PlayerArrowScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!search_done)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Tool"))
            {
                switch (other.gameObject.tag)
                {
                    case "TP_GATE":
                        
                        break;
                    case "Blood_Box":

                        break;
                    case "Gold_Box":

                        break;
                    case "Miracle_Box":

                        break;
                    case "Power_Box":

                        break;
                }
            }
        }
        
    }

}
