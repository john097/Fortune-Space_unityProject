using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    public int playerCredit;
    public Text image_Credit;

    public float killstreaksTime;
    private float killstreaksTimer;
    public int killstreaksCredit;
    private int killstreaksNum;
    private GameObject killstreaksUI;

    //Current_State关卡变量名

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player");
        killstreaksNum = 0;
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
            ResetKillstreaksNum();
        }
        else if(killstreaksNum >= 2)
        {
            killstreaksTimer += Time.deltaTime;
        }

        if (killstreaksNum >= 2)
        {
            //出现连杀提示UI
            if (!killstreaksUI)
            {

            }
        }
        else
        {
            if (killstreaksUI)
            {
                Destroy(killstreaksUI);
            }
        }
    }

    //重置连杀奖励
    public void ResetKillstreaksNum()
    {
        killstreaksTimer = killstreaksTime + 1;
        killstreaksNum = 0;
    }

    public void AddKillstreaksNum()
    {
        killstreaksNum += 1;
        if (killstreaksTimer < killstreaksTime && killstreaksTimer > 0)
        {
            killstreaksTimer = 0;
        }
        else if(killstreaksTimer >= killstreaksTime && killstreaksNum >= 2)
        {
            killstreaksTimer = 0;
        }
    }


    

}
