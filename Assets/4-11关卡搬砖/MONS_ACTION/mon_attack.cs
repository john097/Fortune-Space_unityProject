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
    GameObject player;

    public bool patrol=false;
    public bool AA;
   

    public float x;
    public float z;
    
    public Quaternion mon_rotation;//转身前角度
    public Quaternion lookat_rotation;//准备朝向的角度
    public float per_second_rotate;//转身速度（每秒转多少度）
    public float lerp_speed = 0.0f;//旋转角度越大，lerp变化速度应该越慢
    public float lerp_tm = 0.0f;//lerp的动态参数

    public SharedTransform target;
    
    public SharedFloat CanSeeDistance;
    public float dis;
    public float i;
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
        AA = true;
        if (mf_manager.Protect_Room_Battle)
        {
            player = GameObject.Find("ProtectPoint");
            target = GameObject.Find("ProtectPoint").transform;
        }
        else
        {
            player = GameObject.Find("Actor");
            target = GameObject.Find("Actor").transform;
        }
        

        Idle_Time = Random.Range(3, 7);
        Idle_Time += Atk_C_A;
    }


    public override TaskStatus OnUpdate()
	{
        dis = Vector3.Distance(transform.position, target.Value.position);

        if (!is_bomber.Value )
        {
            if (patrol)
            {
                //transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, transform.position.y, z), Time.deltaTime);
                if (dis <CanSeeDistance.Value)
                {
                    i = -1;
                   
                }
                else
                {
                    i = 1;
                }

               
                    transform.Translate(Vector3.forward * Time.deltaTime * 0.5f * i);


                if (!is_mons3.Value)
                {
                    if (AA && i == -1)
                    {

                        thisAnimator.SetInteger("ContolInt", 3);

                        AA = false;
                    }
                    else if (AA && i == 1)
                    {
                        thisAnimator.SetInteger("ContolInt", 4);

                        AA = false;
                    }
                }
               
              
               
                

                mon_rotation = transform.rotation;
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                lookat_rotation = transform.rotation;
                float rotate_angle = Quaternion.Angle(mon_rotation, lookat_rotation);
                // 获得lerp速度
                lerp_speed = per_second_rotate / rotate_angle;
                lerp_tm = 0.0f;

                lerp_tm += Time.deltaTime * lerp_speed;
                transform.rotation = Quaternion.Lerp(mon_rotation, lookat_rotation, lerp_tm);

                


            }
        }
            

        if (mons_actor.isAlive && !mons_skill.coolDownFlag && attack_finished.Value)
        {
            if (is_bomber.Value)
            {
                patrol = false;
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                mons_actor.Skills_0[0].UseSkill();
                attack_warning.SetActive(true);
                gameObject.GetComponent<Actor>().GoDie();
                attack_finished.Value = false;

            }
            else if (is_mons3.Value)
            {
                patrol = false;
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                mons_actor.Skills_0[0].UseSkill();
                attack_warning.SetActive(true);
                attack_finished.Value = false;
            }
            else
            {
                patrol = false;
                mons_actor.Skills_0[0].UseSkill();
                attack_warning.SetActive(true);

                if (!is_bomber.Value && !is_mons3.Value)
                {
                    thisAnimator.SetInteger("ContolInt", 1);
                }

                Idle_Time += Atk_C_A;
                attack_finished.Value = false;
            }
            


            StartCoroutine(Remove3(Atk_C_A_2+1f));
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
            //attack_finished = true;
            attack_warning.SetActive(false);
            //s = true;
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
            //attack_finished = true;
            attack_warning.SetActive(false);
            //s = true;
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

    IEnumerator Remove3(float duration)
    {
        yield return new WaitForSeconds(duration);

        patrol = true;

    }



}
