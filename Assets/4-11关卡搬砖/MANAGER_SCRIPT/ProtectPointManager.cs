using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectPointManager : MonoBehaviour
{
    
    private Actor pp_actor;
    private Stage pp_stage;
    private GameObject Player;    
    private Start_Scene_Change pp_BattleManager;
    private Dialog_Manager pp_DialogManager;
    public bool StartCrack;
    private bool A=false;

    // Start is called before the first frame update
    void Start()
    {
        
        pp_actor = GetComponent<Actor>();
        pp_stage= GetComponent<Stage>();
        if (GameObject.Find("BattleManager"))
        {
            pp_BattleManager = GameObject.Find("BattleManager").GetComponent<Start_Scene_Change>();
            pp_DialogManager = GameObject.Find("BattleManager").GetComponent<Dialog_Manager>();
        }
        
        

       StartCrack = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (pp_DialogManager.start_crack)
        {
            StartCrack = true;
        }

        if (pp_stage.heal <= 0)
        {
            if (!A)
            {
                pp_BattleManager.ProtectFailed();
                A = true;
            }
            
        }
        

    }  

    private void OnTriggerStay(Collider other)
    {
        
           
        
    }




}
