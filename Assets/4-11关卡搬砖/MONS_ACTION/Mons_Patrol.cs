using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;


public class Mons_Patrol : Action
{

    private NavMeshAgent navMeshAgent;
    private Actor mons_actor;
    private BattleManager BM;
    float timer;
    float random_x;
    float random_z;
    public float patrol_distance;//随机巡逻的距离
    public float patrol_cd;//随机巡逻的时间间隔
    public float patrol_timer;
    public override void OnStart()
	{
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        mons_actor = GetComponent<Actor>();
        patrol_timer = 0f;

        if (patrol_cd ==0)
        {
            patrol_cd = 3f;
        }
        if (patrol_distance == 0)
        {
            patrol_distance = 6f;
        }
        if (GameObject.Find("BattleManager"))
        {
            BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }
        
    }

	public override TaskStatus OnUpdate()
	{
        timer += Time.deltaTime;
        patrol_timer += Time.deltaTime;

        if (patrol_timer >= 20f)
        {
            BM.MON_NUMS -= 1;
            Object.Destroy(gameObject);

        }

        if (timer > patrol_cd)//随机巡逻
        {
            
            random_x = Random.Range(transform.position.x - patrol_distance, transform.position.x + patrol_distance);
            random_z = Random.Range(transform.position.z - patrol_distance, transform.position.z + patrol_distance);

            navMeshAgent.SetDestination(new Vector3(random_x, transform.position.y, random_z));
            timer = 0;

        }

        return TaskStatus.Running;
    }
    
}