using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class Skill : MonoBehaviour
{
    [Tooltip("技能名")]
        public string skillName;

    [Tooltip("技能基础描述")]
        public string skillExplain;

    [Tooltip("技能伤害机制描述")]
        public string skillDamageExplain;

    [Tooltip("技能易伤机制描述")]
        public string skillVulnerabilityExplain;

    [Tooltip("技能施力机制描述")]
        public string skillRepelExplain;

    [Tooltip("技能特殊机制描述")]
        public string skillSpecialExplain;

    [Tooltip("技能图标")]
        public Sprite skillIcon;

    [Tooltip("价格")]
        public int credit;

    [Tooltip("武器动画类型(只有武器需要配这个)")]
        public Actor.weaponType thisWeaponType;

    [Tooltip("武器模型编号")]
        public int weaponModelNum;

    [Tooltip("技能动画")]
        public int thisSkillAnimState;

    [Tooltip("技能动画速率")]
        public float thisSkillAnimSpeed;

    [Tooltip("调整的动画速率编号")]
        public int thisSkillSpeedNum;

    [Tooltip("动画信号几秒后恢复默认值")]
        public float thisSkillAnimTime;

    [Tooltip("角色当前是否处于前摇阶段的Tag")]
        [HideInInspector]
            public bool castActionTime_Tag;//前摇阶段Tag

    [Tooltip("此Tag置为True时，计时器将停止计时")]
        [HideInInspector]
            public bool Main_Tag;//打断所有逻辑的Tag

    [Tooltip("前摇时间")]
        public float castActionTime;//前摇时间

    [Tooltip("前摇+后摇时间")]
        public float castTime;//前摇+后摇时间

    [Tooltip("后摇时间无法被打断(优先级低于必须打断)")]
    [HideInInspector]
        public bool cantCancelAfterCastAction;

    [Tooltip("后摇时间内多久无法被打断")]
        public float cantCancelACATime;

    [Tooltip("是否前后摇时间内无法转向")]
        public bool cantTurnInCastTime;

    [Tooltip("第几秒开始无法转向")]
        public float cantTurnStartTime;

    [Tooltip("不可转向时间")]
        public float cantTurnTime;

    [Tooltip("使用次数上限")]
        public int ammoNumLimit;

    [Tooltip("剩余使用次数")]
        public int ammoNum;

    [Tooltip("使用次数装填时间")]
        public float reloadTime;

    [Tooltip("使用次数再装填按键")]
        public KeyCode reloadKey;
    
    [Tooltip("冷却时间")]
        public float coolDownTime;//冷却时间

    [Tooltip("无条件打断能被打断的技能(优先级高于后摇禁止打断，低于无法被打断)")]
        public bool mustCancel;   

    [Tooltip("无法被打断")]
        public bool cantCancel;//最高优先级

    [Tooltip("是否是位移技能")]
        public bool isStepSkill;

    [Tooltip("位移技能位移速度")]
        public float stepSpeed;

    [Tooltip("技能释放时震动")]
        public CamShake.ShakeIntensity skillShake;

    [Tooltip("技能对应发射的子弹预支物")]
        public GameObject[] bulletPrefabs;//技能发射子弹

    [Tooltip("子弹发射位置的偏移量")]
        public Vector3[] bulletOffsets;

    [Tooltip("子弹发射口旋转值")]
        public Vector3[] bulletRotationOffsets;

    [Tooltip("攻击预警预支物")]
        public GameObject[] attackWarningPrefabs;

    [Tooltip("攻击预警出现位置偏移量")]
        public Vector3[] attackWarningOffsets;

    [Tooltip("攻击预警旋转值")]
        public Vector3[] attackWarningRotationOffsets;

    [Tooltip("攻击预警几秒后消失")]
        public float attackWarningDestoryTime;    

    [Tooltip("技能变体")]
        public Skill skillVariant;//技能变体

    [Tooltip("Combo触发间隔时间")]
        public float comboTime;

    [Tooltip("当前Combo段数")]
        public int comboNum;

    [Tooltip("Combo技能")]
        public Skill[] comboSkills;

    [Tooltip("技能前摇时给自身附加的Buff")]
        public GameObject[] buffPrefabsInCastActionTime;

    [Tooltip("技能释放时给自身附加的Buff")]
        public GameObject[] buffPrefabs;//Buff预支体

    [Tooltip("技能释放完毕后是否触发技能变体(注意与子弹触发事件可能存在冲突)")]
        public bool skillVariantEvent;

    private float timer;//计时用的变量
    [HideInInspector]
        public  float coolDownTimer;//冷却计时用的变量
    private float reloadTimer;//换弹计时用的变量
    private float comboTimer;//计算Combo的变量
    private float thisSkillAnimTimer;

    private bool useSkillOnce;//卡Update用的flag
    [HideInInspector]
        public bool coolDownFlag;//字面意思
    private bool reloadFlag;//字面意思

    [HideInInspector]
        public Actor actor;//角色对象
    private Skill thisSkillClass;//当前技能类

    private AimIconFollowMouse AimIcon;

    // 初始化
    void Start()
    {
        actor = this.gameObject.transform.root.gameObject.GetComponent<Actor>();
        castActionTime_Tag = false;
        Main_Tag = true;
        timer = 0;
        coolDownTimer = coolDownTime;
        cantCancelAfterCastAction = true;
        useSkillOnce = true;
        coolDownFlag = true;
        reloadFlag = false;
        thisSkillClass = this.gameObject.GetComponent<Skill>();
        comboNum = 0;
        thisSkillAnimTimer = 0;

        if (GameObject.Find("Aim"))
        {
            AimIcon = GameObject.Find("Aim").GetComponent<AimIconFollowMouse>();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (GameObject.Find("Aim"))
        {
            AimIcon = GameObject.Find("Aim").GetComponent<AimIconFollowMouse>();
        }
    }

    // 每帧检测需要响应的事件
    void Update()
    {
        if (actor)
        {
            TimerFunc();

            CoolDownFunc();

            ListenReloadKey();

            ReloadFunc();

            ComboTimeFunc();
        }
    }

    //外部调用技能逻辑的接口
    public bool UseSkill()
    {
        //Combo系统触发
        if (!reloadFlag)
        {
            if (comboSkills.Length == 0)
            {
                Debug.Log("无Combo");
                return UseThisSkill();
            }
            else if (comboNum == 0)
            {
                Debug.Log("第一段Combo_0");
                if (UseThisSkill())
                {
                    AddComboNum();
                    return true;
                }

                return false;
            }
            else if (comboNum <= comboSkills.Length)
            {
                if (!coolDownFlag)
                {
                    if (comboSkills[comboNum - 1].UseThisSkill())
                    {
                        Debug.Log("第" + comboNum + "段");
                        AddComboNum();
                        return true;
                    }
                    else
                    {
                        Debug.Log("卡连段");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.Log("第一段Combo_1");
                AddComboNum();
                return UseThisSkill();
            }
        }
        else
        {
            Debug.Log("正在换弹，无法开火");
            return false;
        }
    }

    private bool UseThisSkill()
    {
        Debug.Log("当前技能为:" + thisSkillClass);
        if (!coolDownFlag)
        {
            if (!actor.UsingSkill)
            {
                if (ammoNumLimit == 0)
                {
                    Main_Tag = false;
                    actor.UsingSkill = thisSkillClass;
                    return true;
                }
                else if (ammoNum > 0)
                {
                    Main_Tag = false;
                    actor.UsingSkill = thisSkillClass;
                    return true;
                }
                else
                {
                    Debug.Log("弹药不足");
                    return false;
                }
            }
            else if (actor.UsingSkill.cantCancel)
            {
                Debug.Log("无法被打断!");
                return false;
            }
            else if (mustCancel)
            {
                if (ammoNumLimit == 0)
                {
                    actor.UsingSkill.SkillActionStop(true);
                    actor.UsingSkill = thisSkillClass;
                    Main_Tag = false;
                    Debug.Log("强制打断成功!");
                    return true;
                }
                else if (ammoNum > 0)
                {
                    actor.UsingSkill.SkillActionStop(true);
                    actor.UsingSkill = thisSkillClass;
                    Main_Tag = false;
                    Debug.Log("强制打断成功!");
                    return true;
                }
                else
                {
                    Debug.Log("弹药不足");
                    return false;
                }
            }
            else if (actor.UsingSkill.cantCancelAfterCastAction)
            {
                Debug.Log("技能后摇打断失败!");
                return false;
            }
            else if (actor.UsingSkill.castActionTime_Tag)
            {
                Debug.Log("技能前摇打断失败!");
                return false;
            }
            else
            {
                if (ammoNumLimit == 0)
                {
                    actor.UsingSkill.SkillActionStop(false);
                    actor.UsingSkill = thisSkillClass;
                    Main_Tag = false;
                    Debug.Log("打断成功!");
                    return true;
                }
                else if (ammoNum > 0)
                {
                    actor.UsingSkill.SkillActionStop(false);
                    actor.UsingSkill = thisSkillClass;
                    Main_Tag = false;
                    Debug.Log("打断成功!");
                    return true;
                }
                else
                {
                    Debug.Log("弹药不足");
                    return false;
                }
            }
        }
        else
        {
            Debug.Log(skillName + "  技能冷却中");
            return false;
        }
    }

    private void AddComboNum()
    {
        if (comboNum< comboSkills.Length)
        {
            comboNum += 1;
        }
        else
        {
            comboNum = 0;
        }

        //重置Combo计时器
        comboTimer = 0;
    }

    private void ComboTimeFunc()
    {
        if (comboSkills.Length != 0)
        {
            if (comboTimer >= comboTime)
            {
                comboNum = 0;
            }
            else
            {
                comboTimer += Time.deltaTime;
            }
        }
    }

    //计时器函数
    private void TimerFunc()
    {
        if (!Main_Tag)
        {
            //需补充角色禁止移动逻辑

            //技能开始CD计时
            if (timer==0)
            {
                coolDownFlag = true;
                coolDownTimer = 0;
                CastActionFunc();

                actor.SetLookAtTag(true);

                if(buffPrefabsInCastActionTime.Length != 0)
                {
                    for (int i = 0; i < buffPrefabsInCastActionTime.Length; i++)
                    {
                        if (buffPrefabsInCastActionTime[i])
                        {
                            GameObject buff = Instantiate(buffPrefabsInCastActionTime[i]);
                            buff.GetComponent<Buff>().SetTarget(actor);
                        }
                    }
                }

            }

            if (thisSkillAnimTimer >= thisSkillAnimTime)
            {
                actor.StopAnim();
            }
            else
            {
                thisSkillAnimTimer += Time.deltaTime;
            }

            //开始无法转向
            if (timer >= cantTurnStartTime)
            {
                //if (cantTurnInCastTime)
                //{
                //    actor.SetLookAtTag(false);
                //}
                //else
                //{
                //    actor.SetLookAtTag(true);
                //}
                actor.SetLookAtTag(false);
            }

            //结束计时
            if (timer <= castTime)
            {
                timer += Time.deltaTime;
            }

            if (timer >= cantCancelACATime)
            {
                cantCancelAfterCastAction = false;
            }

            if (timer >= cantTurnTime)
            {
                //if (cantTurnInCastTime)
                //{
                //    actor.SetLookAtTag(true);
                //}
                actor.SetLookAtTag(true);
            }

            if (timer >= attackWarningDestoryTime)
            {
                if (actor.aWarning)
                {
                    Destroy(actor.aWarning);
                }
            }

            //前摇时间结束，释放技能
            if (timer >= castActionTime)
            {
                SkillAction();
            }

            //后摇时间结束，计时器终止，启用打断Tag
            if (timer>=castTime)
            {
                SkillActionStop(false);
            }

            
        }
    }

    //冷却时间函数
    private void CoolDownFunc()
    {
        if (coolDownFlag)
        {
            if (coolDownTimer >= coolDownTime)
            {
                //coolDownTimer = 0;
                coolDownFlag = false;
                useSkillOnce = true;
                //Debug.Log(skillName + "  技能冷却完毕");
            }
            else
            {
                if (actor.coolDownBuff)
                {
                    coolDownTimer += actor.coolDownBuff.GetComponent<Buff>().percent * Time.deltaTime;
                }
                else
                {
                    coolDownTimer += Time.deltaTime;
                }
            }
        }
    }

    //换弹监听事件
    public void ListenReloadKey()
    {
        if (reloadKey != KeyCode.None)
        {
            if (Input.GetKeyDown(reloadKey))
            {
                if (!reloadFlag && ammoNumLimit != 0 && ammoNum < ammoNumLimit && !actor.UsingSkill && actor.skillArrNum == 0)
                {
                    reloadFlag = true;
                    actor.StartAnim(-1,3f/reloadTime,0);
                    actor.PlayReloadAudio(thisWeaponType);
                }
            }
        }
    }

    //换弹函数
    private void ReloadFunc()
    {
        if (reloadFlag)
        {
            if (reloadTimer >= reloadTime)
            {
                reloadTimer = 0;
                reloadFlag = false;
                ammoNum = ammoNumLimit;

                if (AimIcon)
                {
                    AimIcon.ReloadIcon(0);
                }
            }
            else
            {
                if (AimIcon)
                {
                    AimIcon.ReloadIcon(1);
                }

                reloadTimer += Time.deltaTime;
            }
        }
        else
        {
            if (thisWeaponType == Actor.weaponType.手枪 || thisWeaponType == Actor.weaponType.冲锋枪 || thisWeaponType == Actor.weaponType.霰弹枪 || thisWeaponType == Actor.weaponType.狙击枪)
            {
                if (AimIcon && ammoNumLimit != 0 && ammoNum == 0 && actor.skillArrNum == 0)
                {
                    AimIcon.NoAmmoIcon(1);
                }
                else if (AimIcon)
                {
                    AimIcon.NoAmmoIcon(0);
                }
            }
        }
    }

    //技能前摇事件
    private void CastActionFunc()
    {
        castActionTime_Tag = true;

        //播放技能动画
        actor.StartAnim(thisSkillAnimState,thisSkillAnimSpeed, thisSkillSpeedNum);

        if (isStepSkill)
        {
            actor.SetSteping(isStepSkill);
        }
        

        //克隆攻击预警
        if (actor.aWarning)
        {
            Destroy(actor.aWarning);
        }

        if (attackWarningPrefabs.Length != 0)
        {
            for (int i = 0; i < attackWarningPrefabs.Length; i++)
            {
                if (attackWarningPrefabs[i])
                {
                    GameObject aW = Instantiate(attackWarningPrefabs[i], actor.transform.position, actor.transform.rotation);
                    aW.transform.Rotate(attackWarningRotationOffsets[i], Space.Self);
                    aW.transform.Translate(attackWarningOffsets[i]);
                    actor.aWarning = aW;
                    aW.transform.parent = actor.transform;
                }
            }
        }

    }

    //技能前摇结束事件
    private void CastActionEndFunc()
    {
        castActionTime_Tag = false;
    }

    //技能释放函数
    private void SkillAction()
    {
        if (useSkillOnce)
        {
            //Debug.Log("调用资源（音效、特效），链接角色类传递动画信号");

            CastActionEndFunc();

            //准星扩散
            AimIconEvent();

            //使用次数减一
            if (ammoNumLimit != 0)
            {
                ammoNum -= 1;
            }

            //发射子弹
            if (bulletPrefabs.Length != 0)
            {
                for (int i = 0; i < bulletPrefabs.Length; i++)
                {
                    if (bulletPrefabs[i])
                    {
                        GameObject bullet = Instantiate(bulletPrefabs[i], actor.transform.position, actor.transform.rotation);
                        bullet.transform.Rotate(bulletRotationOffsets[i], Space.Self);
                        bullet.transform.Translate(bulletOffsets[i]);
                        bullet.GetComponent<Bullet>().actor = actor;
                        bullet.GetComponent<Bullet>().skillParent = thisSkillClass;
                    }
                }
            }

            //对自身施加Buff
            if (buffPrefabs.Length != 0)
            {
                for(int i = 0;i < buffPrefabs.Length; i++)
                {
                    if (buffPrefabs[i])
                    {
                        GameObject buff = Instantiate(buffPrefabs[i]);
                        buff.GetComponent<Buff>().SetTarget(actor);
                    }
                }
            }

            //判断是否为闪避技能
            if (isStepSkill)
            {
                actor.speed += stepSpeed;
                actor.SetSteping(true);
            }

            //模拟后坐力摄像机震动
            if (skillShake != CamShake.ShakeIntensity.无 && actor.isPlayer && GameObject.Find("CM vcam1"))
            {
                Vector3 c = Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(transform.position + transform.forward);

                actor.FollowCamera.GetComponent<CamShake>().CameraShake(c, skillShake);
            }

            //技能第一次释放之后触发变体打断原技能
            if (skillVariantEvent)
            {
                Debug.Log("释放变体");
                if (ammoNumLimit != 0)
                {
                    ammoNum -= 1;
                    UseSkillVariant();
                }
                else if (ammoNumLimit == 0)
                {
                    if (gameObject.transform.parent.gameObject.GetComponent<Skill>())
                    {
                        if (gameObject.transform.parent.gameObject.GetComponent<Skill>().ammoNumLimit != 0)
                        {
                            Skill pS = gameObject.transform.parent.gameObject.GetComponent<Skill>();

                            if (pS.ammoNum > 0)
                            {
                                UseSkillVariant();
                                pS.ammoNum -= 1;
                            }
                        }
                        else
                        {
                            UseSkillVariant();
                        }
                    }
                    else
                    {
                        UseSkillVariant();
                    }
                }
                //else if (gameObject.transform.parent.gameObject.GetComponent<Skill>().ammoNumLimit != 0)
                //{
                //    Skill pS = gameObject.transform.parent.gameObject.GetComponent<Skill>();
                    
                //    if (pS.ammoNum > 0)
                //    {
                //        UseSkillVariant();
                //        pS.ammoNum -= 1;
                //    }
                //}
                
            }

            //阻止程序逻辑执行二次释放
            useSkillOnce = false;

            //Debug.Log(skillName + "  释放！");
        }
    }

    public void SkillActionStop(bool a)
    {
        if (a && comboNum != 0)
        {
            comboNum = 0;
        }

        if (isStepSkill)
        {
            actor.speed -= stepSpeed;
            actor.SetSteping(false);
        }

        cantCancelAfterCastAction = true;

        thisSkillAnimTimer = 0;
        actor.StopAnim();

        Main_Tag = true;

        timer = 0;
        actor.UsingSkill = null;
    }

    //技能变体释放
    public void UseSkillVariant()
    {
        skillVariant.UseSkill();
    }

    public void AimIconEvent()
    {
        if (AimIcon && actor.isPlayer)
        {
            AimIcon.ShootingIconAnim(0.4f, Color.red);
        }
    }
    
}