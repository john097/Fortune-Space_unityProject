using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mons_Speed_Change : MonoBehaviour
{
    private Actor mons_actor;
    private NavMeshAgent navMeshAgent;
    public bool Behit;

    // Start is called before the first frame update
    void Start()
    {
        mons_actor = GetComponent<Actor>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Behit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mons_actor.slowDownBuff)
        {
            navMeshAgent.velocity = navMeshAgent.desiredVelocity.normalized * 1;
        }
        if (Behit)
        {
            navMeshAgent.velocity = navMeshAgent.desiredVelocity.normalized * 2f;
        }
    }

    public void BEHIT()
    {
        Behit = true;
        StartCoroutine(BEHIT_REMOVE(0.3F));
    }

    IEnumerator BEHIT_REMOVE(float duration)
    {
        yield return new WaitForSeconds(duration);
        Behit = false;
    }
}
