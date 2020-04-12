using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public GameObject DOORS;

    public Player_IN_Room[] S1_ROOM;//第一关房间列表
    public Player_IN_Room[] S2_ROOM;
    public Player_IN_Room[] S3_ROOM;

    Actor Player;
    Stage PP;

    private int checknum_a;
    private int sum;

    
    private bool TP;

    public bool Normal_Room_Battle;//普通怪物房战斗

    public bool Protect_Room_Battle;//守护据点战斗
    public float PP_Heal;//据点生命值
    public float Crack_Progress;//破译进度


    public bool Dead_Room_Battle;//死斗房战斗
    public float Dead_Fight_Timer;//死斗房计时器
    public float Dead_Fight_MaxTime;//死斗房最大时长

    public bool BOSS_Battle;//BOSS房战斗

    public int Monster_Waves;//怪物总波数
    public int MAX_MON_NUMS;//每波最大怪物数量
    public int MON_NUMS;//当前剩余怪物数量


    public int[] Player_Choose;//玩家关卡选项 

    public bool startspawn;//开始刷怪
    public bool finishspawn;//完成刷怪
    public bool IsLastWave;//是否最后一波怪
    public bool RoomClear;//
    public bool BattleFinish;

    public bool In_New_State;//进入到新的关卡
    public bool isTalking;
    private Flowchart flowchart;

    public int currentstate;

    private Transform player_tf;
    private Transform BornRoom_tf;
    public void Awake()
    {
        PlayerPrefs.DeleteKey("Current_State");
        if (!PlayerPrefs.HasKey("Current_State"))
        {
            PlayerPrefs.SetInt("Current_State", 1);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
        Player= GameObject.Find("Actor").GetComponent<Actor>();
        player_tf = GameObject.Find("Actor").transform;
        flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();

        switch(PlayerPrefs.GetInt("Current_State"))//关卡开始传送到出生房
        {
            case 1:
                BornRoom_tf = GameObject.Find("S1-Born_Zoom").transform;
                player_tf.transform.position = new Vector3(BornRoom_tf.position.x, player_tf.position.y, BornRoom_tf.position.z);
                break;
            case 2:
                BornRoom_tf = GameObject.Find("S2-Born_Zoom").transform;
                player_tf.transform.position = new Vector3(BornRoom_tf.position.x, player_tf.position.y, BornRoom_tf.position.z);
                break;
            case 3:
                BornRoom_tf = GameObject.Find("S3-Born_Zoom").transform;
                player_tf.transform.position = new Vector3(BornRoom_tf.position.x, player_tf.position.y, BornRoom_tf.position.z);
                break;
        }
        

        Crack_Progress = 0f;
        Dead_Fight_Timer = 0f;
        Dead_Fight_MaxTime = 10f;

       checknum_a = 1;
       

        int[] num = new int[5];
        int[] num_2 = new int[5];

        Player_Choose = new int[3];


        if (PlayerPrefs.GetInt("Current_State") == 3)//生成随机数组，用于定义房间类型
        {
            for (int j = 0; j < num.Length; j++)//最后一关
            {
                if (checknum_a >= 3)
                {
                    checknum_a = 3;
                }

                num_2[j] = Random.Range(1, checknum_a);

                checknum_a += 1;

                if (j == 3 && sum < 4)
                {
                    num_2[j] = 2;
                    num_2[j + 1] = 3;
                    break;
                }
                if (j == 4)
                {
                    num_2[j] = 3;

                    break;
                }

                sum += num_2[j];
            }
  
        }
        else
        {
            for (int j = 0; j < num.Length; j++)//前2关随机房间逻辑
            {
                if (checknum_a >= 4)
                {
                    checknum_a = 4;
                }

                num[j] = Random.Range(1, checknum_a);

                checknum_a += 1;

                if (j == 3 && sum < 4)
                {
                    num[j] = 2;
                    num[j + 1] = 3;
                    break;
                }
                if (j == 3 && sum >= 4)
                {
                    num[j] = 3;
                    num[j + 1] = Random.Range(1, 3);
                    break;
                }

                sum += num[j];
            }
        }

        switch (PlayerPrefs.GetInt("Current_State"))//定义房间类型
        {
            case 1:
                for (int i = 0; i < S1_ROOM.Length; i++)
                {
                    S1_ROOM[i].SET_ROOM_TYPE(num[i]);
                }
                break;
            case 2:
                for (int i = 0; i < S2_ROOM.Length; i++)
                {
                    S2_ROOM[i].SET_ROOM_TYPE(num[i]);
                }
                break;
            case 3:
                for (int i = 0; i < S3_ROOM.Length; i++)
                {
                    S3_ROOM[i].SET_ROOM_TYPE(num_2[i]);
                }
                break;

        }


        //Debug.Log("GO");



        MON_NUMS = MAX_MON_NUMS;
        Monster_Waves = 0;

        Normal_Room_Battle = false;
        Protect_Room_Battle = false;
        Dead_Room_Battle = false;
        BOSS_Battle = false;

        startspawn = false;
        finishspawn = false;

        IsLastWave = false;
        RoomClear = false;
        BattleFinish = false;

        TP = false;
        In_New_State = true;
        isTalking = false;

    }

    // Update is called once per frame
    void Update()
    {

        currentstate = PlayerPrefs.GetInt("Current_State");

        if (MON_NUMS == 0&& IsLastWave==false)//非最后一波怪时刷新下一波怪
        {
            startspawn = true;
            finishspawn = false;
        }

        if(MON_NUMS == 0 && IsLastWave == true && finishspawn == true&&BattleFinish==false)//最后一波怪时不再刷新下一波怪
        {
            RoomClear = true;
            BattleFinish = true;

        }

        if (BattleFinish)//打开房间门口
        {
            //DOORS.SetActive(true);
        }
        else
        {
            //DOORS.SetActive(false);
        }


        if (Protect_Room_Battle && !isTalking)
        {
            if (Crack_Progress >= 30f)
            {
                IS_LAST_WAVE();
                FINISH_SPAWN();
                Protect_Room_Battle_Finish();
            }
            else
            {
                Crack_Progress += Time.deltaTime;
            }
        }


        if (Dead_Room_Battle&&!isTalking)//死斗房计时
        {

            if (Dead_Fight_Timer <= Dead_Fight_MaxTime)
            {
                Dead_Fight_Timer += Time.deltaTime;


            }
            else//死斗房时间结束，自动销毁所有怪物
            {
                IS_LAST_WAVE();
                FINISH_SPAWN();
                MON_NUMS = 0;
                Dead_Room_Battle = false;
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("ENEMY");
                for(int i = 0; i < enemys.Length; i++)
                {
                    enemys[i].GetComponent<Actor>().GoDie();
                }
                
               

            }
        }
        if (flowchart.GetBooleanVariable("SceneChange"))//传送到下一关卡，当前关卡数+1
        {
            State_Up();
        }
            



    }

    public void State_Up() //场景切换
    {

        switch (PlayerPrefs.GetInt("Current_State"))
        {
            case 1:
                PlayerPrefs.SetInt("Current_State", 2);
                
                flowchart.SetBooleanVariable("SceneChange", false);

                SceneManager.LoadScene("Level 2  Scene 1");
                break;

            case 2:
                PlayerPrefs.SetInt("Current_State", 3);
                
                flowchart.SetBooleanVariable("SceneChange", false);
                break;

        }


    }

    public void Mon_Dead()
    {
        if (MON_NUMS > 0)
        {
            MON_NUMS -= 1;
            Debug.Log(MON_NUMS);
        }
    }



    public void Normal_BATTLE_START()//普通房战斗
    {
        BattleFinish = false;
        IsLastWave = false;
        RoomClear = false;
        MAX_MON_NUMS = 5;
        Monster_Waves = 1;
    }

    public void Special_Battle_Start()//特殊房战斗类型判断
    {
        

        if (PlayerPrefs.GetInt("Current_State") == 1)//守护据点
        {
            
            
                Protect_Room_Battle_Start();
                START_SPAWN();
            
            
        }

        if (PlayerPrefs.GetInt("Current_State") == 2)//死斗
        {
            Dead_Room_Battle_Start();
            START_SPAWN();
        }

        if (PlayerPrefs.GetInt("Current_State") == 3)//BOSS战
        {
            Boss_Battle_Start();
            START_SPAWN();
        }
    }

    public void Protect_Room_Battle_Start()//守护据点房战斗
    {
        BattleFinish = false;
        IsLastWave = false;
        RoomClear = false;

        Protect_Room_Battle = true;

        Monster_Waves = 100;

    }
    public void Protect_Room_Battle_Finish()//守护据点房战斗结束
    {
        Protect_Room_Battle = false;
    }

    public void Dead_Room_Battle_Start()//死斗房战斗
    {
        BattleFinish = false;
        IsLastWave = false;
        RoomClear = false;

        Dead_Room_Battle = true;

        MAX_MON_NUMS = 100;
        Debug.Log("dead fight!");
    }

    public void Boss_Battle_Start()//BOSS房战斗
    {
        BattleFinish = false;
        RoomClear = false;
        IsLastWave = false;

        BOSS_Battle = true;

        MAX_MON_NUMS = 1;
        Monster_Waves = 1;
    }

    public void START_WAVE()
    {
        MON_NUMS = MAX_MON_NUMS;
        startspawn = false;
    }

    public void START_SPAWN()
    {
        startspawn = true;
    }

    public void FINISH_SPAWN()
    {
        finishspawn = true;
    }

    public void IS_LAST_WAVE()
    {
        IsLastWave = true;
    }


}
