using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections.Generic;
using UnityEngine.AI;
using Fungus;

public class Mons_Patrol : Action
{
    private Flowchart flowchart;
    private NavMeshAgent navMeshAgent;
    float timer;
    float random_x;
    float random_z;
    public float patrol_distance;//随机巡逻的距离
    public float patrol_cd;//随机巡逻的时间间隔

    public override void OnStart()
	{
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        if (patrol_cd ==0)
        {
            patrol_cd = 3f;
        }
        if (patrol_distance == 0)
        {
            patrol_distance = 6f;
        }
        
        
    }

	public override TaskStatus OnUpdate()
	{
        timer += Time.deltaTime;
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