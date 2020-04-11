using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections.Generic;
using UnityEngine.AI;

public class MONS_CANSEE : Conditional
{
    public SharedFloat fieldOfViewAngle;//��Ұ��Χ�Ƕ�
    public SharedFloat CanSeeDistance;//��Ұ��Χֱ������
    public SharedVector3 offset;
    public SharedTransform target;
    public LayerMask targetLayerMask;
    public mon_attack A;
    public float dis;

    private BattleManager MC_manager;

    private NavMeshAgent navMeshAgent;
    private Actor mons_actor;
    public float patrol_distance;//���Ѳ�ߵľ���
    public float patrol_cd;//���Ѳ�ߵ�ʱ����
    float random_x;
    float random_z;
    float timer;
   

    public override void OnStart()
    {
        target = GameObject.Find("Actor").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        mons_actor = GetComponent<Actor>();

        MC_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();

        base.OnStart();

    }

    public override TaskStatus OnUpdate()
	{
        
        dis = Vector3.Distance(transform.position ,target.Value.position);
        timer += Time.deltaTime;
        

        if (timer > patrol_cd)//���Ѳ��
        {
            
            random_x = Random.Range(transform.position.x - patrol_distance, transform.position.x + patrol_distance);
            random_z = Random.Range(transform.position.z - patrol_distance, transform.position.z + patrol_distance);
           
            navMeshAgent.SetDestination(new Vector3(random_x, transform.position.y,  random_z));
            timer = 0;
        }
        
        if (dis < CanSeeDistance.Value)
        {
            navMeshAgent.enabled = false;
            return TaskStatus.Success;
        }
        if (mons_actor.BeAttacked)
        {
            navMeshAgent.enabled = false;
            return TaskStatus.Success;
        }
        if (MC_manager.Protect_Room_Battle)//��Ϊ�����ݵ�ս����ֱ��׷���ݵ㣬����ҪѲ��
        {
            return TaskStatus.Success;
        }
        //if(dis> CanSeeDistance.Value)
        //{
        //    return TaskStatus.Failure;
        //}

        return TaskStatus.Running;
	}


    public override void OnDrawGizmos()
    {
        if (fieldOfViewAngle == null || CanSeeDistance == null)
        {
            return;
        }

        
        MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, CanSeeDistance.Value, false);
    }

}