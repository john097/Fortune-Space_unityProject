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

    [Tooltip("敌人血条")]
        public bool protecpointHealBar;

    [Tooltip("个人信息界面")]
        public bool playerMenu;

    [Tooltip("伤害提示")]
        public bool DamageTip;

    [Tooltip("特殊交互提示")]
        public bool SpecialInteractiveTip;

    private Text[] messages;

    Actor player;
    Stage protectpoint;
    Credit playerCredit;
    Image healImage;
    Image[] skills;
    Text creditText;
    Text ammoText;
    Text weaponNameText;
    Image weaponIcon;

    private int skillMode;
    private int selectingSkill;

    GameObject followPlayerCamera;

    [HideInInspector]
        public string damageTipText;
    [HideInInspector]
    public Color damageTipColor;
    private float damageTipTimer;
    public float damageTipTime;

    private Text specialInteractiveText;

    // Start is called before the first frame update
    void Start()
    {
        if (fightingUI)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
            playerCredit = player.gameObject.GetComponent<Credit>();

            healImage = GameObject.Find("I_Health").gameObject.GetComponent<Image>();

            skills = new Image[4];
            for (int i = 0; i < 4; i++)
            {
                skills[i] = gameObject.transform.GetChild(0).transform.GetChild(1).transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>();
            }

            creditText = GameObject.Find("T_Credit").gameObject.GetComponent<Text>();
            ammoText = GameObject.Find("T_Ammo").gameObject.GetComponent<Text>();

            //weaponNameText = gameObject.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Text>();
            weaponIcon = GameObject.Find("I_Weapon").gameObject.GetComponent<Image>();
        }
        else if(enemyHealBar)
        {
            healImage = gameObject.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Image>();
            player = gameObject.transform.parent.gameObject.GetComponent<Actor>();
            followPlayerCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>().FollowCamera;
        }
        else if (protecpointHealBar)//保护据点的血条***DISON.VER***
        {
            healImage = gameObject.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Image>();
            protectpoint = gameObject.transform.parent.gameObject.GetComponent<Stage>();
            followPlayerCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>().FollowCamera;
        }
        else if (playerMenu)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
            playerCredit = player.gameObject.GetComponent<Credit>();
            skillMode = player.skillArrNum;
            selectingSkill = -1;

            skills = new Image[5];
            for (int i = 0; i < 5; i++)
            {
                skills[i] = gameObject.transform.GetChild(1).transform.GetChild(i).gameObject.GetComponent<Image>();
            }

            messages = new Text[gameObject.transform.GetChild(1).transform.GetChild(5).childCount];
            messages = gameObject.transform.GetChild(1).transform.GetChild(5).GetComponentsInChildren<Text>();

            creditText = gameObject.transform.GetChild(1).transform.GetChild(6).transform.GetChild(0).gameObject.GetComponent<Text>();
        }
        else if (DamageTip)
        {
            damageTipTimer = 0;
            transform.Rotate(0,180,0);
            GetComponent<Text>().text = damageTipText;
            GetComponent<Text>().color = damageTipColor;
        }
        else if (SpecialInteractiveTip)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
            specialInteractiveText = gameObject.GetComponentInChildren<Text>();
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

        if (protecpointHealBar)//保护据点的血条***DISON.VER***
        {
            ProtectPointInfoUpdate();
            transform.LookAt(followPlayerCamera.transform);
        }

        if (playerMenu)
        {
            PlayerMenuUpdate();
        }

        if (DamageTip)
        {
            DamageTipUpdate();
        }

        if (SpecialInteractiveTip)
        {
            int k = 0;
            float dis = 9999999;
            for (int i = 0; i < player.Tools.Length; i++)
            {
                if (player.Tools[i])
                {
                    if (Vector3.Distance(player.Tools[i].transform.position, player.gameObject.transform.position) < dis)
                    {
                        k = i;
                        dis = Vector3.Distance(player.Tools[i].transform.position, player.gameObject.transform.position);
                    }
                }
            }

            switch (player.Tools[k].GetComponent<Stage>().thisToolType)
            {
                case Stage.toolType.道具:
                    specialInteractiveText.text = "按 F 拾取道具";
                    break;
                case Stage.toolType.商店:
                    specialInteractiveText.text = "按 F 打开商店";
                    break;
                case Stage.toolType.特殊交互点:
                    switch (player.Tools[k].tag)
                    {
                        case "Heal_Treasure":
                            specialInteractiveText.text = "按 F 拾取血包";
                            break;
                        case "Gold_Treasure":
                            specialInteractiveText.text = "按 F 拾取金币箱";
                            break;
                        case "Miracle_Box":
                            specialInteractiveText.text = "按 F 打开箱子";
                            break;
                        case "Blood_Box":
                            specialInteractiveText.text = "按 F 消耗最大血量的25%开启武器箱";
                            break;
                        case "Gold_Box":
                            specialInteractiveText.text = "按 F 消耗" + player.Tools[k].GetComponent<Gold_Box>().ticket + "金币开启箱子";
                            break;
                        case "Power_Box":
                            specialInteractiveText.text = "按 F 拾取增幅箱";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            //if (player.Tools[k].GetComponent<Stage>().thisToolType == Stage.toolType.特殊交互点)
            //{
            //    switch (player.Tools[k].tag)
            //    {
            //        case "Heal_Treasure":
            //            specialInteractiveText.text = "按 F 拾取血包";
            //            break;
            //        case "Gold_Treasure":
            //            specialInteractiveText.text = "按 F 拾取金币箱";
            //            break;
            //        case "Miracle_Box":
            //            specialInteractiveText.text = "按 F 打开箱子";
            //            break;
            //        case "Blood_Box":
            //            specialInteractiveText.text = "按 F 消耗最大血量的25%开启武器箱";
            //            break;
            //        case "Gold_Box":
            //            specialInteractiveText.text = "按 F 消耗" + player.Tools[k].GetComponent<Gold_Box>().ticket + "金币开启箱子";
            //            break;
            //        case "Power_Box":
            //            specialInteractiveText.text = "按 F 拾取增幅箱";
            //            break;
            //        default:
            //            break;
            //    }
            //}
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
            //UpdateText(player.Skills_0[0].skillName,weaponNameText);
            weaponIcon.sprite = player.Skills_0[0].skillIcon;
        }
        else if (player.skillArrNum == 1)
        {
            //UpdateText(player.Skills_1[0].skillName, weaponNameText);
            weaponIcon.sprite = player.Skills_1[0].skillIcon;
        }
    }

    private void EnemyInfoUpdate()
    {
        //血量信息
        FillAmountUpdate(healImage, player.heal, 0, player.maxHeal);
        if (player.heal <= player.maxHeal*0.2)
        {
            healImage.color = Color.red;
        }
        else if(player.heal > player.maxHeal * 0.2)
        {
            if (player.vulnerabilityBuff)
            {
                healImage.color = Color.yellow;
            }
            else
            {
                healImage.color = Color.green;
            }
        }

        if (!player.isAlive)
        {
            if (gameObject.transform.Find("Slider_Heal"))
            {
                Destroy(gameObject.transform.Find("Slider_Heal").gameObject);
            }
            
        }
    }

    private void ProtectPointInfoUpdate()//保护据点的血条***DISON.VER***
    {
        FillAmountUpdate(healImage, protectpoint.heal, 0, protectpoint.maxHeal);

        if (protectpoint.heal <= protectpoint.maxHeal * 0.2)
        {
            healImage.color = Color.red;
        }
        else if (protectpoint.heal > protectpoint.maxHeal * 0.2&& protectpoint.heal <= protectpoint.maxHeal * 0.5)
        {     
            healImage.color = Color.yellow;
        }
        else
        {
            healImage.color = Color.green;
        }

        if (protectpoint.heal<=0)
        {
            if (gameObject.transform.Find("Slider_Heal"))
            {
                Destroy(gameObject.transform.Find("Slider_Heal").gameObject);
            }

        }
    }

    private void PlayerMenuUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            DestroySelf();
        }

        for (int i = 0; i < skills.Length; i++)
        {
            if (skillMode == 0)
            {
                skills[i].sprite = player.Skills_0[i].skillIcon;
            }
            else if(skillMode == 1)
            {
                skills[i].sprite = player.Skills_1[i].skillIcon;
            }
        }

        if (player && selectingSkill >= 0)
        {
            if (skillMode == 0)
            {
                messages[0].text = player.Skills_0[selectingSkill].skillName;
                messages[1].text = player.Skills_0[selectingSkill].skillExplain;
                messages[2].text = "伤害： " + player.Skills_0[selectingSkill].skillDamageExplain;
                messages[3].text = "对易伤敌人的伤害倍率： " + player.Skills_0[selectingSkill].skillVulnerabilityExplain;
                messages[4].text = player.Skills_0[selectingSkill].skillRepelExplain;
                messages[5].text = player.Skills_0[selectingSkill].skillSpecialExplain;
            }
            else if (skillMode == 1)
            {
                messages[0].text = player.Skills_1[selectingSkill].skillName;
                messages[1].text = player.Skills_1[selectingSkill].skillExplain;
                messages[2].text = "伤害： " + player.Skills_1[selectingSkill].skillDamageExplain;
                messages[3].text = "对易伤敌人的伤害倍率： " + player.Skills_1[selectingSkill].skillVulnerabilityExplain;
                messages[4].text = player.Skills_1[selectingSkill].skillRepelExplain;
                messages[5].text = player.Skills_1[selectingSkill].skillSpecialExplain;
            }
        }
        else
        {
            messages[0].text = "";
            messages[1].text = "";
            messages[2].text = "";
            messages[3].text = "";
            messages[4].text = "";
            messages[5].text = "";
        }

        creditText.text = playerCredit.playerCredit + "";
    }

    private void DamageTipUpdate()
    {
        if (damageTipTimer >= damageTipTime)
        {
            Destroy(gameObject);
        }
        else
        {
            damageTipTimer += Time.deltaTime;
            transform.position +=  new Vector3(0,Time.deltaTime,0);
        }
    }

    public  void ChangeSelectingSkill(int i)
    {
        selectingSkill = i;
    }

    public void ChangeSkillMode()
    {
        if (skillMode == 0)
        {
            skillMode = 1;
        }
        else if (skillMode == 1)
        {
            skillMode = 0;
        }
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

    public void DestroySelf()
    {
        if (playerMenu)
        {
            player.playerMenu = null;
            player.isOpeningPlayerMenu = false;
        }
        Destroy(gameObject);
    }
}
