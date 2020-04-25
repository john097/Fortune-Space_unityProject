using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;

public class mon_follow : Action
{
    //private MON_AGENT agent;
    private Actor mons_actor;
    private BattleManager mf_manager;
    private NavMeshAgent navMeshAgent;
    GameObject PLAYER;
    Vector3 ProtectPoint;
    float player_distance;

    public bool doing_sth;

    public override void OnStart()
	{
       
        mons_actor = GetComponent<Actor>();
        doing_sth = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        mf_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        PLAYER = GameObject.Find("Actor");
        
    }

	public override TaskStatus OnUpdate()
	{
        //gameObject.layer = LayerMask.NameToLayer("Enemy");

        if (!mons_actor.isAlive)
        {
            navMeshAgent.enabled = false;
            return TaskStatus.Success;
        }

        if (mons_actor.isAlive)
        {
            navMeshAgent.enabled = true;
            if (mf_manager.Protect_Room_Battle)
            {
                navMeshAgent.SetDestination(ProtectPoint);
            }
            else
            {
                navMeshAgent.SetDestination(PLAYER.transform.position);
            }

        }

        if (mf_manager.Protect_Room_Battle)
        {
            ProtectPoint = GameObject.Find("ProtectPoint").transform.position;
        }
       
  
            return TaskStatus.Running;
       
	}

    public override void OnEnd()
    {
        navMeshAgent.enabled = false;

    }


}