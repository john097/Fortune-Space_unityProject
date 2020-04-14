using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MON_ROTATE_ATK : Action
{
    public Quaternion mon_rotation;//ת��ǰ�Ƕ�
    public Quaternion lookat_rotation;//׼������ĽǶ�
    public float per_second_rotate;//ת���ٶȣ�ÿ��ת���ٶȣ�
    public float lerp_speed = 0.0f;//��ת�Ƕ�Խ��lerp�仯�ٶ�Ӧ��Խ��
    public float lerp_tm = 0.0f;//lerp�Ķ�̬����

    GameObject player;
    GameObject ProtectPoint;

    private BattleManager MRA_manager;
    public mon_attack C;
    

    public BehaviorTree behaviortree;
    public SharedBool boss_usingskill;
    
    public SharedInt mons3_action;

    public override void OnStart()
    {
        player = GameObject.Find("Actor");
        behaviortree = gameObject.GetComponent<BehaviorTree>();
        MRA_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        
        

        if (gameObject.tag == "BOSS")
        {
            boss_usingskill= (SharedBool)behaviortree.GetVariable("USING_SKILL");
  
        }
        else
        {
            ProtectPoint = GameObject.Find("ProtectPoint");

                C.attack_finished.Value = true;
     
        }

    }

	public override TaskStatus OnUpdate()
	{

        if (gameObject.tag != "BOSS"&&!C.attack_finished.Value)
        {

            return TaskStatus.Success;
        }
     

        if (gameObject.tag == "BOSS" && boss_usingskill.Value)
        {
            return TaskStatus.Success;
        }

        if (MRA_manager.Protect_Room_Battle)//���ڱ����ݵ�ս��ת���ж�
        {
            mon_rotation = transform.rotation;
            transform.LookAt(new Vector3(ProtectPoint.transform.position.x, transform.position.y, ProtectPoint.transform.position.z));
            lookat_rotation = transform.rotation;
            float rotate_angle_pp = Quaternion.Angle(mon_rotation, lookat_rotation);
            // ���lerp�ٶ�
            lerp_speed = per_second_rotate / rotate_angle_pp;
            lerp_tm = 0.0f;

            lerp_tm += Time.deltaTime * lerp_speed;
            transform.rotation = Quaternion.Lerp(mon_rotation, lookat_rotation, lerp_tm);
            //return TaskStatus.Success;

        }
        else
        {
            mon_rotation = transform.rotation;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            lookat_rotation = transform.rotation;
            float rotate_angle = Quaternion.Angle(mon_rotation, lookat_rotation);
            // ���lerp�ٶ�
            lerp_speed = per_second_rotate / rotate_angle;
            lerp_tm = 0.0f;

            lerp_tm += Time.deltaTime * lerp_speed;
            transform.rotation = Quaternion.Lerp(mon_rotation, lookat_rotation, lerp_tm);
        }
        
        

        if (lerp_tm >= 1)
        {
            transform.rotation = lookat_rotation;
            return TaskStatus.Success;
            // ��ʱ, ת�����, �Ѿ�����Ŀ������
        }
        //if(lerp_tm < 1)
        //{
        //    return TaskStatus.Running;
        //}

        else
        {
            return TaskStatus.Success;
        }
            
	}
}