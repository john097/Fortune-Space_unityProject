using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Stage : MonoBehaviour
{
    [HideInInspector]
    public Skill toolSkill;
    [HideInInspector]
    public Skill[] storeSkills;
    private Actor actor;

    public float heal;

    public enum toolType
    {
        道具,
        商店,
        特殊交互点
    }

    public enum createType
    {
        固定,
        随机
    }

    public toolType thisToolType;
    public createType thisCreateType;

    public GameObject interactiveUI;
    public GameObject storeUI;

    private BattleManager pp_BattleManager;//DISON.VER
    private Dialog_Manager pp_DialogManager;//DISON.VER



    // Start is called before the first frame update
    void Start()
    {
        actor = GameObject.Find("Actor").GetComponent<Actor>();

        if (thisToolType != toolType.特殊交互点)
        {
            if (thisCreateType == createType.固定)
            {
                RefrashToolList();
            }
            else if (thisCreateType == createType.随机)
            {
                RandomTools(0);
            }
        }
        else
        {
            pp_BattleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();//DISON.VER
            pp_DialogManager = GameObject.Find("BattleManager").GetComponent<Dialog_Manager>();//DISON.VER
        }
    }

    // Update is called once per frame
    void Update()
    {
        

      
    }

    public void ReGetRandomTools()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
        RandomTools(0);
    }

    public void RefrashToolList()
    {
        if (thisToolType == toolType.道具)
        {
            toolSkill = gameObject.transform.GetComponentInChildren<Skill>();
        }
        else if (thisToolType == toolType.商店)
        {
            storeSkills = new Skill[gameObject.transform.childCount];
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                storeSkills[i] = gameObject.transform.GetChild(i).GetComponent<Skill>();
            }
        }
    }

    //随机刷道具
    private void RandomTools(int k)
    {
        GameObject sPrefabs;
        GameObject s;

        //武器随机数量
        int[] wHArr = GetRandomSequence(46);
        //技能随机数量
        int[] sHArr = GetRandomSequence(4);

        if (thisToolType == toolType.道具)
        {
            int t = Random.Range(0,2);

            if (t == 0)
            {
                int j = Random.Range(0, k + 1);
                sPrefabs = Resources.Load("SkillPrefabs/Weapon/Level_" + j + "/" + wHArr[0] + "/S_" + wHArr[0]) as GameObject;
                s = Instantiate(sPrefabs, gameObject.transform);
            }
            else 
            {
                int j = Random.Range(0, k + 1);
                sPrefabs = Resources.Load("SkillPrefabs/Skills/Level_" + j + "/" + sHArr[0] + "/S_" + sHArr[0]) as GameObject;
                s = Instantiate(sPrefabs, gameObject.transform);
            }
        }
        else if (thisToolType == toolType.商店)
        {
            //四武器
            for (int i = 0; i < 4; i++)
            {
                int j = Random.Range(0, k + 1);
                sPrefabs = Resources.Load("SkillPrefabs/Weapon/Level_" + j + "/" + wHArr[i] + "/S_" + wHArr[i]) as GameObject;
                s = Instantiate(sPrefabs, gameObject.transform);
            }

            //两技能
            for (int i = 0; i < 2; i++)
            {
                int j = Random.Range(0, k + 1);
                int h = Random.Range(0, 4);
                sPrefabs = Resources.Load("SkillPrefabs/Skills/Level_" + j + "/" + sHArr[i] + "/S_" + sHArr[i]) as GameObject;
                s = Instantiate(sPrefabs, gameObject.transform);
            }
        }

        RefrashToolList();
    }

    //生成不重复随机数数组
    public static int[] GetRandomSequence(int total)
    {
        int[] hashtable = new int[total];
        int[] output = new int[total];

        for (int i = 0; i < total; i++)
        {
            int num = Random.Range(0, total);
            while (hashtable[num] > 0)
            {
                num = Random.Range(0, total);
            }

            output[i] = num;
            hashtable[num] = 1;
        }

        return output;
    }

    private void SkillDataSeedFunc(Skill s)
    {
        switch (s.thisWeaponType)
        {
            case Actor.weaponType.非武器:

                break;
            case Actor.weaponType.手枪:
                break;
            case Actor.weaponType.冲锋枪:
                break;
            case Actor.weaponType.霰弹枪:
                break;
            case Actor.weaponType.狙击枪:
                break;
            case Actor.weaponType.太刀:
                break;
            case Actor.weaponType.锤子:
                break;
            default:
                break;
        }
    }

    public void UseFunc()
    {
        actor.isTakingTool = true;
        if (thisToolType == toolType.道具)
        {
            GetTool();
        }
        else if(thisToolType == toolType.商店)
        {
            GameObject a = Instantiate(storeUI, GameObject.Find("Canvas").transform);
            a.GetComponent<SelectUIScript>().tool = gameObject.GetComponent<Stage>();
            a.GetComponent<SelectUIScript>().SetInformation();
        }
        else if (thisToolType == toolType.特殊交互点)
        {
            actor.isTakingTool = false;
            if (gameObject.tag == "ProtectPoint")//保护据点触发器//DISON.VER
            {
                pp_DialogManager.ProtectPoint_Start_Talk();//触发对话//DISON.VER
                pp_BattleManager.Special_Battle_Start();
            }


            if (gameObject.tag == "DeadPoint")//死斗触发器//DISON.VER
            {
                pp_DialogManager.DeadPoint_Start_Talk();//触发对话
                pp_BattleManager.Special_Battle_Start();
            }

            if (gameObject.tag == "TP_GATE")//传送触发器//DISON.VER
            {
                pp_DialogManager.TP_Talk();//触发对话
                
            }
        }
    }

    public void GetTool()
    {
        if (toolSkill.thisWeaponType != Actor.weaponType.非武器)
        {
            ExchangeSkill(0);
        }
        else
        {
            GameObject a = Instantiate(interactiveUI, GameObject.Find("Canvas").transform);
            a.GetComponent<SelectUIScript>().tool = gameObject.GetComponent<Stage>();
        }
    }

    public void ExchangeSkill(int i)
    {
        Skill s;

        toolSkill.transform.parent = GameObject.Find("Actor").transform.Find("Skills");
        toolSkill.transform.position = Vector3.zero;
        s = toolSkill.GetComponent<Skill>();
        s.actor = actor;
        if (s.gameObject.transform.childCount != 0)
        {
            for (int k = 0; k < s.gameObject.transform.childCount; k++)
            {
                s.gameObject.transform.GetChild(k).GetComponent<Skill>().actor = actor;
            }
        }
        

        switch (i)
        {
            case 0:
                if (s.thisWeaponType != Actor.weaponType.太刀 && s.thisWeaponType != Actor.weaponType.锤子 && s.thisWeaponType != Actor.weaponType.非武器)
                {
                    DropSkill(actor.Skills_0[0]);
                    actor.Skills_0[0] = s;
                    actor.isTakingTool = false;
                    break;
                }
                else if (s.thisWeaponType == Actor.weaponType.太刀 || s.thisWeaponType == Actor.weaponType.锤子)
                {
                    DropSkill(actor.Skills_1[0]);
                    actor.Skills_1[0] = s;
                    actor.isTakingTool = false;
                    break;
                }
                else
                {
                    break;
                }

            case 1:
                DropSkill(actor.Skills_0[1]);
                actor.Skills_0[1] = s;
                break;

            case 2:
                DropSkill(actor.Skills_0[2]);
                actor.Skills_0[2] = s;
                break;

            case 3:
                DropSkill(actor.Skills_1[1]);
                actor.Skills_1[1] = s;
                break;

            case 4:
                DropSkill(actor.Skills_1[2]);
                actor.Skills_1[2] = s;
                break;
        }

        
        actor.isTakingTool = false;

        if (thisToolType == Stage.toolType.道具)
        {
            Destroy(gameObject);
        }
    }

    private void DropSkill(Skill s)
    {
        GameObject tPrefab = Resources.Load("Prefabs/ToolPrefab") as GameObject;
        GameObject t = Instantiate(tPrefab, null);

        s.actor = null;
        if (s.gameObject.transform.childCount != 0)
        {
            for (int k = 0; k < s.gameObject.transform.childCount; k++)
            {
                s.gameObject.transform.GetChild(k).GetComponent<Skill>().actor = null;
            }
        }

        s.transform.parent = t.transform;
        s.transform.position = Vector3.zero;

        t.transform.position = actor.transform.position;
        t.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.01f, ForceMode.Impulse);
    }

    public void TakeDamege(float i)
    {
        heal -= i;

        if (heal <= 0)
        {
            heal = 0;//DISON.VER
        }
    }

}
