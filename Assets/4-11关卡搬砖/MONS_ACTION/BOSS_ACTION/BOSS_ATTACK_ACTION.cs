using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Movement;

public class BOSS_ATTACK_ACTION : Action
{
    private Actor mons_actor;



    public bool s;//打断判断变量

    public SharedBool attack_finished;//判断是否攻击完毕
    public float[] Atk_C_A;//攻击硬值时间（攻击期间无法移动）
    public float[] Atk_cd;//攻击间隔
    
    
    
    public BehaviorTree behaviortree;

    public SharedInt Action_Num;

    public SharedFloat Action_Time;//攻击技能的公共CD
    public SharedFloat Action_Timer;//攻击技能的公共CD计时器

    public SharedBool Num_Borning;
    public SharedBool lv2_Attack;
    public SharedBool dash_Attack;
    public SharedBool usingskill_Attack;
    public Animator thisAnimator;

    public override void OnAwake()
    {
        mons_actor = GetComponent<Actor>();
        behaviortree = GetComponent<BehaviorTree>();
        thisAnimator = gameObject.transform.Find("m002-shenshi").GetComponent<Animator>();

        for (int i = 0; i < 4; i++)
        {
            Atk_C_A[i] = mons_actor.Skills_0[i].castActionTime + mons_actor.Skills_0[i].castTime;//获取攻击硬值时间
            Atk_cd[i] = mons_actor.Skills_0[i].coolDownTime + Atk_C_A[i];
        }

        Num_Borning.Value = false;
        Action_Timer.Value = 4f;
        Action_Num.Value = -1;
        base.OnAwake();
    }
    public override void OnStart()
	{

        thisAnimator.SetBool("Idle", true);
        thisAnimator.SetBool("Walk", false);

        s = false;

        lv2_Attack = (SharedBool)behaviortree.GetVariable("LV2_SPEED_RUNNING"); 
        dash_Attack= (SharedBool)behaviortree.GetVariable("DASH_RUNNING");
        usingskill_Attack=(SharedBool)behaviortree.GetVariable("USING_SKILL");
        



    }

	public override TaskStatus OnUpdate()
	{
       
        Action_Timer.Value += Time.deltaTime;

        if (s)
        {
            
            return TaskStatus.Success;
        }

        if (mons_actor.isAlive && !usingskill_Attack.Value && dash_Attack.Value)//若boss是以LV3的速度冲向玩家，则必定使用一次冲锋上挑
        {
            mons_actor.Skills_0[3].UseSkill();
            dash_Attack.Value = false;
            usingskill_Attack.Value = true;

            thisAnimator.SetBool("Walk", false);
            thisAnimator.SetBool("Idle", false);
            thisAnimator.SetTrigger("TuCi");

            StartCoroutine(DashRemove());
            return TaskStatus.Running;
        }

        if (mons_actor.isAlive && !usingskill_Attack.Value && !Num_Borning.Value)//生成行为随机数
        {
            Action_Num.Value = Random.Range(0, 2);//随机行为数
            Action_Time.Value = Random.Range(2f, 4f);//随机行动公共CD
            Num_Borning.Value = true;
        }

        if (mons_actor.isAlive && !usingskill_Attack.Value && lv2_Attack.Value)//若boss是以LV2的速度冲向玩家，玩家进入攻击范围后立刻进行一次攻击（平A或突刺）
        {
            Action_Timer.Value = Action_Time.Value;

            lv2_Attack.Value = false;
        }

        

        if(mons_actor.isAlive && !usingskill_Attack.Value&& Action_Num.Value == 0 && Action_Timer.Value >= Action_Time.Value)//随机数为0时使用普通攻击
        {
            NormalAttack();
           
            return TaskStatus.Running;
        }

        if (mons_actor.isAlive && !usingskill_Attack.Value && Action_Num.Value == 1 && Action_Timer.Value >= Action_Time.Value)//随机数为1时使用突刺
        {
            Thrust();
           
            return TaskStatus.Running;
        }


        return TaskStatus.Success;
	}

    public void NormalAttack()
    {
        mons_actor.Skills_0[0].UseSkill();
        thisAnimator.SetBool("Walk", false);
        thisAnimator.SetBool("Idle", false);
        thisAnimator.SetTrigger("TuCi");
        usingskill_Attack.Value = true;
        StartCoroutine(NormalAttackRemove());
        Action_Timer.Value = 0;
    }

    public void Thrust()
    {
        if (!mons_actor.Skills_0[1].coolDownFlag)//若此时突刺还未冷却完毕，转为使用普通攻击
        {
            mons_actor.Skills_0[1].UseSkill();
            thisAnimator.SetBool("Walk", false);
            thisAnimator.SetBool("Idle", false);
            thisAnimator.SetTrigger("TuCi");
            usingskill_Attack.Value = true;
            StartCoroutine(ThrustRemove());
            Action_Timer.Value = 0;
        }
        else
        {
            NormalAttack();
            thisAnimator.SetBool("Walk", false);
            thisAnimator.SetBool("Idle", false);
            thisAnimator.SetTrigger("TuCi");
            Debug.Log("突刺为冷却完毕，转为普通攻击！");
        }
        
    }


    IEnumerator NormalAttackRemove()
    {
        yield return new WaitForSeconds(Atk_C_A[0]);

        usingskill_Attack.Value = false;
        Num_Borning.Value = false;
        s = true;

        thisAnimator.SetBool("Idle", true);

        Action_Num.Value = -1;
        
    }

    IEnumerator ThrustRemove()
    {
        yield return new WaitForSeconds(Atk_C_A[1]);

        usingskill_Attack.Value = false;
        Num_Borning.Value = false;
        s = true;

        thisAnimator.SetBool("Idle", true);

        Action_Num.Value = -1;
        
    }

    IEnumerator DashRemove()
    {
        yield return new WaitForSeconds(Atk_C_A[3]);

        usingskill_Attack.Value = false;
        Num_Borning.Value = false;
        s = true;

        thisAnimator.SetBool("Idle", true);

        Action_Num.Value = -1;
        Action_Timer.Value = 0;

    }

}