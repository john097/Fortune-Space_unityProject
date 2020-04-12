using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;



public class Actor : MonoBehaviour
{
    public GameObject FollowCamera;

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

    [HideInInspector]
        public Skill UsingSkill;//正在释放的技能

    [HideInInspector]
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

    private float recoverTimeScaleTimer;

    [HideInInspector]
        public float hitChangeTimeScaleTime;

    public Slider healSlider;
    public Text ammoText;
    public Slider skill_0_Slider;
    public Slider skill_1_Slider;
    public Slider skill_2_Slider;
    public Slider skill_3_Slider;

    private Vector3 cForward;
    private Vector3 cRight;
    private Vector3 cUp;

    private const string weaponPrefabsPaths = "Prefabs/Weapons/";


    public bool isDead;//**DISON.ver**判断是否死亡
    public bool BeAttacked;//**DISON.ver**用于怪物巡逻判断（若被攻击，则终止巡逻）
    public bool isTalking;
    private BattleManager AC_manager;//**DISON.ver**
  

    void Start()
    {
        nowThisTakeWeapon = weaponType.非武器;
        heal = maxHeal;
        lookAtTag = true;
        if (isPlayer)
        {
            thisAnimator = gameObject.transform.Find("ActorModel").GetComponent<Animator>();

            Vector3 i = FollowCamera.transform.position;
            Vector3 k = FollowCamera.transform.GetChild(0).transform.position;
            i.y = 0;
            k.y = 0;

            cForward = k - i;
            cForward = cForward.normalized;

            i = FollowCamera.transform.position;
            k = FollowCamera.transform.GetChild(1).transform.position;

            i.y = 0;
            k.y = 0;

            cRight = k - i;
            cRight = cRight.normalized;
        }
        thisRigidbody = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
        steping = false;
        skillArrNum = 0;
        Tools = new GameObject[10];
        isTakingTool = false;

        isDead = false;//**DISON.ver**
        BeAttacked = false;//**DISON.ver**
        isTalking = false;//**DISON.ver**

        //AC_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();//**DISON.ver**

    }

    void Update()
    {
        
            if (isAlive && isPlayer&& !isTalking)//**DISON.ver**
        {
                if (!isTakingTool)
                {
                    //Move();

                    Skill(skillArrNum);

                    SpecialInteractive();
                }

                RecoverTimeScale();

                changeWeaponModel();
            }


            UIUpdate();

            RecoverTimeScale();

        //if (AC_manager.Dead_Room_Battle)//**DISON.ver**用于死斗房间结束时所有怪物自毁
        //{
        //    if (AC_manager.Dead_Fight_Timer > AC_manager.Dead_Fight_MaxTime && gameObject.layer == 10)
        //    {
        //        GoDie();
        //    }
        //}


    }

    private void FixedUpdate()
    {
       
            if (isAlive && isPlayer && !isTalking)//**DISON.ver**
        {
                if (!isTakingTool)
                {
                    Look();

                    Move();

                }
            }
        
            
    }

    private void UIUpdate()
    {
        if (healSlider)
        {
            UEScript.UpdateSilder(heal,0,maxHeal,healSlider);
        }

        if (ammoText)
        {
            if (skillArrNum == 0)
            {
                UEScript.UpdateText(Skills_0[0].ammoNum + "/" + Skills_0[0].ammoNumLimit, ammoText);
            }
            else if (skillArrNum == 1)
            {
                UEScript.UpdateText(Skills_1[0].ammoNum + "/" + Skills_1[0].ammoNumLimit, ammoText);
            }
        }

        if (skillArrNum == 0)
        {
            if (skill_0_Slider)
            {
                UEScript.UpdateSilder(Skills_0[1].coolDownTimer,0, Skills_0[1].coolDownTime,skill_0_Slider);
            }

            if (skill_1_Slider)
            {
                UEScript.UpdateSilder(Skills_0[2].coolDownTimer, 0, Skills_0[2].coolDownTime, skill_1_Slider);
            }

            if (skill_2_Slider)
            {
                UEScript.UpdateSilder(Skills_0[3].coolDownTimer, 0, Skills_0[3].coolDownTime, skill_2_Slider);
            }

            if (skill_3_Slider)
            {
                UEScript.UpdateSilder(Skills_0[4].coolDownTimer, 0, Skills_0[4].coolDownTime, skill_3_Slider);
            }
        }else if (skillArrNum == 1)
        {
            if (skill_0_Slider)
            {
                UEScript.UpdateSilder(Skills_1[1].coolDownTimer, 0, Skills_1[1].coolDownTime, skill_0_Slider);
            }

            if (skill_1_Slider)
            {
                UEScript.UpdateSilder(Skills_1[2].coolDownTimer, 0, Skills_1[2].coolDownTime, skill_1_Slider);
            }

            if (skill_2_Slider)
            {
                UEScript.UpdateSilder(Skills_1[3].coolDownTimer, 0, Skills_1[3].coolDownTime, skill_2_Slider);
            }

            if (skill_3_Slider)
            {
                UEScript.UpdateSilder(Skills_1[4].coolDownTimer, 0, Skills_1[4].coolDownTime, skill_3_Slider);
            }
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
                    Tools[selectIndex].GetComponent<Stage>().UseFunc();
                }
            }
        }
    }

    //移动
    private void Move()
    {
        if (!imprisonmentBuff && isAlive && isPlayer)//是否存在禁锢Buff
        {
            //moveDirection = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");

            moveDirection = cForward * Input.GetAxis("Vertical") + cRight * Input.GetAxis("Horizontal");

            if (steping)
            {
                moveDirection = moveDirection.normalized;
            }

            //是否憨憨玩家没有按方向键

            if (steping && Input.GetAxis("Horizontal") <= 0.01f && Input.GetAxis("Vertical") <= 0.01f)

            if (steping && Input.GetAxis("Horizontal") < 0.01 && Input.GetAxis("Vertical") < 0.01)
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
            NowMoveState(moveDirection.normalized);

            moveDirection.y = thisRigidbody.velocity.y;

            thisRigidbody.velocity = moveDirection;
           
        }

    }

    //移动动画
    private void NowMoveState(Vector3 dir)
    {
        if (isPlayer)
        {
            float v, h;
            v = Vector3.Distance(transform.position, transform.position + Vector3.Project(dir, transform.forward));
            h = Vector3.Distance(transform.position, transform.position + Vector3.Project(dir, transform.right));

            if (Vector3.Angle(dir, transform.forward) > 90)
            {
                v = -v;
            }

            if (Vector3.Angle(dir, transform.right) > 90)
            {
                h = -h;
            }

            thisAnimator.SetFloat("VSpeed", v);
            thisAnimator.SetFloat("HSpeed", h);


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
        if (isPlayer)//DISON.VER
        {
            if (aS == 0)
            {
                thisAnimator.SetBool("NormalAttack", true);
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

            if (k == 0)
            {
                thisAnimator.SetFloat("SkillAnimSpeed", skillAnimPlaySpeed);
            }
            else
            {
                thisAnimator.SetFloat("SkillAnimSpeed_1", skillAnimPlaySpeed);
            }

            thisAnimator.SetInteger("Skill", aS);
        }
            

        
    }

    //技能、攻击动画停止
    public void StopAnim()
    {
        if (isPlayer)//DISON.VER{
        {
            thisAnimator.SetBool("NormalAttack", false);

            thisAnimator.SetInteger("Skill", 0);
        }

           
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
        isDead = true;//**DISON.ver**   

        if (gameObject.layer == 10)
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().Mon_Dead();//**DISON.ver**用于怪物刷新系统的击杀怪物数量计算
            Destroy(gameObject);//***DISON.ver***
        }

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
            bool a = true;
            for (int i = 0; i < Tools.Length; i++)
            {
                if (Tools[i] == other.gameObject)
                {
                    a = false;
                }
                
            }

            if (a)
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
