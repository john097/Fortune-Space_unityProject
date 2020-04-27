using System.Collections;
using UnityEngine;
using Fungus;

public class Enemy_Manager : MonoBehaviour
{
    private Actor actor;
    private Player_IN_Room ME;

    public GameObject[] This_Room_Enemys;// 怪物数组
    public GameObject[] This_Room_Boss;//Boss数组

    public float Spawn_Cd;//每只怪物刷新的间隔时间

    [Tooltip("怪物总波数")]
    
    public int Monster_CurrentWaves;//当前怪物波数
    [Tooltip("每波次怪物数量")]


    public bool StopSpawn;

    BattleManager B_Manager;

    Transform player_transform;
    Actor Player;

    private Flowchart flowchart;


    private const string Prefabs = "Prefabs/";
    private int tutorial_mon_num = 0;

    public int mons_1;
    public int mons_2;
    public int mons_3;
    public int mons_4;

    // Start is called before the first frame update
    void Start()
    {
        player_transform = GameObject.Find("Actor").transform;
        Player = GameObject.Find("Actor").GetComponent<Actor>();

        ME = GetComponent<Player_IN_Room>();
        flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();


        Spawn_Cd = 1f;
        
       
        Monster_CurrentWaves = 0;
    
       
        StopSpawn = false;

       

        B_Manager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
    }







    Bound getBound(Transform tf)
    {
       
            Vector3 player_center = player_transform.position;
            Vector3 extents = tf.GetComponent<BoxCollider>().bounds.extents;
            Vector3 dL = new Vector3(player_center.x - extents.x, player_center.y, player_center.z - extents.z);
            Vector3 dR = new Vector3(player_center.x + extents.x, player_center.y, player_center.z - extents.z);
            Vector3 sR = new Vector3(player_center.x + extents.x, player_center.y, player_center.z + extents.z);
            Vector3 sL = new Vector3(player_center.x - extents.x, player_center.y, player_center.z + extents.z);
            Bound bound = new Bound(dL, dR, sR, sL, player_center, player_center.y);

        return bound;
    }

    Bound getBound2(Transform tf)
    {

        Vector3 center = tf.GetComponent<BoxCollider>().bounds.center;
        Vector3 extents = tf.GetComponent<BoxCollider>().bounds.extents;
        Vector3 dL = new Vector3(center.x - extents.x, center.y, center.z - extents.z);
        Vector3 dR = new Vector3(center.x + extents.x, center.y, center.z - extents.z);
        Vector3 sR = new Vector3(center.x + extents.x, center.y, center.z + extents.z);
        Vector3 sL = new Vector3(center.x - extents.x, center.y, center.z + extents.z);
        Bound bound = new Bound(dL, dR, sR, sL, center, center.y);

        return bound;
    }



    // Update is called once per frame
    private void Update()
    {
        if(PlayerPrefs.GetInt("Current_State") == 0)
        {
            if (B_Manager.startspawn == true && Monster_CurrentWaves < B_Manager.Monster_Waves && ME.this_room_type == 3 && !flowchart.GetBooleanVariable("IS_TALKING"))//教程房间
            {
                if (Monster_CurrentWaves == 2)
                {
                    B_Manager.MAX_MON_NUMS = 2;
                }
                tutorial_mon_num = 0;
                B_Manager.START_WAVE();
                StartCoroutine(TutorialRoomCreateEnemies(Spawn_Cd));
               
                Monster_CurrentWaves += 1;
            }
        }
       

        if (!flowchart.GetBooleanVariable("IS_TALKING"))
        {

            if (B_Manager.startspawn == true && Monster_CurrentWaves < B_Manager.Monster_Waves && ME.this_room_type == 1)//开始普通房刷怪模式
            {


                B_Manager.START_WAVE();
                StartCoroutine(NormalRoomCreateEnemies(Spawn_Cd));
                Monster_CurrentWaves += 1;

            }

            if (B_Manager.startspawn == true && Monster_CurrentWaves < B_Manager.Monster_Waves && ME.this_room_type == 3&& PlayerPrefs.GetInt("Current_State") == 3)//开始普通房刷怪模式
            {


                B_Manager.START_WAVE();
                StartCoroutine(NormalRoomCreateEnemies(Spawn_Cd));
                Monster_CurrentWaves += 1;

            }

           

                if (B_Manager.startspawn == true && Monster_CurrentWaves < B_Manager.Monster_Waves && ME.this_room_type == 3 && PlayerPrefs.GetInt("Current_State") == 1)//开始守护据点房刷怪模式（需要调整逻辑
            {

                B_Manager.START_WAVE();
                StartCoroutine(ProtectRoomCreateEnemies(Spawn_Cd));

            }

            if (B_Manager.startspawn == true && ME.this_room_type == 3 && B_Manager.Dead_Room_Battle && PlayerPrefs.GetInt("Current_State") == 2)//开始死斗房刷怪模式（需要调整逻辑
            {

                B_Manager.START_WAVE();
                StartCoroutine(DeadRoomCreateEnemies(Spawn_Cd*2));

            }


           

            if (B_Manager.startspawn == true && ME.this_room_type == 3 && B_Manager.BOSS_Battle && PlayerPrefs.GetInt("Current_State") == 4)//开始BOSS房刷怪模式（需要调整逻辑
            {
                B_Manager.START_WAVE();
                B_Manager.IS_LAST_WAVE();
                StartCoroutine(BossRoomCreateEnemies(Spawn_Cd));

                Debug.Log("go");


            }


            if (B_Manager.RoomClear)//完成关底，生成传送门
            {
                if (PlayerPrefs.GetInt("Current_State") != 0)
                {
                    if (ME.this_room_type == 3)
                    {
                        int x = Random.Range(0, 10);
                        int z = Random.Range(0, 10);
                        GameObject tp_gate;
                        tp_gate = Resources.Load(Prefabs + "TP_GATE") as GameObject;

                        Vector3 pos = new Vector3(player_transform.position.x + x, player_transform.position.y, player_transform.position.z + z);
                        Instantiate(tp_gate, pos, Quaternion.identity);


                    }

                    GetComponent<Enemy_Manager>().enabled = false;
                }
            }
            if (Monster_CurrentWaves == B_Manager.Monster_Waves)
            {

                B_Manager.IS_LAST_WAVE();
            }


        }



    }


    public void StoreBorn()//生成商店
    {
        Bound bound = getBound2(gameObject.transform);
        Vector3 pos = new Vector3(bound.center.x, bound.y+2, bound.center.z);
        GameObject Store;

        Store = Resources.Load(Prefabs + "StorePrefab") as GameObject;

        Instantiate(Store, pos, Quaternion.identity);
    }

    public void ToolBorn()//生成道具
    {
        Bound bound = getBound2(gameObject.transform);
        Vector3 pos = new Vector3(bound.center.x, bound.y, bound.center.z);
        GameObject Tool;

        Tool= Resources.Load(Prefabs + "ToolPrefab") as GameObject;

        Instantiate(Tool, pos, Quaternion.identity);
    }



    IEnumerator NormalRoomCreateEnemies(float duration)//普通房间刷新怪物
    {
        int count = 0;
        int This_Room_Enemys_Index = 0;
        mons_1 = 0;
        mons_2 = 0;
        mons_3 = 0;
        mons_4 = 0;

        while (true)
        {
            if (count < B_Manager.MAX_MON_NUMS)
            {
                yield return new WaitForSeconds(duration);

                if (PlayerPrefs.GetInt("Current_State") != 1)//第一关只会刷新rank1级别的怪物
                {
                    This_Room_Enemys_Index = Random.Range(0, This_Room_Enemys.Length);

                }
                else
                {
                    This_Room_Enemys_Index = Random.Range(0, (This_Room_Enemys.Length - 2));
                }

                if (mons_4 >= 2)//Rank2级别的怪物每波每种最多刷两只；
                {
                    This_Room_Enemys_Index = Random.Range(0, (This_Room_Enemys.Length - 1));

                    
                }

                if (mons_3 >= 2)
                {
                    This_Room_Enemys_Index = Random.Range(0, (This_Room_Enemys.Length - 2));
                }

                switch (This_Room_Enemys_Index)
                {
                    case 0:
                        mons_1 += 1;
                        break;
                    case 1:
                        mons_2 += 1;
                        break;
                    case 2:
                        mons_3 += 1;
                        break;
                    case 3:
                        mons_4 += 1;
                        break;
                }





                Bound bound = getBound(gameObject.transform);
                Vector3 pos = new Vector3(bound.getRandomX(), bound.y, bound.getRandomZ());
                // 开始刷新怪物
                Instantiate(This_Room_Enemys[This_Room_Enemys_Index], pos, Quaternion.identity);
                count++;

            }
            else
            {
                B_Manager.FINISH_SPAWN();
                break;
            }
        }
        
    }

    IEnumerator TutorialRoomCreateEnemies(float duration)//新手教程房间刷新怪物
    {
        int count = 0;

        while (true)
        {
            if (count < B_Manager.MAX_MON_NUMS)
            {
                yield return new WaitForSeconds(duration);

                Transform BornRoom_tf;
                Transform BornRoom_tf2;
                BornRoom_tf = GameObject.Find("Enemy_BornZoom").transform;
                BornRoom_tf2 = GameObject.Find("Enemy_BornZoom2").transform;
                Vector3[] pos=new Vector3[2];

                pos[0]= new Vector3(BornRoom_tf.position.x, BornRoom_tf.position.y, BornRoom_tf.position.z);
                pos[1] = new Vector3(BornRoom_tf2.position.x, BornRoom_tf2.position.y, BornRoom_tf2.position.z);

                // 开始刷新怪物
                Instantiate(This_Room_Enemys[tutorial_mon_num], pos[tutorial_mon_num], Quaternion.Euler(new Vector3(0,-90f,0)));
                count++;
                tutorial_mon_num++;
            }
            else
            {
                B_Manager.FINISH_SPAWN();
                break;
            }
        }

    }


    IEnumerator ProtectRoomCreateEnemies(float duration)//守护据点房间刷新怪物
    {
        int count = 0;
        mons_1 = 0;
        mons_2 = 0;
        mons_3 = 0;
        mons_4 = 0;
           
        while (true)
        {
            if (count < B_Manager.MAX_MON_NUMS)
            {
                yield return new WaitForSeconds(duration);
                
                int This_Room_Enemys_Index = Random.Range(0, This_Room_Enemys.Length);
                Bound bound = getBound(gameObject.transform);

                switch (This_Room_Enemys_Index)
                {
                    case 0:
                        mons_1 += 1;
                        break;
                    case 1:
                        mons_2 += 1;
                        break;
                    case 2:
                        mons_3 += 1;
                        break;
                    case 3:
                        mons_4 += 1;
                        break;
                }

                if(count== B_Manager.MAX_MON_NUMS - 1)//守护据点房中每波怪至少有一只自爆虫
                {
                    if (mons_4 < 1)
                    {
                        This_Room_Enemys_Index = 3;
                    }
                    
                }
                Vector3 pos = new Vector3(bound.getRandomX(), bound.y + 0.3f, bound.getRandomZ());
                // 开始刷新怪物
                Instantiate(This_Room_Enemys[This_Room_Enemys_Index], pos, Quaternion.identity);
                count++;
            }
            else
            {
                B_Manager.FINISH_SPAWN();
                break;
            }
        }

    }

    IEnumerator DeadRoomCreateEnemies(float duration)//死斗房间刷新怪物
    {
        int count = 0;

        while (true)
        {
            if (count < B_Manager.MAX_MON_NUMS && B_Manager.Dead_Room_Battle)
            {
                yield return new WaitForSeconds(duration);
                
                int This_Room_Enemys_Index = Random.Range(0, This_Room_Enemys.Length);
                Bound bound = getBound(gameObject.transform);
                Vector3 pos = new Vector3(bound.getRandomX(), bound.y + 0.3f, bound.getRandomZ());
                // 开始刷新怪物
                Instantiate(This_Room_Enemys[This_Room_Enemys_Index], pos, Quaternion.identity);
                count++;
            }
            else
            {
                B_Manager.FINISH_SPAWN();
                break;
            }
        }

    }

    IEnumerator BossRoomCreateEnemies(float duration)//BOSS房间刷新怪物
    {
        int count = 0;

        while (true)
        {
            if (count < B_Manager.MAX_MON_NUMS)
            {
                yield return new WaitForSeconds(duration);


                //Transform BornRoom_tf;
                //BornRoom_tf = GameObject.Find("Boss_BornZoom").transform;
                //Vector3 pos = new Vector3(BornRoom_tf.position.x, BornRoom_tf.position.y, BornRoom_tf.position.z);
                //// 开始刷新怪物
                //Instantiate(This_Room_Boss[0], pos, Quaternion.identity);
                count++;
            }
            else
            {
                B_Manager.FINISH_SPAWN();
                break;
            }
        }

    }

}

public class Bound : MonoBehaviour
{
    public Vector3 dL;
    public Vector3 dR;
    public Vector3 sR;
    public Vector3 sL;
    public Vector3 center;
    
    public float y;


    public Bound(Vector3 dL, Vector3 dR, Vector3 sR, Vector3 sL, Vector3 center,float y)
    {
        this.dL = dL;
        this.dR = dR;
        this.sR = sR;
        this.sL = sL;
        this.center = center;
        this.y = y;
    }

  



    public float getRandomX()
    {
        float num = Random.Range(dL.x, dR.x);
        return num;
    }


    public float getRandomZ()
    {
        float num = Random.Range(dL.z, sL.z);
        return num;
    }






}