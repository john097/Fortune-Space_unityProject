using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Cinemachine.Utility;
using Cinemachine;

public class BattleManager : MonoBehaviour
{
    public Player_IN_Room S0_ROOM;
    public Player_IN_Room[] S1_ROOM;//第一关房间列表
    public Player_IN_Room[] S2_ROOM;
    public Player_IN_Room[] S3_ROOM;
    public Player_IN_Room BOSS_ROOM;

    Actor Player;
    Stage PP;
    Image black;
    Credit player_combo_kill;

    private bool Image_found = false;
    private int checknum_a;
    private int sum;

    
    private bool TP;

    public bool Normal_Room_Battle;//普通怪物房战斗

    public bool Protect_Room_Battle;//守护据点战斗
    public bool PP_Dead;//据点是否被破坏
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

    public bool In_New_State=false;//进入到新的关卡
    public bool isTalking;
    private Flowchart flowchart;

    public int currentstate;

    private Transform player_tf;
    private Transform BornRoom_tf;

    private Dialog_Manager dialog_manager;
    public bool tutorial_talking;

    public CinemachineVirtualCamera camera_follow;
    public GameObject camera;

    public string scene_name;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Fade_IN_OUT_Image"))//淡入淡出
        {
            black = GameObject.Find("Fade_IN_OUT_Image").GetComponent<Image>();
            black.DOFade(0, 2);
            Image_found = true;
        }

        Player = GameObject.Find("Actor").GetComponent<Actor>();
        player_combo_kill= GameObject.Find("Actor").GetComponent<Credit>();

        player_tf = GameObject.Find("Actor").transform;

        if (gameObject.scene.name != "SpawnRoom")
        {
            camera_follow = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

            camera = GameObject.Find("Main Camera");

            camera_follow.m_Follow = player_tf;
            Player.FollowCamera = camera;
        }
        

         flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();
        dialog_manager = GetComponent<Dialog_Manager>();
        tutorial_talking = false;

        scene_name = SceneManager.GetActiveScene().name;

      



        switch (PlayerPrefs.GetInt("Current_State"))//关卡开始传送到出生房
        {
         

            case 0:
                Player.TakeDamege(-(Player.maxHeal * 0.5f));//新关卡回血
                BornRoom_tf = GameObject.Find("Tutorial-Born_Zoom").transform;
                player_tf.transform.position = new Vector3(BornRoom_tf.position.x, BornRoom_tf.position.y, BornRoom_tf.position.z);
                break;
            case 1:
                Player.TakeDamege(-(Player.maxHeal * 0.5f));
                BornRoom_tf = GameObject.Find("S1-Born_Zoom").transform;
                player_tf.transform.position = new Vector3(BornRoom_tf.position.x, BornRoom_tf.position.y, BornRoom_tf.position.z);
                break;
            case 2:
                Player.TakeDamege(-(Player.maxHeal * 0.5f));
                BornRoom_tf = GameObject.Find("S2-Born_Zoom").transform;
                player_tf.transform.position = new Vector3(BornRoom_tf.position.x, BornRoom_tf.position.y, BornRoom_tf.position.z);
                break;
            case 3:
                Player.TakeDamege(-(Player.maxHeal * 0.5f));
                BornRoom_tf = GameObject.Find("S3-Born_Zoom").transform;
                player_tf.transform.position = new Vector3(BornRoom_tf.position.x, BornRoom_tf.position.y, BornRoom_tf.position.z);
                break;
                
            case 4:
                Player.TakeDamege(-(Player.maxHeal * 0.5f));
                BornRoom_tf = GameObject.Find("BossRoom_BornZoom").transform;
                player_tf.transform.position = new Vector3(BornRoom_tf.position.x, BornRoom_tf.position.y, BornRoom_tf.position.z);
                break;
        }


        Crack_Progress = 0f;
        Dead_Fight_Timer = 0f;
        Dead_Fight_MaxTime = 60f;

       checknum_a = 2;
       

        int[] num = new int[5];
        int[] num_2 = new int[4];

        Player_Choose = new int[3];


        if (PlayerPrefs.GetInt("Current_State") == 3)//生成随机数组，用于定义房间类型
        {
            for (int j = 0; j < num_2.Length; j++)//第三关随机房间逻辑
            {
                if (checknum_a >= 4)
                {
                    checknum_a = 4;
                }

                num_2[j] = Random.Range(1, checknum_a);

                checknum_a += 1;

                if (j == 2 && sum < 3)
                {
                    
                    num_2[j ] = 2;
                    num_2[j + 1] = 3;
                    break;
                }
                
                if (j == 3 )
                {
                    if(sum < 6)
                    {
                        num_2[j] = 3;
                    }
                    else
                    {
                        num_2[j]= Random.Range(1, checknum_a-1);
                    }
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
                if (j == 3 && sum >= 5)
                {
                    if (num[2] == 3)//前三关就出现特殊关时，后面两关不再出现特殊关
                    {

                        if(num[1] == 1)
                        {
                            num[j] = 2;
                            num[j + 1] = Random.Range(1, 3);
                            break;
                        }
                        else
                        {
                            num[j] = 1;
                            num[j + 1] = Random.Range(1, 3);
                            break;
                        }
                        
  
                    }
                    else
                    {
                        int a = Random.Range(1, 3);
                        if (a == 1)
                        {
                            num[j] = 1;
                        }
                        if (a == 2)
                        {
                            num[j] = 3;
                        }
                    }
                   
                   
                }
                if (j == 4)
                {
                    if (num[3] == 3)
                    {
                        num[j] = Random.Range(1, 3); 
                    }
                    else
                    {
                        num[j] = 3; 
                    }
                }

                sum += num[j];
            }
        }

        switch (PlayerPrefs.GetInt("Current_State"))//定义房间类型
        {
            case 0:
                S0_ROOM.SET_ROOM_TYPE(3);
                break;

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
            case 4:
                BOSS_ROOM.SET_ROOM_TYPE(3);
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
        PP_Dead = false;
       IsLastWave = false;
        RoomClear = false;
        BattleFinish = false;

        TP = false;

        In_New_State = true;

    }

    // Update is called once per frame
    void Update()
    {
        isTalking = flowchart.GetBooleanVariable("IS_TALKING");
        
        currentstate = PlayerPrefs.GetInt("Current_State");

        

        if(PlayerPrefs.GetInt("Current_State") == 0)//斩杀、克制教程
        {
            if (flowchart.GetIntegerVariable("Tutorial_Process") == 2)
            {
                float A = GameObject.FindGameObjectWithTag("ENEMY").GetComponent<Actor>().heal;
                float B= GameObject.FindGameObjectWithTag("ENEMY").GetComponent<Actor>().maxHeal;

                if (A <= (B * 0.2f)&& !tutorial_talking)
                {
                    

                    if (!Player.steping)
                    {
                        dialog_manager.Tutorial_Process_Talk();
                        tutorial_talking = true;
                    }
                }
                
            }

            if (flowchart.GetIntegerVariable("Tutorial_Process") == 3)
            {
                if (IsLastWave&& !tutorial_talking)
                {

                    
                    if (!Player.steping)
                    {
                        dialog_manager.Tutorial_Process_Talk();
                        tutorial_talking = true;

                    }
                    
                }
            }

        }
        
        

        if (MON_NUMS == 0&& IsLastWave==false)//非最后一波怪时刷新下一波怪
        {

            if (PlayerPrefs.GetInt("Current_State") == 0&&!tutorial_talking)
            {
                
                if (!Player.steping)
                {
                    dialog_manager.Tutorial_Process_Talk();
                    tutorial_talking = true;
                }

            }
            startspawn = true;
            finishspawn = false;

        }

        if(MON_NUMS == 0 && IsLastWave == true && finishspawn == true&&BattleFinish==false)//最后一波怪时不再刷新下一波怪
        {
            tutorial_talking = false;
            if (PlayerPrefs.GetInt("Current_State") == 0 && !tutorial_talking)
            {

                

                if (!Player.steping)
                {
                    dialog_manager.Tutorial_Process_Talk();
                    tutorial_talking = true;
                }
                
            }

            RoomClear = true;
            BattleFinish = true;
            
            

        }

        if (BattleFinish)//打开房间门口
        {

            if (GameObject.FindGameObjectWithTag("ENEMY"))
            {
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("ENEMY");
                for (int i = 0; i < enemys.Length; i++)
                {
                    enemys[i].GetComponent<Actor>().GoDie();
                    Debug.Log("godie");
                }
            }
        }
       


        if (Protect_Room_Battle && !isTalking)//保护据点房计时
        {
            if (Crack_Progress >= 10f||PP_Dead)
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
                Dead_Room_Battle = false;
                IS_LAST_WAVE();
                FINISH_SPAWN();
                MON_NUMS = 0;
                
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("ENEMY");
                for(int i = 0; i < enemys.Length; i++)
                {
                    enemys[i].GetComponent<Actor>().GoDie();
                    Debug.Log("godie");
                }
                
               

            }
        }

        if (PlayerPrefs.GetInt("Current_State")==-2)
        {

            if (flowchart.GetIntegerVariable("Spawn_Room_Choose") == 2)
            {
                PlayerPrefs.SetInt("Spawn_To_Level_1", 1);

            }
            else
            {
                PlayerPrefs.SetInt("Spawn_To_Level_1", 2);
            }
        }

        if (flowchart.GetBooleanVariable("SceneChange"))//传送到下一关卡，当前关卡数+1
        {
            if (Image_found)
            {
                black.DOFade(1, 0.5f);
                StartCoroutine(State_Up_IE(1f));
            }
            else
            {
                State_Up();
            }

           

        }

        


    }

    IEnumerator State_Up_IE(float duration)
    {
        
        yield return new WaitForSeconds(duration);
        State_Up();
    }

    public void State_Up() //场景切换
    {
        flowchart.SetBooleanVariable("SceneChange", false);
        SceneManager.LoadScene("Loading_Scene");
    }

    public void Mon_Dead()
    {
        if (MON_NUMS > 0)
        {
            MON_NUMS -= 1;
            player_combo_kill.ResetKillstreaksNum();

            Debug.Log(MON_NUMS);
        }
    }

    public void Tutorial_Room_Start()
    {
        BattleFinish = false;
        IsLastWave = false;
        RoomClear = false;
        MAX_MON_NUMS = 1;
        Monster_Waves = 3;
    }

    public void Normal_BATTLE_START()//普通房战斗
    {
        BattleFinish = false;
        IsLastWave = false;
        RoomClear = false;
        switch (PlayerPrefs.GetInt("Current_State"))
        {
            case 1:
                MAX_MON_NUMS = Random.Range(4,6);
                Monster_Waves = Random.Range(1,2);
                break;
            case 2:
                MAX_MON_NUMS = Random.Range(5, 9);
                Monster_Waves = Random.Range(2, 4);
                break;
            case 3:
                MAX_MON_NUMS = Random.Range(5, 6);
                Monster_Waves = Random.Range(2, 3);
                break;
        }
        
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

        if (PlayerPrefs.GetInt("Current_State") == 4)//BOSS战
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
        tutorial_talking = false;
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
