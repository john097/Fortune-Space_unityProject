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
        tool.toolSkill = tool.storeSkills[i];
        tool.GetTool();
        Destroy(gameObject);
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
        }
        else
        {
            messages[messages.Length - 1].transform.GetChild(0).GetComponent<Text>().text = " ";
            messages[messages.Length - 1].transform.GetChild(1).GetComponent<Text>().text = " ";
        }
    }
}
