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

    [Tooltip("角色积分")]
        public int actorCredit;

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

    [HideInInspector]
        public GameObject[] Tools;

    //[HideInInspector]
        public Skill UsingSkill;//正在释放的技能

    [HideInInspector]
        public GameObject aWarning;

    //[HideInInspector]
    public bool steping;//是否处于闪避状态

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

    //[HideInInspector]
        public  bool lookAtTag;

    [HideInInspector]
        public bool isTakingTool;

    [HideInInspector]
        public bool isOpeningPlayerMenu;

    [HideInInspector]
        public weaponType nowThisTakeWeapon;

    [HideInInspector]
        public int nowThisTakeWeaponNum;

    [HideInInspector]
        public float recoverTimeScaleTimer;

    [HideInInspector]
        public float hitChangeTimeScaleTime;

    [HideInInspector]
    public GameObject uiTips_SpecialInteractive;

    //[HideInInspector]
    public GameObject playerMenu;

    private Vector3 cForward;
    private Vector3 cRight;
    private Vector3 cUp;

    private Vector3 stepMoveDir;

    private const string weaponPrefabsPaths = "Prefabs/Weapons/";


    [HideInInspector]
    public bool BeAttacked;//**DISON.ver**用于怪物巡逻判断（若被攻击，则终止巡逻）
    [HideInInspector]
    public bool isTalking;//**DISON.ver**对话时不让玩家移动

    void Start()
    {
        if (isPlayer)
        {
            DontDestroyOnLoad(gameObject);

            thisAnimator = gameObject.transform.Find("ActorModel").GetComponent<Animator>();
        }

        nowThisTakeWeapon = weaponType.非武器;
        heal = maxHeal;
        lookAtTag = true;
        thisRigidbody = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;
        steping = false;
        skillArrNum = 0;
        Tools = new GameObject[10];
        isTakingTool = false;

        BeAttacked = false;//**DISON.ver**
        isTalking = false;//**DISON.ver**
    }

    void Update()
    {
        if (isAlive && isPlayer)//**DISON.ver**
        {
            CameraDirUpdata();

            if (!isTakingTool && !isTalking && !isOpeningPlayerMenu)
                {
                    Skill(skillArrNum);

                    SpecialInteractive();

                    Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                CallPlayerMenu();
            }

                RecoverTimeScale();

                changeWeaponModel();
            }

        if (heal <= 0 && isAlive)
        {
            GoDie();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && isPlayer && !isTalking)//**DISON.ver**
        {
            if (!isTakingTool && !isOpeningPlayerMenu)
            {
                Look();

                Move();
            }

        }  
    }

    private void CameraDirUpdata()
    {
        if (FollowCamera)
        {
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

    public void CallPlayerMenu()
    {
        if (!playerMenu)
        {
            playerMenu = Instantiate(Resources.Load("UIPrefabs/PlayerMenu") as GameObject,GameObject.Find("Canvas").transform);
            isOpeningPlayerMenu = true;
        }
    }

    //移动
    private void Move()
    {
        if (isAlive && isPlayer)//是否存在禁锢Buff
        {
            CameraDirUpdata();

            moveDirection = cForward * Input.GetAxis("Vertical") + cRight * Input.GetAxis("Horizontal");

            //是否憨憨玩家没有按方向键
            if (steping && moveDirection == Vector3.zero)
            {
                moveDirection = thisTimeForward;
            }

            if (steping && stepMoveDir == Vector3.zero)
            {
                stepMoveDir = moveDirection.normalized;
            }

            if (steping && !lookAtTag)
            {
                moveDirection = stepMoveDir;
            }

            if (!steping)
            {
                stepMoveDir = Vector3.zero;
            }

            if (slowDownBuff)
            {
                moveDirection = moveDirection * speed * slowDownBuff.GetComponent<Buff>().percent;
            }
            else 
            {
                moveDirection *= speed;
            }

            if (imprisonmentBuff)
            {
                moveDirection = Vector3.zero;
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
        if (isPlayer)//DISON.VER
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

        if (imprisonmentBuff && a)
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
            if (healOverBuff && healOverBuff.lifeTime == a.lifeTime && healOverBuff.percent == a.percent)
            {
                healOverBuff.lifeTimeTimer = 0;
                Destroy(a);
            }
            else
            {
                healOverBuff = a;
            }
        }
        else if (a.thisType == Buff.buffType.持续伤害)
        {
            if (damageOverTimeBuff && damageOverTimeBuff.lifeTime == a.lifeTime && damageOverTimeBuff.percent == a.percent)
            {
                damageOverTimeBuff.lifeTimeTimer = 0;
                Destroy(a);
            }
            else
            {
                damageOverTimeBuff = a;
            }
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
            //skillArrNum = (int)a.percent;
            if (skillArrNum == 0)
            {
                skillArrNum = 1;
            }
            else
            {
                skillArrNum = 0;
            }
        }
    }

    public void BuffAction(Buff.buffType i,Buff b)
    {
        if (i == Buff.buffType.回血 && healOverBuff == b)
        {
            TakeDamege(-maxHeal * healOverBuff.percent);
        }
        else if (i == Buff.buffType.持续伤害 && damageOverTimeBuff == b)
        {
            TakeDamege(maxHeal * damageOverTimeBuff.percent);
        }
    }

    //受伤函数响应
    public void TakeDamege(float i)
    {
        BeAttacked = true;
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
            }
            else
            {
                if (i>0 && !superArmorBuff)
                {
                    if (UsingSkill)
                    {
                        UsingSkill.SkillActionStop(true);
                    }

                    if (isPlayer)
                    {
                        thisAnimator.SetTrigger("Behit");
                    }
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
        if (isPlayer)
        {
            thisAnimator.SetTrigger("Dead");
        }

        if (gameObject.layer == LayerMask.NameToLayer("Enemy") && gameObject.tag != "BOSS")
        {
            Debug.Log(GameObject.FindGameObjectWithTag("Player"));

            if (GameObject.FindGameObjectWithTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Credit>().AddPlayerCredit(actorCredit);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Credit>().AddKillstreaksNum();
            }

            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            //播放怪物死亡动画、特效
            //2秒后销毁怪物
            StartCoroutine(Monster_Dead_Animation(2));//***DISON.ver***

            //Destroy(gameObject);
        }


    }

    IEnumerator Monster_Dead_Animation(float duration)//***DISON.ver***
    {
        yield return new WaitForSeconds(duration);

        if (GameObject.Find("BattleManager"))
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().Mon_Dead();//**DISON.ver**用于怪物刷新系统的击杀怪物数量计算
        }

        Destroy(gameObject);//***DISON.ver***
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

            if (!uiTips_SpecialInteractive)
            {
                uiTips_SpecialInteractive = Instantiate(Resources.Load("UIPrefabs/Image_SpecialInteractive") as GameObject, GameObject.Find("Canvas").transform);
                uiTips_SpecialInteractive.transform.SetSiblingIndex(0);
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

            for (int i = 0; i < Tools.Length; i++)
            {
                if (Tools[i] != null)
                {
                    break;
                }

                if (i == Tools.Length - 1 && uiTips_SpecialInteractive)
                {
                    Destroy(uiTips_SpecialInteractive);
                }
            }
        }
    }

}
