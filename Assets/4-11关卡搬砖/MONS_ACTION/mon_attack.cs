using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Movement;

public class mon_attack : Action
{
   
    private Actor mons_actor;
    
    private BattleManager mf_manager;
    public BehaviorTree behaviortree;

    public SharedBool attack_finished;//判断是否攻击完毕
    public SharedBool have_rotate_atk;
    public SharedBool is_bomber;
    
    public float Atk_C_A;//攻击硬值时间（攻击期间无法移动）
    public float Atk_C;//攻击硬值时间（攻击期间无法移动）
    public float Atk_cd;//攻击间隔
    float timer;//计时器

    public bool s;



    
    public MON_ROTATE_ATK C;

    public override void OnStart()
	{
      
        s = false;
        attack_finished.Value = true;

        mons_actor = GetComponent<Actor>();

        Atk_C = mons_actor.Skills_0[0].castActionTime;
        Atk_C_A = mons_actor.Skills_0[0].castActionTime + mons_actor.Skills_0[0].castTime;//获取攻击硬值时间
        Atk_cd = mons_actor.Skills_0[0].coolDownTime+Atk_C_A;

        mf_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        behaviortree = GetComponent<BehaviorTree>();
        have_rotate_atk=(SharedBool)behaviortree.GetVariable("have_rotate_atk");
        is_bomber= (SharedBool)behaviortree.GetVariable("is_bomber");




    }

    

	public override TaskStatus OnUpdate()
	{


        if (mons_actor.isAlive && attack_finished.Value == true)
        {
            
            mons_actor.Skills_0[0].UseSkill();
            attack_finished.Value = false;

            if (is_bomber.Value)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            
            StartCoroutine(Remove());
            return TaskStatus.Running;
        }
        if (s == true)
        {
            return TaskStatus.Success;
        }


        return TaskStatus.Success;
	}

   

    IEnumerator Remove()
    {
        
        
        if (is_bomber.Value)
        {
            yield return new WaitForSeconds(Atk_C);
            gameObject.GetComponent<Actor>().GoDie();
            attack_finished = true;
            s = true;
        }
        else
        {
            yield return new WaitForSeconds(Atk_C_A);   
            C.lerp_tm = 0f;
            attack_finished = true;
            s = true;
        }
    }
        
        
        

}
