using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections.Generic;
using UnityEngine.AI;
using Fungus;

public class MONS_CANSEE : Conditional
{
    public SharedFloat fieldOfViewAngle;//视野范围角度
    public SharedFloat CanSeeDistance;//视野范围直径长度
    public SharedVector3 offset;
    public SharedTransform target;
    public LayerMask targetLayerMask;
    public mon_attack A;
    public float dis;

    private BattleManager MC_manager;
    private Flowchart flowchart;

    private NavMeshAgent navMeshAgent;
    private Actor mons_actor;
    public float patrol_distance;//随机巡逻的距离
    public float patrol_cd;//随机巡逻的时间间隔
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
        flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();

        if (PlayerPrefs.GetInt("Current_State") == 0)
        {
            mons_actor.maxHeal = 200;
            mons_actor.heal = 200;
        }


            base.OnStart();




    }

    public override TaskStatus OnUpdate()
	{
        
        dis = Vector3.Distance(transform.position ,target.Value.position);
        timer += Time.deltaTime;
        
        if(PlayerPrefs.GetInt("Current_State") != 0)
        {
            if (timer > patrol_cd)//随机巡逻
            {

                random_x = Random.Range(transform.position.x - patrol_distance, transform.position.x + patrol_distance);
                random_z = Random.Range(transform.position.z - patrol_distance, transform.position.z + patrol_distance);

                navMeshAgent.SetDestination(new Vector3(random_x, transform.position.y, random_z));
                timer = 0;
            }
        }
        

        if (PlayerPrefs.GetInt("Current_State")==0)
        {
            navMeshAgent.enabled = false;
            return TaskStatus.Running;
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
        if (MC_manager.Protect_Room_Battle)//若为保护据点战，则直接追击据点，不需要巡逻
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