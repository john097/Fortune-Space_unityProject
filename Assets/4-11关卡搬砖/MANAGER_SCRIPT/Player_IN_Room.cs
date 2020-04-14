using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_IN_Room : MonoBehaviour
{
    private Actor actor;
    private Dialog_Manager dialog_manager;
    BattleManager B_Manager;
   
    public GameObject DOORS;
    public GameObject ProtectPoint;

    public GameObject DeadPoint;

    GameObject player_collider;
    Actor Player;

    public bool PlayerInRoom;
    public bool inroom_started;
    private bool a;
    private bool b;
    public int this_room_type;//房间类型，=1时为怪物房间，=2时为道具/商店房间
    
    
  


   



    // Start is called before the first frame update
    void Start()
    {
        player_collider = GameObject.Find("Actor");
        Player = GameObject.Find("Actor").GetComponent<Actor>();
        PlayerInRoom = false;
        inroom_started = false;
        B_Manager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        a = false;
        b = true;
        dialog_manager = GameObject.Find("BattleManager").GetComponent<Dialog_Manager>();


    }

    // Update is called once per frame
    void Update()
    {

        if ((B_Manager.Protect_Room_Battle || B_Manager.Dead_Room_Battle) && !B_Manager.isTalking && !b)
        {
            var A = gameObject.transform.GetChild(2).gameObject;
            A.SetActive(true);

            b = true;
            
        }


        if (this_room_type == 3 && gameObject.tag == ("S1-ROOM") && !a)//守护据点关卡触发器
        {
            var A = gameObject.transform.GetChild(0).gameObject;
            A.SetActive(true);
           
            a = true;
        }

        if (this_room_type == 3 && gameObject.tag==("S2-ROOM") && !a)//死斗房间触发器
        {
            var A = gameObject.transform.GetChild(1).gameObject;
            A.SetActive(true);
            
            a = true;
        }

        if (this_room_type == 3 && gameObject.tag == ("S3-ROOM") && !a)
        {
           

            a = true;
        }



    }




    public void SET_ROOM_TYPE(int a)
    {
        this_room_type = a;
    }

    private void OnTriggerEnter(Collider other)
    {

        

        if (other.gameObject == player_collider && inroom_started == false && this_room_type == 1)//开始普通房战斗
        {
            var A = gameObject.transform.GetChild(2).gameObject;//生成围墙
            A.SetActive(true);

            PlayerInRoom = true;
            inroom_started = true;
            GetComponent<Enemy_Manager>().enabled = true;
            GetComponent<Player_IN_Room>().enabled = false;
            B_Manager.START_SPAWN();
            B_Manager.Normal_BATTLE_START();

        }

        if (other.gameObject == player_collider && inroom_started == false && this_room_type == 2)//商店、道具房
        {
            inroom_started = true;
            GetComponent<Enemy_Manager>().enabled = true;

            gameObject.GetComponent<Enemy_Manager>().StoreBorn();

            GetComponent<Player_IN_Room>().enabled = false;

        }


            if (other.gameObject == player_collider && inroom_started == false && this_room_type == 3)//特殊房战斗
        {
            

            PlayerInRoom = true;
            inroom_started = true;
            B_Manager.RoomClear = false;


            if(PlayerPrefs.GetInt("Current_State") == 0)
            {
                B_Manager.START_SPAWN();
                B_Manager.Tutorial_Room_Start();

                GetComponent<Enemy_Manager>().enabled = true;
                GetComponent<Player_IN_Room>().enabled = false;

            }


            if(PlayerPrefs.GetInt("Current_State") == 1)
            {
                dialog_manager.ProtectPoint_Enter_Talk();
                b = false;
            }

            if (PlayerPrefs.GetInt("Current_State") == 2)
            {
                
                b = false;
            }

            if (PlayerPrefs.GetInt("Current_State") == 3)
            {
                var A = gameObject.transform.GetChild(2).gameObject;//生成围墙
                A.SetActive(true);

                GetComponent<Enemy_Manager>().enabled = true;
                GetComponent<Player_IN_Room>().enabled = false;
                B_Manager.START_SPAWN();
                B_Manager.Normal_BATTLE_START();

                b = false;
            }


            if (PlayerPrefs.GetInt("Current_State") == 4)//Boss房战斗
            {
                dialog_manager.BossBattle_Start_Talk();
                B_Manager.Special_Battle_Start();
                b = false;
            }

            GetComponent<Enemy_Manager>().enabled = true;
            //GetComponent<Player_IN_Room>().enabled = false;
            
            //B_Manager.Special_Battle_Start();

        }

        //if (other.gameObject == player_collider && inroom_started == false && this_room_type == 3&&B_Manager.Current_State==2)//Boss房战斗
        //{
        //    PlayerInRoom = true;
        //    inroom_started = true;  
        //    B_Manager.RoomClear = false;
            

        //    GetComponent<Enemy_Manager>().enabled = true;
        //    GetComponent<Player_IN_Room>().enabled = false;
           

        //}
    }



}







