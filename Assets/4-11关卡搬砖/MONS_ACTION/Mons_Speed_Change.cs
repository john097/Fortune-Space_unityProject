using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mons_Speed_Change : MonoBehaviour
{
    private Actor mons_actor;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        mons_actor = GetComponent<Actor>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mons_actor.slowDownBuff)
        {
            navMeshAgent.velocity = navMeshAgent.desiredVelocity.normalized * 1;
        }
    }
}
