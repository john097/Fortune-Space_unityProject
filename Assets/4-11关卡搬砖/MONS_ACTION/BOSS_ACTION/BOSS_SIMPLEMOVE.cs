using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Movement;
public class BOSS_SIMPLEMOVE : Action
{
    private Actor mons_actor;
    private NavMeshAgent navMeshAgent;
    Vector3 PLAYER;
    float player_distance;

    public float follow_timer;
    public BehaviorTree behaviortree;
    public SharedInt Action_Num;

    public SharedBool usingskill;
    public SharedBool Num_Borning;
    public SharedBool Lasering;
 
    public SharedBool lv2;
    public SharedBool dash;

    public float Laser_Aim_Timer;

    
    public float LaserCannon_Atk_C_A;//攻击硬值时间（攻击期间无法移动）
    public float LaserCannon_Atk_C;//获取前摇时间


    public Quaternion mon_rotation;//转身前角度
    public Quaternion lookat_rotation;//准备朝向的角度
    public float per_second_rotate;//转身速度（每秒转多少度）
    public float lerp_speed = 0.0f;//旋转角度越大，lerp变化速度应该越慢
    public float lerp_tm = 0.0f;//lerp的动态参数

    public Animator thisAnimator;

    public override void OnAwake()
    {
        mons_actor = GetComponent<Actor>();
        behaviortree = gameObject.GetComponent<BehaviorTree>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        LaserCannon_Atk_C_A = mons_actor.Skills_0[2].castActionTime + mons_actor.Skills_0[2].castTime;//获取攻击硬值时间
        LaserCannon_Atk_C = mons_actor.Skills_0[2].castActionTime;//获取前摇时间

        Action_Num.Value = -1;
        usingskill.Value = false;
        Num_Borning.Value = false;

        thisAnimator= gameObject.transform.Find("m002-shenshi").GetComponent<Animator>();

        base.OnAwake();
    }

    public override void OnStart()
	{

        
        
        

        usingskill = (SharedBool)behaviortree.GetVariable("USING_SKILL");
        lv2 = (SharedBool)behaviortree.GetVariable("LV2_SPEED_RUNNING");
        dash = (SharedBool)behaviortree.GetVariable("DASH_RUNNING");
        Lasering= (SharedBool)behaviortree.GetVariable("Laser_Using");

        
    }

	public override TaskStatus OnUpdate()
	{
        
        PLAYER = GameObject.Find("Actor").transform.position;//获取玩家位置
       
        follow_timer += Time.deltaTime;


        if (!mons_actor.isAlive)
        {
            return TaskStatus.Failure;
        }

        if (mons_actor.isAlive && follow_timer <= 5 && !usingskill.Value)//追击TIME未达到MAX时以LV1的速度追击玩家
        {
            navMeshAgent.enabled = true;


            thisAnimator.SetBool("Idle", false);
            thisAnimator.SetBool("Walk", true);

            navMeshAgent.SetDestination(PLAYER);


        }
        if (mons_actor.isAlive && follow_timer > 5 && !Num_Borning.Value)//追击TIME达到MAX时，生成随机行动数A
        {
            navMeshAgent.enabled = true;
           

            
            Action_Num.Value = Random.Range(0,3);//取随机数
            Num_Borning.Value = true;
        }

        if (Action_Num.Value == 0)//行动数A为0时以LV2的速度追击玩家
        {
            LV2_SPEED();
            thisAnimator.SetBool("Idle", false);
            thisAnimator.SetBool("Walk", true);
        }

        if (Action_Num.Value == 1)//行动数A为1时使用激光炮技能，开始蓄力
        {
            if (!usingskill.Value)
            {
                UseLaserCannon();
                thisAnimator.SetBool("Walk", false);
                thisAnimator.SetBool("Idle", true);
                usingskill.Value = true;
            }
            
            Laser_Aim_Timer += Time.deltaTime;

            if (Laser_Aim_Timer <= LaserCannon_Atk_C&&Lasering.Value)//蓄力期间瞄准玩家
            {
                mon_rotation = transform.rotation;
                transform.LookAt(new Vector3(PLAYER.x, transform.position.y, PLAYER.z));
                lookat_rotation = transform.rotation;
                float rotate_angle = Quaternion.Angle(mon_rotation, lookat_rotation);
                // 获得lerp速度
                lerp_speed = per_second_rotate / rotate_angle;
                lerp_tm = 0.0f;

                lerp_tm += Time.deltaTime * lerp_speed;
                transform.rotation = Quaternion.Lerp(mon_rotation, lookat_rotation, lerp_tm);
                if (lerp_tm >= 1)
                {
                    transform.rotation = lookat_rotation;

                }
            }
        }
        if (Action_Num.Value == 2)//行动数为2时开始冲锋，以LV3的速度追击玩家并在进入攻击范围后使用冲锋上挑攻击玩家
        {
            LV3_SPEED();
        }

        //实现2、3级速度在攻击后自动回恢复1级速度的效果



        return TaskStatus.Running;
	}
    public override void OnEnd()
    {
        navMeshAgent.enabled = false;
    }

    public void LV2_SPEED()
    {
        navMeshAgent.velocity = navMeshAgent.desiredVelocity.normalized * 12;
        navMeshAgent.SetDestination(PLAYER);
        lv2.Value = true;
        Debug.Log("NOW IS LV2_SPEED!");
        StartCoroutine(SpeedUpRemove());
    }

    public void LV3_SPEED()
    {
        if (!mons_actor.Skills_0[3].coolDownFlag)
        {
            navMeshAgent.velocity = navMeshAgent.desiredVelocity.normalized * 16;
            navMeshAgent.SetDestination(PLAYER);
            dash.Value = true;
            //添加冲锋粒子特效
            Debug.Log("NOW IS LV3_SPEED!");
            StartCoroutine(SpeedUpRemove2());
        }
        else
        {
            Action_Num.Value = 1;//若冲锋技能未冷却完毕，则转为使用激光炮技能
            Debug.Log("冲锋未冷却完毕，转为使用激光炮技能");
        }
      
    }

    public void UseLaserCannon()
    {
        if (!mons_actor.Skills_0[2].coolDownFlag)
        {
            mons_actor.Skills_0[2].UseSkill();
            navMeshAgent.enabled = false;
            Lasering.Value = true;
            StartCoroutine(LaserRemove());
        }
        else
        {
            Action_Num.Value = 0;//若激光炮技能未冷却完毕，则转为使用LV2的速度追击玩家
            Debug.Log("激光炮未冷却完毕，转为使用LV2速度追击");
        }
       
    }

    IEnumerator LaserRemove()
    {

        yield return new WaitForSeconds(LaserCannon_Atk_C_A);
        follow_timer = 0;
        Action_Num.Value = -1;
        usingskill.Value = false;
        Num_Borning.Value = false;
        Lasering.Value = false;
        Laser_Aim_Timer = 0;


    }

    IEnumerator SpeedUpRemove()
    {
        yield return new WaitForSeconds(4);
        
        lv2.Value = false;
        dash.Value = false;
        Num_Borning.Value = false;
        
        Action_Num.Value = -1;
        follow_timer = 0;
        navMeshAgent.velocity = navMeshAgent.desiredVelocity.normalized * 5f;
    }

    IEnumerator SpeedUpRemove2()
    {
        yield return new WaitForSeconds(6);

        lv2.Value = false;
        dash.Value = false;
        Num_Borning.Value = false;

        Action_Num.Value = -1;
        follow_timer = 0;
        navMeshAgent.velocity = navMeshAgent.desiredVelocity.normalized * 5f;
    }


    public void SpeedUpRemove_ATK_DIS()
    {
        Action_Num.Value = -1;
        follow_timer = 0;
        navMeshAgent.velocity = navMeshAgent.desiredVelocity.normalized * 5f;
        //follow_timer = 0;
    }

}