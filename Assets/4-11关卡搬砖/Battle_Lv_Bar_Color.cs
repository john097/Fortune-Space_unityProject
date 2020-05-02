using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle_Lv_Bar_Color : MonoBehaviour
{
   
    string OBname;
    private BattleManager BM;
    public GameObject IN;
    // Start is called before the first frame update
    void Start()
    {
        
        OBname = gameObject.name;
       
        if (GameObject.Find("BattleManager"))
        {
            BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (OBname)
        {
            case "LV0":
                
                if (BM.Game_Level == 0)
                {
                    IN.SetActive(true);
                   
                }
                else
                {
                    IN.SetActive(false);
                    
                }
               
                break;
            case "LV1":
                if (BM.Game_Level == 1)
                {
                    IN.SetActive(true);
                }
                else
                {
                    IN.SetActive(false);
                    
                }
                break;
            case "LV2":
                if (BM.Game_Level == 2)
                {
                    IN.SetActive(true);
                }
                else
                {
                    IN.SetActive(false);
                    
                }

                break;
            case "LV3":
                if (BM.Game_Level == 3)
                {
                    IN.SetActive(true);
                }
                else
                {
                    IN.SetActive(false);
                    
                }
                break;
            case "LV4":
                if (BM.Game_Level == 4)
                {
                    IN.SetActive(true);
                }
                else
                {
                    IN.SetActive(false);
                   
                }
                break;

        }
    }

    
}
