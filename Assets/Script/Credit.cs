using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    public int playerCredit;
    public Text image_Credit;

    //Current_State关卡变量名

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (image_Credit)
        {
            image_Credit.text = playerCredit + "";
        }
    }

    public void AddPlayerCredit(int i)
    {
        playerCredit += i;
    }
}
