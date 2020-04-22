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
            killstreaksNum = 0;
        }
        else
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
    }


    

}
