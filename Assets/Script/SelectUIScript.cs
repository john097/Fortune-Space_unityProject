using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIScript : MonoBehaviour
{
    [HideInInspector]
        public Stage tool;
    public Stage.toolType thisType;

    private GameObject[] messages;
    private int selectingTool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (thisType == Stage.toolType.商店)
        {
            InformationUpdata();
        }
    }

    public void ChangeSelectingTool(int i)
    {
            selectingTool = i;
    }

    public void ButtonPressEvent(int i)
    {
        tool.ExchangeSkill(i);
        Destroy(gameObject);
    }

    public void SeletTool(int i)
    {
        if (tool.storeSkills[i].credit <= GameObject.FindGameObjectWithTag("Player").GetComponent<Credit>().playerCredit)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Credit>().AddPlayerCredit(-tool.storeSkills[i].credit);
            tool.toolSkill = tool.storeSkills[i];
            tool.GetTool();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("积分不足");
        }
        
    }

    public void SetInformation()
    {
        messages = new GameObject[gameObject.transform.GetChild(0).childCount];

        for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
        {
            messages[i] = gameObject.transform.GetChild(0).GetChild(i).gameObject;
        }

        for (int i = 0; i < messages.Length - 1; i++)
        {
            if (tool.storeSkills[i])
            {
                messages[i].GetComponentInChildren<Text>().text = tool.storeSkills[i].skillName;
                messages[i].GetComponent<Image>().sprite = tool.storeSkills[i].skillIcon;
            }
            else
            {
                Destroy(messages[i]);
            }
        }
    }

    private void InformationUpdata()
    {
        if (selectingTool >= 0)
        {
            messages[messages.Length - 1].transform.GetChild(0).GetComponent<Text>().text = tool.storeSkills[selectingTool].skillName;
            messages[messages.Length - 1].transform.GetChild(1).GetComponent<Text>().text = tool.storeSkills[selectingTool].skillExplain;
            messages[messages.Length - 1].transform.GetChild(2).GetComponent<Text>().text = "价格： " + tool.storeSkills[selectingTool].credit;
            messages[messages.Length - 1].transform.GetChild(3).GetComponent<Text>().text = "伤害： " + tool.storeSkills[selectingTool].skillDamageExplain;
            messages[messages.Length - 1].transform.GetChild(4).GetComponent<Text>().text = "对易伤敌人的伤害倍率： " + tool.storeSkills[selectingTool].skillVulnerabilityExplain + "%";
            messages[messages.Length - 1].transform.GetChild(5).GetComponent<Text>().text = tool.storeSkills[selectingTool].skillRepelExplain;
            messages[messages.Length - 1].transform.GetChild(6).GetComponent<Text>().text = tool.storeSkills[selectingTool].skillSpecialExplain;
        }
        else
        {
            messages[messages.Length - 1].transform.GetChild(0).GetComponent<Text>().text = " ";
            messages[messages.Length - 1].transform.GetChild(1).GetComponent<Text>().text = " ";
        }
    }
}
