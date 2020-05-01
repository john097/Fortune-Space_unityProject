using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Movement;

public class mon_attack : Action
{
   
    private Actor mons_actor;
    private Skill mons_skill;
    private BattleManager mf_manager;
    public BehaviorTree behaviortree;

    public SharedBool attack_finished;//判断是否攻击完毕
    public SharedBool have_rotate_atk;
    public SharedBool is_bomber;
    public SharedBool is_mons3;


    public float Atk_C_A;//攻击硬值时间（攻击期间无法移动）
    public float Atk_C_A_2;//攻击硬值时间（攻击期间无法移动）
    public float Atk_C;//攻击硬值时间（攻击期间无法移动）
    public float Atk_cd;//攻击间隔
    float timer;//计时器

    public bool s;
    public Animator thisAnimator;

    public float Idle_Time;

    public MON_ROTATE_ATK C;

    public GameObject attack_warning;

    public override void OnAwake()
	{
      
        s = false;
        attack_finished.Value = true;

        mons_actor = GetComponent<Actor>();
        mons_skill = gameObject.transform.GetChild(0).GetComponent<Skill>();

        Atk_C_A_2=mons_actor.Skills_0[0].castActionTime + mons_actor.Skills_0[0].castTime;//获取攻击硬值时间
        Atk_C = mons_actor.Skills_0[0].castActionTime;
        Atk_C_A = mons_actor.Skills_0[0].castActionTime + mons_actor.Skills_0[0].castTime;//获取攻击硬值时间
        Atk_cd = mons_actor.Skills_0[0].coolDownTime + Atk_C_A;

        mf_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        behaviortree = GetComponent<BehaviorTree>();

        have_rotate_atk=(SharedBool)behaviortree.GetVariable("have_rotate_atk");
        is_bomber= (SharedBool)behaviortree.GetVariable("is_bomber");
        is_mons3= (SharedBool)behaviortree.GetVariable("is_mons3");

        if (!is_bomber.Value&&!is_mons3.Value)
        {
            thisAnimator = gameObject.transform.Find("m002-LM-1").GetComponent<Animator>();
        }
            



    }

    public override void OnStart()
    {
        Idle_Time = Random.Range(2, 5);
        Idle_Time += Atk_C_A;
    }


    public override TaskStatus OnUpdate()
	{


        if (mons_actor.isAlive && !mons_skill.coolDownFlag && attack_finished.Value)
        {
            if (is_bomber.Value|| is_mons3.Value)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                mons_actor.Skills_0[0].UseSkill();
                attack_warning.SetActive(true);
                attack_finished.Value = false;
            }
            else
            {
                mons_actor.Skills_0[0].UseSkill();
                attack_warning.SetActive(true);

                if (!is_bomber.Value && !is_mons3.Value)
                {
                    thisAnimator.SetInteger("ContolInt", 1);
                }
                
                
                attack_finished.Value = false;
            }




            StartCoroutine(Remove2(Atk_C_A_2));
            StartCoroutine(Remove(Idle_Time));

            
        }
        if (s == true)
        {
            return TaskStatus.Success;
        }


        return TaskStatus.Running;
    }

   

    IEnumerator Remove(float duration)
    {
        
        
        if (is_bomber.Value)
        {
            yield return new WaitForSeconds(duration);
            gameObject.GetComponent<Actor>().GoDie();
            attack_finished = true;
            attack_warning.SetActive(false);
            s = true;
        }
        else
        {
            yield return new WaitForSeconds(duration);
 
            C.lerp_tm = 0f;
            attack_finished = true;
            s = true;
        }
    }
    IEnumerator Remove2(float duration)
    {
        if (is_bomber.Value)
        {
            yield return new WaitForSeconds(duration);
            gameObject.GetComponent<Actor>().GoDie();
            attack_finished = true;
            attack_warning.SetActive(false);
            s = true;
        }
        else
        {
            yield return new WaitForSeconds(duration);
            attack_warning.SetActive(false);
            if (!is_bomber.Value && !is_mons3.Value)
            {
                thisAnimator.SetInteger("ContolInt", 0);
            }

        }
    }





}
