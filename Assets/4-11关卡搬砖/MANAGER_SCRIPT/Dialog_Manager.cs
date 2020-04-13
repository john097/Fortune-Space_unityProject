using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;


public class Dialog_Manager : MonoBehaviour
{
    private Flowchart flowchart;
    private BattleManager DM_MANAGER;
    private Actor actor;

    // Start is called before the first frame update
    void Start()
    {

        flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();        
        actor = GameObject.Find("Actor").GetComponent<Actor>();
        DM_MANAGER = GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {

        State_Start_Talks();

        Talking_Remove();


    }

    public void State_Start_Talks()
    {
        if (DM_MANAGER.In_New_State)//第一关开场对话
        {
            if (PlayerPrefs.GetInt("Current_State") == 0)
            {
                flowchart.SendFungusMessage("State_1_Start");
               
                DM_MANAGER.In_New_State = false;
            }

            if (PlayerPrefs.GetInt("Current_State") == 1)//第二关开场对话
            {

                flowchart.SendFungusMessage("State_2_Start");
               
                DM_MANAGER.In_New_State = false;
            }

            if (PlayerPrefs.GetInt("Current_State") == 2)//第三关开场对话
            {
                flowchart.SendFungusMessage("State_3_Start");
               
                DM_MANAGER.In_New_State = false;
            }



        }
    }

    public void ProtectPoint_Enter_Talk()//据点房入场对话
    {
        flowchart.SendFungusMessage("ProtectPoint_Enter");
    }

    public void ProtectPoint_Start_Talk()//据点房开战前对话
    {
        flowchart.SendFungusMessage("ProtectPoint_Start");
    }

    public void DeadPoint_Start_Talk()//死斗房开战前对话
    {
        flowchart.SendFungusMessage("DeadPoint_Start");
    }

    public void BossBattle_Start_Talk()//BOSS房开战前对话
    {
        flowchart.SendFungusMessage("BossBattle_Start");
    }

    public void TP_Talk()//传送对话
    {
        if (PlayerPrefs.GetInt("Current_State") == 0)
        {
            flowchart.SendFungusMessage("State_1_Talk_TP");
        }

        if (PlayerPrefs.GetInt("Current_State") == 1)
        {
            flowchart.SendFungusMessage("State_2_Talk_TP");
        }

        if (PlayerPrefs.GetInt("Current_State") == 2)
        {
            flowchart.SendFungusMessage("Goto_BossRoom_Talk");
        }
    }


    public void Talking_Remove()
    {
        if (flowchart.GetBooleanVariable("IS_TALKING"))
        {
            actor.isTalking = true;
        }
        else
        {
            actor.isTalking = false;
        }
    }

}
