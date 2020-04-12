using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BattleManager : MonoBehaviour
{
    public GameObject DOORS;

    public Player_IN_Room[] S1_ROOM;//第一关房间列表
    public Player_IN_Room[] S2_ROOM;
    public Player_IN_Room[] S3_ROOM;

    Actor Player;
    Stage PP;

    private int checknum_a;
    private int checknum_b;
    private int sum;

    public int Current_State;//**DISON.ver**当前处于第几关卡
    private bool TP;

    public bool Normal_Room_Battle;//普通怪物房战斗

    public bool Protect_Room_Battle;//守护据点战斗

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
    private Flowchart flowchart;
    
    // Start is called before the first frame update
    void Start()
    {
        Player= GameObject.Find("Actor").GetComponent<Actor>();
        flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();
        Current_State = 1;//**DISON.ver**玩家当前处在第几关卡

        Dead_Fight_Timer = 0f;
        Dead_Fight_MaxTime = 10f;

       checknum_a = 1;
        checknum_b = 1;

        int[] num = new int[5];
        int[] num_2 = new int[5];

        Player_Choose = new int[3];

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

        for (int j = 0; j < num.Length; j++)//最后一关随机房间逻辑
        {
            if (checknum_b >= 3)
            {
                checknum_b = 3;
            }

            num_2[j] = Random.Range(1, checknum_b);

            checknum_b += 1;

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

        for (int i = 0; i < S1_ROOM.Length; i++)
        {
            S1_ROOM[i].SET_ROOM_TYPE(num[i]);//随机定义房间的类型
            //S2_ROOM[i].SET_ROOM_TYPE(num[i]);
            //S3_ROOM[i].SET_ROOM_TYPE(num_2[i]);
        }

        
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
    }

    // Update is called once per frame
    void Update()
    {
    
        if (MON_NUMS == 0&& IsLastWave==false)
        {
            startspawn = true;
            finishspawn = false;
        }

        if(MON_NUMS == 0 && IsLastWave == true && finishspawn == true&&BattleFinish==false)
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

        if (TP)//传送到下一关卡，当前关卡数+1
        {
            Current_State += 1;
            In_New_State = true;
            TP = false;
        }

        if (Dead_Room_Battle&&!flowchart.GetBooleanVariable("IS_TALKING"))//死斗房计时
        {

            if (Dead_Fight_Timer <= Dead_Fight_MaxTime)
            {
                Dead_Fight_Timer += Time.deltaTime;


            }
            else//死斗房时间结束，自动销毁所有怪物
            {
                IS_LAST_WAVE();
                FINISH_SPAWN();
                Dead_Room_Battle = false;
                MON_NUMS = 0;

            }
        }
        

    }

    public void State_Up()
    {

        TP = true;

        Debug.Log("UP!");

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

    public void Special_Battle_Start()//特殊房战斗
    {
        

        if (Current_State == 1)//守护据点
        {
            PP = GameObject.Find("ProtectPoint").GetComponent<Stage>();

            if (PP.StartCrack)
            {
                Protect_Room_Battle_Start();
                START_SPAWN();
            }
            
        }

        if (Current_State == 2)//死斗
        {
            Dead_Room_Battle_Start();
            START_SPAWN();
        }

        if (Current_State == 3)//BOSS战
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
