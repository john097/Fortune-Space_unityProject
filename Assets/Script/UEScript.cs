using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UEScript : MonoBehaviour
{
    [Tooltip("战斗UI寻找玩家对象")]
        public bool fightingUI;

    [Tooltip("敌人血条")]
        public bool enemyHealBar;

    Actor player;
    Credit playerCredit;
    Image healImage;
    Image[] skills;
    Text creditText;
    Text ammoText;
    Text weaponNameText;
    Image weaponIcon;

    GameObject followPlayerCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (fightingUI)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
            playerCredit = player.gameObject.GetComponent<Credit>();

            healImage = gameObject.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Image>();

            skills = new Image[4];
            skills[0] = gameObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>();
            skills[1] = gameObject.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>();
            skills[2] = gameObject.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>();
            skills[3] = gameObject.transform.GetChild(1).transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>();

            creditText = gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>();
            ammoText = gameObject.transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Text>();

            weaponNameText = gameObject.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Text>();
            weaponIcon = gameObject.transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<Image>();
        }
        else
        {
            healImage = gameObject.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Image>();
            player = gameObject.transform.parent.gameObject.GetComponent<Actor>();
            followPlayerCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>().FollowCamera;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fightingUI)
        {
            FightUIFunc();
        }

        if (enemyHealBar)
        {
            EnemyInfoUpdate();
            transform.LookAt(followPlayerCamera.transform);
        }
    }

    private void FightUIFunc()
    {
        //血量信息
        FillAmountUpdate(healImage,player.heal,0,player.maxHeal);

        //技能信息
        if (player.skillArrNum == 0)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                FillAmountUpdate(skills[i], player.Skills_0[i+1].coolDownTimer, 0, player.Skills_0[i+1].coolDownTime);
                skills[i].sprite = player.Skills_0[i + 1].skillIcon;
            }
        }
        else if(player.skillArrNum == 1)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                FillAmountUpdate(skills[i], player.Skills_1[i+1].coolDownTimer, 0, player.Skills_1[i+1].coolDownTime);
                skills[i].sprite = player.Skills_1[i + 1].skillIcon;
            }
        }

        //积分信息
        UpdateText(playerCredit.playerCredit + "",creditText);

        //子弹信息
        if (player.skillArrNum == 0)
        {
            if (player.Skills_0[0].ammoNumLimit == 0)
            {
                UpdateText("∞", ammoText);
            }
            else
            {
                UpdateText(player.Skills_0[0].ammoNum + "/" + player.Skills_0[0].ammoNumLimit, ammoText);
            }
        }
        else if(player.skillArrNum == 1)
        {
            if (player.Skills_1[0].ammoNumLimit == 0)
            {
                UpdateText("∞", ammoText);
            }
            else
            {
                UpdateText(player.Skills_1[0].ammoNum + "/" + player.Skills_1[0].ammoNumLimit, ammoText);
            }
        }

        //武器信息
        if (player.skillArrNum == 0)
        {
            UpdateText(player.Skills_0[0].skillName,weaponNameText);
            weaponIcon.sprite = player.Skills_0[0].skillIcon;
        }
        else if (player.skillArrNum == 1)
        {
            UpdateText(player.Skills_1[0].skillName, weaponNameText);
            weaponIcon.sprite = player.Skills_1[0].skillIcon;
        }
    }

    private void EnemyInfoUpdate()
    {
        //血量信息
        FillAmountUpdate(healImage, player.heal, 0, player.maxHeal);
    }

    private void FillAmountUpdate(Image img,float value,float minValue, float maxValue)
    {
        img.fillAmount = value / (maxValue - minValue);
    }

    public static void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public static void CreateUE(GameObject u)
    {
        GameObject uE = Instantiate(u, GameObject.Find("Canvas").transform);
    }

    public static void UpdateSilder(float value,float minValue,float maxValue,Slider s)
    {
        s.value = value;
        s.minValue = minValue;
        s.maxValue = maxValue;
    }

    public static void UpdateSilder(float value, Slider s)
    {
        s.value = value;
    }

    public static void UpdateText(string s,Text t)
    {
        t.text = s;
    }
}
