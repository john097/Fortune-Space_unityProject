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

        

            base.OnStart();




    }

    public override TaskStatus OnUpdate()
	{
        if (!mons_actor.isAlive)
        {
            return TaskStatus.Success;
        }

        dis = Vector3.Distance(transform.position ,target.Value.position);
  
        if (PlayerPrefs.GetInt("Current_State")==0)
        {
            navMeshAgent.enabled = false;
            return TaskStatus.Running;
        }

        if (dis < CanSeeDistance.Value)
        {
            mons_actor.BeAttacked = false;
            return TaskStatus.Success;
        }

        if (mons_actor.BeAttacked)
        {

            return TaskStatus.Success;
        }

       
       
        if (MC_manager.Protect_Room_Battle)//若为保护据点战，则直接追击据点，不需要巡逻
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
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