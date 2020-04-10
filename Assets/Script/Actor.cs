﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public enum specialType
    {
        无,
        远,
        近
    }

    public enum weaponType
    {
        非武器,
        手枪,
        冲锋枪,
        霰弹枪,
        狙击枪,
        太刀,
        锤子
    }

    public enum normalState
    {
        Idle,
        Run_F,
        Run_B,
        Run_L,
        Run_R,
        Behit,
        Dead
    }

    public enum attackState
    {
        NormalAttack,
        Reload,
        Kick,
        PinDown,
        Eliminate,
        Revenge_0,
        Revenge_1,
        Step
    }

    [Tooltip("是否是玩家角色")]
        public bool isPlayer;

    [Tooltip("角色类型")]
        public specialType thisSpType;

    [Tooltip("最大生命值")]
        public float maxHeal;

    [Tooltip("当前生命值")]
        public float heal;

    [Tooltip("角色是否存活")]
        public bool isAlive;

    [Tooltip("角色移动速度")]
        public float speed;

    [Tooltip("鼠标射线点与角色的最小距离")]
        public float minPointDistance;

    [Tooltip("角色看向目标点旋转速度")]
        public float lookAtSpeed;

    [Tooltip("当前使用的技能组")]
        public int skillArrNum;

    [Tooltip("技能对应的按键")]
        public KeyCode[] playerControl;

    [Tooltip("存放技能预支物的数组_0")]
        public Skill[] Skills_0;

    [Tooltip("存放技能预支物的数组_1")]
        public Skill[] Skills_1;

    [Tooltip("特殊交互键")]
        public KeyCode specialInteractiveKey;

    private GameObject[] Tools;

    //[HideInInspector]
        public Skill UsingSkill;//正在释放的技能

    //[HideInInspector]
        public GameObject aWarning;

    private bool steping;//是否处于闪避状态

    public Buff imprisonmentBuff;//禁锢Buff
    public Buff silenceBuff;//沉默Buff
    public Buff slowDownBuff;//减速Buff
    public Buff vulnerabilityBuff;//易伤Buff
    public Buff healOverBuff;//回血Buff
    public Buff damageOverTimeBuff;//持续伤害Buff
    public Buff coolDownBuff;//冷却缩减Buff
    public Buff superArmorBuff;//霸体Buff

    [HideInInspector]
        public Vector3 moveDirection;

    [HideInInspector]
    public Vector3 thisTimeForward;

    Collider thisCollider;

    [HideInInspector]
        public Rigidbody thisRigidbody;

    [HideInInspector]
        public Animator thisAnimator;

    [HideInInspector]
        public bool lookAtTag;

    [HideInInspector]
        public bool isTakingTool;

    [HideInInspector]
        public weaponType nowThisTakeWeapon;

    [HideInInspector]
        public int nowThisTakeWeaponNum;

    public float recoverTimeScaleTimer;

    //[HideInInspector]
        public float hitChangeTimeScaleTime;

    private const string weaponPrefabsPaths = "Prefabs/Weapons/";

    void Start()
    {
        nowThisTakeWeapon = weaponType.非武器;
        heal = maxHeal;
        lookAtTag = true;
        if (isPlayer)
        {
            thisAnimator = gameObject.transform.Find("ActorModel").GetComponent<Animator>();
        }
        thisRigidbody = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
        steping = false;
        skillArrNum = 0;
        Tools = new GameObject[10];
        isTakingTool = false;
    }

    void Update()
    {
        if (isAlive && isPlayer)
        {
            if (!isTakingTool)
            {
                Move();
                
                Look();

                Skill(skillArrNum);

                SpecialInteractive();
            }

            RecoverTimeScale();

            changeWeaponModel();
        }

        
    }

    private void Look()
    {
        if (lookAtTag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            LayerMask layerMask = 0;
            layerMask = 1 << LayerMask.NameToLayer("Environment");
            Vector3 dir = Vector3.zero;

            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                dir = new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z);

                if (Vector3.Distance(gameObject.transform.position, dir) > minPointDistance)
                {
                    Quaternion rBefore = gameObject.transform.rotation;
                    gameObject.transform.LookAt(dir);
                    Quaternion rNow = gameObject.transform.rotation;
                    gameObject.transform.rotation = Quaternion.Lerp(rBefore,rNow,Time.deltaTime*lookAtSpeed);
                }
            }
        }
    }

    //场景交互
    public void SpecialInteractive()
    {
        if (Input.GetKeyDown(specialInteractiveKey))
        {
            bool b = false;
            float dis = 9999;
            int selectIndex = -1;

            for (int i = 0; i < Tools.Length; i++)
            {
                if (Tools[i])
                {
                    b = true;
                    break;
                }
            }

            if (b)
            {
                for (int i = 0; i < Tools.Length; i++)
                {
                    if (Tools[i])
                    {
                        if (Vector3.Distance(Tools[i].transform.position, gameObject.transform.position) < dis)
                        {
                            selectIndex = i;
                            dis = Vector3.Distance(Tools[i].transform.position, gameObject.transform.position);
                        }
                    }
                }

                if (selectIndex != -1)
                {
                    Tools[selectIndex].GetComponent<Tool>().UseFunc();
                }
            }
        }
    }

    //移动
    private void Move()
    {
        if (!imprisonmentBuff && isAlive && isPlayer)//是否存在禁锢Buff
        {
            moveDirection = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");

            if (steping)
            {
                moveDirection = moveDirection.normalized;
            }

            //是否憨憨玩家没有按方向键
            if (steping && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                moveDirection = thisTimeForward;
            }

            if (slowDownBuff && !steping)
            {
                moveDirection = moveDirection * speed * slowDownBuff.GetComponent<Buff>().percent;
            }
            else 
            {
                moveDirection *= speed;
            }

            //播放动画
            NowMoveState(moveDirection);

            thisRigidbody.velocity = moveDirection;
        }

    }

    //移动动画
    private void NowMoveState(Vector3 dir)
    {
        if (isPlayer)
        {
            Vector3 d = dir;
            d = Quaternion.AngleAxis(Vector3.Angle(Vector3.forward, transform.forward), Vector3.up) * d;

            if (Vector3.Angle(Vector3.right, transform.forward) < 45 && transform.forward.x > 0)
            {
                d.z = -d.z;
            }

            thisAnimator.SetFloat("HSpeed", d.x);
            thisAnimator.SetFloat("VSpeed", d.z);

            if (skillArrNum == 0)
            {
                if (Skills_0[0].thisWeaponType == weaponType.手枪)
                {
                    thisAnimator.SetBool("ShortGun", true);
                    thisAnimator.SetBool("LongGun", false);
                    thisAnimator.SetBool("CoolWeapon", false);
                }
                else if (Skills_0[0].thisWeaponType != weaponType.太刀 && Skills_0[0].thisWeaponType != weaponType.锤子 && Skills_0[0].thisWeaponType != weaponType.非武器)
                {
                    thisAnimator.SetBool("ShortGun", false);
                    thisAnimator.SetBool("LongGun", true);
                    thisAnimator.SetBool("CoolWeapon", false);
                }
            }
            else
            {
                thisAnimator.SetBool("ShortGun", false);
                thisAnimator.SetBool("LongGun", false);
                thisAnimator.SetBool("CoolWeapon", true);
            }
        }
    }

    //技能、攻击动画触发
    public void StartAnim(int aS,float skillAnimPlaySpeed,int k)
    {
        if (aS == 0)
        {
            thisAnimator.SetBool("NormalAttack",true);
        }
        else if (aS == -1)
        {
            thisAnimator.SetBool("NormalAttack", false);
            thisAnimator.SetTrigger("Reload");
        }
        else if (aS == 1)
        {
            thisAnimator.SetTrigger("cNormalAttack");
            thisAnimator.SetBool("NormalAttack", false);
        }
        else
        {
            thisAnimator.SetBool("NormalAttack", false);
        }

        if (k==0)
        {
            thisAnimator.SetFloat("SkillAnimSpeed", skillAnimPlaySpeed);
        }
        else
        {
            thisAnimator.SetFloat("SkillAnimSpeed_1", skillAnimPlaySpeed);
        }
        
        thisAnimator.SetInteger("Skill", aS);

        
    }

    //技能、攻击动画停止
    public void StopAnim()
    {
        thisAnimator.SetBool("NormalAttack", false);

        thisAnimator.SetInteger("Skill", 0);
    }

    //实时更换武器模型
    private void changeWeaponModel()
    {
        if (skillArrNum == 0 && (Skills_0[0].thisWeaponType != nowThisTakeWeapon || Skills_0[0].weaponModelNum != nowThisTakeWeaponNum))
        {
            //销毁原武器
            switch (nowThisTakeWeapon)
            {
                case weaponType.手枪:
                    if (GameObject.Find("Ho_Pistol").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_Pistol").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.冲锋枪:
                    if (GameObject.Find("Ho_SMG").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_SMG").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.霰弹枪:
                    if (GameObject.Find("Ho_ShotGun").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_ShotGun").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.狙击枪:
                    if (GameObject.Find("Ho_Sniper").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_Sniper").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.太刀:
                    if (GameObject.Find("Ho_Katana").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_Katana").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.锤子:
                    if (GameObject.Find("Ho_Hammer").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_Hammer").transform.GetChild(0).gameObject);
                    }
                    break;
            }

            GameObject wPrefab;
            GameObject w;

            //生成新武器
            switch (Skills_0[0].thisWeaponType)
            {
                case weaponType.手枪:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "Pistol/" + "Pistol_" + Skills_0[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_Pistol").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.冲锋枪:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "SMG/" + "SMG_" + Skills_0[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_SMG").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1,1,1);
                    break;
                case weaponType.霰弹枪:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "ShotGun/" + "ShotGun_" + Skills_0[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_ShotGun").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.狙击枪:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "Sniper/" + "Sniper_" + Skills_0[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_Sniper").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.太刀:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "Katana/" + "Katana_" + Skills_0[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_Katana").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.锤子:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "Hammer/" + "Hammer_" + Skills_0[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_Hammer").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                default:
                    break;
            }

            nowThisTakeWeapon = Skills_0[0].thisWeaponType;
            nowThisTakeWeaponNum = Skills_0[0].weaponModelNum;
        }
        else if (skillArrNum == 1 && (Skills_1[0].thisWeaponType != nowThisTakeWeapon || Skills_1[0].weaponModelNum != nowThisTakeWeaponNum))
        {
            //销毁原武器
            switch (nowThisTakeWeapon)
            {
                case weaponType.手枪:
                    if (GameObject.Find("Ho_Pistol").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_Pistol").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.冲锋枪:
                    if (GameObject.Find("Ho_SMG").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_SMG").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.霰弹枪:
                    if (GameObject.Find("Ho_ShotGun").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_ShotGun").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.狙击枪:
                    if (GameObject.Find("Ho_Sniper").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_Sniper").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.太刀:
                    if (GameObject.Find("Ho_Katana").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_Katana").transform.GetChild(0).gameObject);
                    }
                    break;
                case weaponType.锤子:
                    if (GameObject.Find("Ho_Hammer").transform.childCount != 0)
                    {
                        Destroy(GameObject.Find("Ho_Hammer").transform.GetChild(0).gameObject);
                    }
                    break;
            }

            GameObject wPrefab;
            GameObject w;

            //生成新武器
            switch (Skills_1[0].thisWeaponType)
            {
                case weaponType.手枪:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "Pistol/" + "Pistol_" + Skills_1[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_Pistol").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.冲锋枪:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "SMG/" + "SMG_" + Skills_1[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_SMG").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.霰弹枪:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "ShotGun/" + "ShotGun_" + Skills_1[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_ShotGun").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.狙击枪:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "Sniper/" + "Sniper_" + Skills_1[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_Sniper").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.太刀:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "Katana/" + "Katana_" + Skills_1[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_Katana").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case weaponType.锤子:
                    wPrefab = Resources.Load(weaponPrefabsPaths + "Hammer/" + "Hammer_" + Skills_1[0].weaponModelNum) as GameObject;
                    w = Instantiate(wPrefab, GameObject.Find("Ho_Hammer").transform);
                    w.transform.localPosition = Vector3.zero;
                    w.transform.localEulerAngles = Vector3.zero;
                    w.transform.localScale = new Vector3(1, 1, 1);
                    break;
                default:
                    break;
            }

            nowThisTakeWeapon = Skills_1[0].thisWeaponType;
            nowThisTakeWeaponNum = Skills_1[0].weaponModelNum;
        }
    }

    public void SetSteping(bool a)
    {
        steping = a;

        if (imprisonmentBuff)
        {
            Destroy(imprisonmentBuff);
        }

        if (a)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Steping");
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Actor");
        }
        thisTimeForward = transform.forward;
    }

    public void SetLookAtTag(bool a)
    {
        lookAtTag = a;
    }

    private void SetAlive(bool a)
    {
        isAlive = a;
    }

    //技能释放事件监听函数
    private void Skill(int i)
    {
        if (i == 0)
        {
            if (Input.GetKey(playerControl[0]))
            {
                Skills_0[0].UseSkill();
            }

            if (!silenceBuff)
            {
                if (Input.GetKey(playerControl[1]))
                {
                    Skills_0[1].UseSkill();
                }

                if (Input.GetKey(playerControl[2]))
                {
                    Skills_0[2].UseSkill();
                }

                if (Input.GetKey(playerControl[3]))
                {
                    Skills_0[3].UseSkill();
                }

                if (Input.GetKey(playerControl[4]))
                {
                    Skills_0[4].UseSkill();
                }
            }
        }
        else if (i == 1)
        {
            if (Input.GetKey(playerControl[0]))
            {
                Skills_1[0].UseSkill();
            }

            if (!silenceBuff)
            {
                if (Input.GetKey(playerControl[1]))
                {
                    Skills_1[1].UseSkill();
                }

                if (Input.GetKey(playerControl[2]))
                {
                    Skills_1[2].UseSkill();
                }

                if (Input.GetKey(playerControl[3]))
                {
                    Skills_1[3].UseSkill();
                }

                if (Input.GetKey(playerControl[4]))
                {
                    Skills_1[4].UseSkill();
                }
            }
        }
    }

    public void SetBuff(Buff a)
    {
        if (a.thisType == Buff.buffType.止步)
        {
            imprisonmentBuff = a;
            thisRigidbody.velocity = Vector3.zero;
        }
        else if (a.thisType == Buff.buffType.沉默)
        {
            silenceBuff = a;
        }
        else if (a.thisType == Buff.buffType.减速)
        {
            slowDownBuff = a;
        }
        else if (a.thisType == Buff.buffType.易伤)
        {
            vulnerabilityBuff = a;
        }
        else if (a.thisType == Buff.buffType.回血)
        {
            healOverBuff = a;
        }
        else if (a.thisType == Buff.buffType.持续伤害)
        {
            damageOverTimeBuff = a;
        }
        else if (a.thisType == Buff.buffType.冷却缩减)
        {
            coolDownBuff = a;
        }
        else if (a.thisType == Buff.buffType.霸体)
        {
            superArmorBuff = a;
        }
        else if (a.thisType == Buff.buffType.斩杀20)
        {
            if (heal < (maxHeal * 0.2))
            {
                TakeDamege(heal);
            }
        }
        else if (a.thisType == Buff.buffType.装备切换)
        {
            skillArrNum = (int)a.percent;
        }
    }

    public void BuffAction(Buff.buffType i)
    {
        if (i == Buff.buffType.回血)
        {
            TakeDamege(-maxHeal * healOverBuff.percent);
        }
        else if (i == Buff.buffType.持续伤害)
        {
            TakeDamege(maxHeal * damageOverTimeBuff.percent);
        }
    }

    //受伤函数响应
    public void TakeDamege(float i)
    {
        if (isAlive)
        {
            if (vulnerabilityBuff)
            {
                heal -= i * vulnerabilityBuff.GetComponent<Buff>().percent;
            }
            else
            {
                heal -= i;
            }

            if (heal <= 0)
            {
                heal = 0;
                GoDie();
            }
            else
            {
                if (i>0 && !superArmorBuff && UsingSkill)
                {
                    UsingSkill.SkillActionStop(true);
                    //播放受击动画
                }
            }

            if (heal >= maxHeal)
            {
                heal = maxHeal;
            }
        }
    }

    public void GoDie()
    {
        SetAlive(false);
    }

    private void RecoverTimeScale()
    {
        if (Time.timeScale != 1)
        {
            if (recoverTimeScaleTimer >= hitChangeTimeScaleTime)
            {
                Time.timeScale = 1;
                recoverTimeScaleTimer = 0;
                hitChangeTimeScaleTime = 0;
            }
            else
            {
                recoverTimeScaleTimer += Time.deltaTime / Time.timeScale;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Tool"))
        {
            for (int i = 0; i < Tools.Length; i++)
            {
                if (!Tools[i])
                {
                    Tools[i] = other.gameObject;
                    break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Tool"))
        {
            for (int i = 0; i < Tools.Length; i++)
            {
                if (Tools[i] == other.gameObject)
                {
                    Tools[i] = null;
                    break;
                }
            }
        }
    }

}
