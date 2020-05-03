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

    
    public float LaserCannon_Atk_C_A;//����Ӳֵʱ�䣨�����ڼ��޷��ƶ���
    public float LaserCannon_Atk_C;//��ȡǰҡʱ��


    public Quaternion mon_rotation;//ת��ǰ�Ƕ�
    public Quaternion lookat_rotation;//׼������ĽǶ�
    public float per_second_rotate;//ת���ٶȣ�ÿ��ת���ٶȣ�
    public float lerp_speed = 0.0f;//��ת�Ƕ�Խ��lerp�仯�ٶ�Ӧ��Խ��
    public float lerp_tm = 0.0f;//lerp�Ķ�̬����

    public Animator thisAnimator;

    public override void OnAwake()
    {
        mons_actor = GetComponent<Actor>();
        behaviortree = gameObject.GetComponent<BehaviorTree>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        LaserCannon_Atk_C_A = mons_actor.Skills_0[2].castActionTime + mons_actor.Skills_0[2].castTime;//��ȡ����Ӳֵʱ��
        LaserCannon_Atk_C = mons_actor.Skills_0[2].castActionTime;//��ȡǰҡʱ��

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
        
        PLAYER = GameObject.Find("Actor").transform.position;//��ȡ���λ��
       
        follow_timer += Time.deltaTime;


        if (!mons_actor.isAlive)
        {
            return TaskStatus.Failure;
        }

        if (mons_actor.isAlive && follow_timer <= 5 && !usingskill.Value)//׷��TIMEδ�ﵽMAXʱ��LV1���ٶ�׷�����
        {
            navMeshAgent.enabled = true;


            thisAnimator.SetBool("Idle", false);
            thisAnimator.SetBool("Walk", true);

            navMeshAgent.SetDestination(PLAYER);


        }
        if (mons_actor.isAlive && follow_timer > 5 && !Num_Borning.Value)//׷��TIME�ﵽMAXʱ����������ж���A
        {
            navMeshAgent.enabled = true;
           

            
            Action_Num.Value = Random.Range(0,3);//ȡ�����
            Num_Borning.Value = true;
        }

        if (Action_Num.Value == 0)//�ж���AΪ0ʱ��LV2���ٶ�׷�����
        {
            LV2_SPEED();
            thisAnimator.SetBool("Idle", false);
            thisAnimator.SetBool("Walk", true);
        }

        if (Action_Num.Value == 1)//�ж���AΪ1ʱʹ�ü����ڼ��ܣ���ʼ����
        {
            if (!usingskill.Value)
            {
                UseLaserCannon();
                thisAnimator.SetBool("Walk", false);
                thisAnimator.SetBool("Idle", true);
                usingskill.Value = true;
            }
            
            Laser_Aim_Timer += Time.deltaTime;

            if (Laser_Aim_Timer <= LaserCannon_Atk_C&&Lasering.Value)//�����ڼ���׼���
            {
                mon_rotation = transform.rotation;
                transform.LookAt(new Vector3(PLAYER.x, transform.position.y, PLAYER.z));
                lookat_rotation = transform.rotation;
                float rotate_angle = Quaternion.Angle(mon_rotation, lookat_rotation);
                // ���lerp�ٶ�
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
        if (Action_Num.Value == 2)//�ж���Ϊ2ʱ��ʼ��棬��LV3���ٶ�׷����Ҳ��ڽ��빥����Χ��ʹ�ó�������������
        {
            LV3_SPEED();
        }

        //ʵ��2��3���ٶ��ڹ������Զ��ػָ�1���ٶȵ�Ч��



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
            //��ӳ��������Ч
            Debug.Log("NOW IS LV3_SPEED!");
            StartCoroutine(SpeedUpRemove2());
        }
        else
        {
            Action_Num.Value = 1;//����漼��δ��ȴ��ϣ���תΪʹ�ü����ڼ���
            Debug.Log("���δ��ȴ��ϣ�תΪʹ�ü����ڼ���");
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
            Action_Num.Value = 0;//�������ڼ���δ��ȴ��ϣ���תΪʹ��LV2���ٶ�׷�����
            Debug.Log("������δ��ȴ��ϣ�תΪʹ��LV2�ٶ�׷��");
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