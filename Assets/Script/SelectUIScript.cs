using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIScript : MonoBehaviour
{
    [HideInInspector]
        public Stage tool;
    public Stage.toolType thisType;

    public Sprite nSprite;

    private GameObject[] messages;
    private int selectingTool;
    private Text randomToolButtonText;

    public bool skillSelectUI;
    private GameObject[] exchangeButton;

    // Start is called before the first frame update
    void Start()
    {
        if (!skillSelectUI)
        {
            randomToolButtonText = gameObject.transform.GetChild(2).gameObject.GetComponentInChildren<Text>();
        }
        else 
        {
            exchangeButton = new GameObject[4];

            for (int i = 0; i < 4; i++)
            {
                exchangeButton[i] = gameObject.transform.GetChild(0).transform.GetChild(i+1).gameObject;
            }
            for (int i = 0; i < 2; i++)
            {
                exchangeButton[i].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>().Skills_0[i+1].skillIcon;
            }
            for (int i = 0; i < 2; i++)
            {
                exchangeButton[i+2].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>().Skills_1[i+1].skillIcon;
            }
        }
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

    public void ReGetRandomTool()
    {
        tool.ReGetRandomTools();
        SetInformation();
    }

    public void ButtonPressEvent(int i)
    {
        tool.ExchangeSkill(i);
        Destroy(gameObject);
    }

    public void SeletTool(int i)
    {
        if (tool.storeSkills[i] && tool.storeSkills[i].credit <= GameObject.FindGameObjectWithTag("Player").GetComponent<Credit>().playerCredit)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Credit>().AddPlayerCredit(-tool.storeSkills[i].credit);
            tool.toolSkill = tool.storeSkills[i];
            tool.GetTool();
            tool.storeSkills[i] = null;
            Destroy(gameObject);
        }
        else
        {
            if (tool.storeSkills[i])
            {
                Debug.Log("积分不足");
            }
            else
            {
                Debug.Log("售空");
            }
            
        }
    }

    public void SetInformation()
    {
        messages = new GameObject[gameObject.transform.GetChild(1).childCount];

        for (int i = 0; i < gameObject.transform.GetChild(1).childCount; i++)
        {
            messages[i] = gameObject.transform.GetChild(1).GetChild(i).gameObject;
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
                messages[i].GetComponentInChildren<Text>().text = "售空";
                messages[i].GetComponent<Image>().sprite = nSprite;
            }
        }
    }

    private void InformationUpdata()
    {
        if (selectingTool >= 0 && tool.storeSkills[selectingTool])
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
            messages[messages.Length - 1].transform.GetChild(2).GetComponent<Text>().text = " ";
            messages[messages.Length - 1].transform.GetChild(3).GetComponent<Text>().text = " ";
            messages[messages.Length - 1].transform.GetChild(4).GetComponent<Text>().text = " ";
            messages[messages.Length - 1].transform.GetChild(5).GetComponent<Text>().text = " ";
            messages[messages.Length - 1].transform.GetChild(6).GetComponent<Text>().text = " ";
        }

        randomToolButtonText.text = "花费" + tool.refrashCredit + "刷新商品";
    }

    public void DestroySelf()
    {
        tool.actor.isTakingTool = false;
        Destroy(gameObject);
    }
}
