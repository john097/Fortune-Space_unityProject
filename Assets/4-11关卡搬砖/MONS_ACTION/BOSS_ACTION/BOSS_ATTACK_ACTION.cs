using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Movement;

public class BOSS_ATTACK_ACTION : Action
{
    private Actor mons_actor;



    public bool s;//����жϱ���

    public SharedBool attack_finished;//�ж��Ƿ񹥻����
    public float[] Atk_C_A;//����Ӳֵʱ�䣨�����ڼ��޷��ƶ���
    public float[] Atk_cd;//�������
    
    
    
    public BehaviorTree behaviortree;

    public SharedInt Action_Num;

    public SharedFloat Action_Time;//�������ܵĹ���CD
    public SharedFloat Action_Timer;//�������ܵĹ���CD��ʱ��

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
            Atk_C_A[i] = mons_actor.Skills_0[i].castActionTime + mons_actor.Skills_0[i].castTime;//��ȡ����Ӳֵʱ��
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

        if (mons_actor.isAlive && !usingskill_Attack.Value && dash_Attack.Value)//��boss����LV3���ٶȳ�����ң���ض�ʹ��һ�γ������
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

        if (mons_actor.isAlive && !usingskill_Attack.Value && !Num_Borning.Value)//������Ϊ�����
        {
            Action_Num.Value = Random.Range(0, 2);//�����Ϊ��
            Action_Time.Value = Random.Range(2f, 4f);//����ж�����CD
            Num_Borning.Value = true;
        }

        if (mons_actor.isAlive && !usingskill_Attack.Value && lv2_Attack.Value)//��boss����LV2���ٶȳ�����ң���ҽ��빥����Χ�����̽���һ�ι�����ƽA��ͻ�̣�
        {
            Action_Timer.Value = Action_Time.Value;

            lv2_Attack.Value = false;
        }

        

        if(mons_actor.isAlive && !usingskill_Attack.Value&& Action_Num.Value == 0 && Action_Timer.Value >= Action_Time.Value)//�����Ϊ0ʱʹ����ͨ����
        {
            NormalAttack();
           
            return TaskStatus.Running;
        }

        if (mons_actor.isAlive && !usingskill_Attack.Value && Action_Num.Value == 1 && Action_Timer.Value >= Action_Time.Value)//�����Ϊ1ʱʹ��ͻ��
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
        if (!mons_actor.Skills_0[1].coolDownFlag)//����ʱͻ�̻�δ��ȴ��ϣ�תΪʹ����ͨ����
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
            Debug.Log("ͻ��Ϊ��ȴ��ϣ�תΪ��ͨ������");
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