using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

using BehaviorDesigner.Runtime.Tasks.Movement;

public class mon_skill : Action
{
    public GameObject shield;
    public float timer;
    public float skill_time;
    public int random_cd;
    
    public bool timeborn;
    private Actor mons_actor;

    public override void OnAwake()
    {
        timeborn = false;
        skill_time = 5f;
        mons_actor = GetComponent<Actor>();
        base.OnAwake();
    }

  



    public override TaskStatus OnUpdate()
    {
        if (!mons_actor.isAlive)
        {
            shield.SetActive(false);
        }
        if (!timeborn)
        {
            random_cd = Random.Range(5, 6);
            
            timeborn = true;
        }

        if (timeborn&&mons_actor.isAlive)
        {
            timer += Time.deltaTime;
            if (timer >= random_cd)
            {
                shield.SetActive(true);
                StartCoroutine(Remove());
            }
        }
       


        return TaskStatus.Running;
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(5);
        shield.SetActive(false);
        timer = 0f;
        timeborn = false;
    }





}