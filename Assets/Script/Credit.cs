using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    public int playerCredit;
    public Text image_Credit;

    public float killstreaksTime;
    public float killstreaksTimer;
    public int killstreaksCredit;
    public int killstreaksNum;
    private Animator killstreaksUI;
    private Text killstreaksUINum;

    public int eliminateCredit;
    private Animator eliminateUI;
    private Text eliminateUINum;

    //Current_State关卡变量名

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player");
        killstreaksNum = 0;
        killstreaksTimer = killstreaksTime +1;
        killstreaksUI = GameObject.Find("T_Killstreak").GetComponent<Animator>();
        killstreaksUINum = killstreaksUI.gameObject.GetComponent<Text>();
        eliminateUI = GameObject.Find("T_Eliminate").GetComponent<Animator>();
        eliminateUINum = eliminateUI.gameObject.GetComponent<Text>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (image_Credit)
        {
            image_Credit.text = playerCredit + "";
        }

        KillstreaksFunc();
    }

    //玩家积分增加
    public void AddPlayerCredit(int i)
    {
        playerCredit += i;
    }

    //玩家连杀奖励
    private void KillstreaksFunc()
    {
        if (killstreaksTimer >= killstreaksTime)
        {
            AddPlayerCredit(killstreaksCredit * killstreaksNum);
            killstreaksNum = 0;
        }
        else
        {
            killstreaksTimer += Time.deltaTime;
        }

        if (killstreaksNum >= 2)
        {
            //出现连杀提示UI
            if (killstreaksUI)
            {
                killstreaksUI.SetInteger("i", 1);
                killstreaksUINum.text = "Killstreak  × " + killstreaksNum;
            }
        }
        else
        {
            if (killstreaksUI)
            {
                killstreaksUI.SetInteger("i", 2);
            }
        }
    }

    //重置连杀奖励
    public void ResetKillstreaksNum()
    {
        killstreaksTimer = killstreaksTime + 1;
    }

    public void AddKillstreaksNum()
    {
        killstreaksNum += 1;
        killstreaksTimer = 0;
    }

    public void EliminateEvent()
    {
        eliminateUI.SetTrigger("Start");
        eliminateUINum.text = "Eliminate + " + eliminateCredit;
        AddPlayerCredit(eliminateCredit);
    }



}
