using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectPointManager : MonoBehaviour
{
    public float Crack_Progress;
    private Actor pp_actor;
    private GameObject Player;    
    private BattleManager pp_BattleManager;
    public bool StartCrack;
    private bool a;
    

    // Start is called before the first frame update
    void Start()
    {
        Crack_Progress = 0;
        pp_actor = GetComponent<Actor>();
       
        pp_BattleManager= GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        Player = GameObject.Find("Actor");
        

       StartCrack = false;
        a = false;
    }

    // Update is called once per frame
    void Update()
    {
        


        if (pp_actor.isAlive&& StartCrack)
        {
            if (Crack_Progress >= 30f)
            {
                pp_BattleManager.IS_LAST_WAVE();
                pp_BattleManager.FINISH_SPAWN();
                pp_BattleManager.Protect_Room_Battle_Finish();
                StartCrack = false;

            }
            else
            {
                Crack_Progress += Time.deltaTime;   
                //Debug.Log(Crack_Progress);
            }
            
        }
        if (!pp_actor.isAlive)
        {
            pp_BattleManager.IS_LAST_WAVE();
            pp_BattleManager.FINISH_SPAWN();
            pp_BattleManager.Protect_Room_Battle_Finish();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
            StartCrack = true;
            pp_BattleManager.Special_Battle_Start();
            a = true;
            Debug.Log("startcrack!");
        
    }




}
