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
    Vector3 PLAYER;
    Vector3 ProtectPoint;
    float player_distance;

    public bool doing_sth;

    public override void OnStart()
	{
        //agent = GetComponent<MON_AGENT>();
        mons_actor = GetComponent<Actor>();
        doing_sth = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        mf_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
    }

	public override TaskStatus OnUpdate()
	{
        if (mf_manager.Protect_Room_Battle)
        {
            ProtectPoint = GameObject.Find("ProtectPoint").transform.position;//获取据点的位置
        }
        PLAYER = GameObject.Find("Actor").transform.position;//获取玩家位置
        
        
        

        if (mons_actor.isAlive)
        {
            navMeshAgent.enabled = true;
            if (mf_manager.Protect_Room_Battle)
            {
                navMeshAgent.SetDestination(ProtectPoint);
            }
            else
            {
                navMeshAgent.SetDestination(PLAYER);
            }
                
            
        }
        


            return TaskStatus.Running;
       
	}

    public override void OnEnd()
    {
        navMeshAgent.enabled = false;

    }


}