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

    public enum toolType
    {
        道具,
        商店
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

    // Start is called before the first frame update
    void Start()
    {
        actor = GameObject.Find("Actor").GetComponent<Actor>();

        if (thisCreateType == createType.固定)
        {
            RefrashToolList();
        }
        else
        {
            RandomTools(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefrashToolList()
    {
        if (thisToolType == toolType.道具)
        {
            toolSkill = gameObject.transform.GetComponentInChildren<Skill>();
        }
        else
        { 
            storeSkills = new Skill[gameObject.transform.childCount];
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                storeSkills[i] = gameObject.transform.GetChild(i).GetComponent<Skill>();
            }
        }
    }

    private void RandomTools(int k)
    {
        GameObject sPrefabs;
        GameObject s;

        if (thisToolType == toolType.道具)
        {
            int t = Random.Range(0,2);

            if (t == 0)
            {
                int j = Random.Range(0, k + 1);
                int h = Random.Range(0, 5);
                sPrefabs = Resources.Load("SkillPrefabs/Weapon/Level_" + j + "/" + h + "/S_" + h) as GameObject;
                s = Instantiate(sPrefabs, gameObject.transform);
            }
            else
            {
                int j = Random.Range(0, k + 1);
                int h = Random.Range(0, 4);
                sPrefabs = Resources.Load("SkillPrefabs/Skills/Level_" + j + "/" + h + "/S_" + h) as GameObject;
                s = Instantiate(sPrefabs, gameObject.transform);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                int j = Random.Range(0, k + 1);
                int h = Random.Range(0, 5);
                sPrefabs = Resources.Load("SkillPrefabs/Weapon/Level_" + j + "/" + h + "/S_" + h) as GameObject;
                s = Instantiate(sPrefabs, gameObject.transform);
            }

            for (int i = 0; i < 2; i++)
            {
                int j = Random.Range(0, k + 1);
                int h = Random.Range(0, 4);
                sPrefabs = Resources.Load("SkillPrefabs/Skills/Level_" + j + "/" + h + "/S_" + h) as GameObject;
                s = Instantiate(sPrefabs, gameObject.transform);
            }
        }

        RefrashToolList();
    }

    public void UseFunc()
    {
        actor.isTakingTool = true;
        if (thisToolType == toolType.道具)
        {
            GetTool();
        }
        else
        {
            GameObject a = Instantiate(storeUI, GameObject.Find("Canvas").transform);
            a.GetComponent<SelectUIScript>().tool = gameObject.GetComponent<Stage>();
            a.GetComponent<SelectUIScript>().SetInformation();
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

}
