using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_IN_Room : MonoBehaviour
{
    private Actor actor;
    private Dialog_Manager dialog_manager;
    BattleManager B_Manager;
   
    public GameObject DOORS;
   private Stage ProtectPoint;

    public GameObject DeadPoint;

    GameObject player_collider;
    Actor Player;

    public bool PlayerInRoom;
    public bool inroom_started;
    private bool a;
    private bool b;
    private bool C=false;
    public int this_room_type;//房间类型，=1时为怪物房间，=2时为道具/商店房间

    public bool PP_Dead;//判断据点会否被破坏

    public float refresh_timer;
    public float refresh_time;
    public bool refresh = false;




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
        PP_Dead = true;
        dialog_manager = GameObject.Find("BattleManager").GetComponent<Dialog_Manager>();

        ProtectPoint = gameObject.transform.GetChild(0).gameObject.GetComponent<Stage>();
        refresh_time = 5f;
    }

    // Update is called once per frame
    void Update()
    {

        if(this_room_type == 3 && gameObject.tag == ("S1-ROOM") && !C)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            

            C = true;
        }

        

        if (!B_Manager.Protect_Room_Battle && B_Manager.BattleFinish&& this_room_type == 3)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            GetComponent<Player_IN_Room>().enabled = false;
            
        }

        if (PlayerPrefs.GetInt("Current_State") != 0)
        {
            if (this_room_type == 1 && B_Manager.BattleFinish)
            {

                refresh_timer += Time.deltaTime;
                if (refresh_timer >= refresh_time)
                {
                    inroom_started = false;
                    GetComponent<Enemy_Manager>().Monster_CurrentWaves = 0;
                    refresh_timer = 0;
                }
            }
        }
        

    }

    public void wallopen()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }


    public void SET_ROOM_TYPE(int a)
    {
        this_room_type = a;
    }

    private void OnTriggerEnter(Collider other)
    {

        

        if (other.gameObject == player_collider && inroom_started == false && this_room_type == 1)//开始普通房战斗
        {

            PlayerInRoom = true;
            inroom_started = true;
            GetComponent<Enemy_Manager>().enabled = true;
            //GetComponent<Player_IN_Room>().enabled = false;
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


            if (PlayerPrefs.GetInt("Current_State") == 0)
            {
                B_Manager.START_SPAWN();
                B_Manager.Tutorial_Room_Start();

                GetComponent<Enemy_Manager>().enabled = true;
                GetComponent<Player_IN_Room>().enabled = false;

            }


            if(PlayerPrefs.GetInt("Current_State") == 1)
            {
                GameObject[] mons;
                mons = GameObject.FindGameObjectsWithTag("ENEMY");
                for(int i = 0; i < mons.Length; i++)
                {
                    Destroy(mons[i]);
                }
                wallopen();
                dialog_manager.ProtectPoint_Enter_Talk();
                
            }

            if (PlayerPrefs.GetInt("Current_State") == 2)
            {
                GameObject[] mons;
                mons = GameObject.FindGameObjectsWithTag("ENEMY");
                for (int i = 0; i < mons.Length; i++)
                {
                    Destroy(mons[i]);
                }
                wallopen();
                dialog_manager.ProtectPoint_Enter_Talk();
                b = false;
            }

            if (PlayerPrefs.GetInt("Current_State") == 3)
            {
                GameObject[] mons;
                mons = GameObject.FindGameObjectsWithTag("ENEMY");
                for (int i = 0; i < mons.Length; i++)
                {
                    Destroy(mons[i]);
                }
                wallopen();

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

        }

    }



}







