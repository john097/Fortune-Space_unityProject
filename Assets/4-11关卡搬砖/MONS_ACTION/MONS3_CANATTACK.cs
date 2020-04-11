using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections.Generic;
using UnityEngine.AI;

public class MONS3_CANATTACK : Conditional
{
    public SharedFloat fieldOfViewAngle;//视野范围角度
    public SharedFloat CanSeeDistance;//视野范围直径长度
    public SharedVector3 offset;
    public SharedTransform target;
    public LayerMask targetLayerMask;
    public mon_attack A;
    private Actor mons_actor;
    public float dis;
    float timer;

    public override void OnAwake()
    {
        target = GameObject.Find("Actor").transform;
        mons_actor = GetComponent<Actor>();
        base.OnAwake();
    }

    public override TaskStatus OnUpdate()
	{
        dis = Vector3.Distance(transform.position, target.Value.position);
        timer += Time.deltaTime;

        if (dis < CanSeeDistance.Value)
        {
            //A.attack_finished.Value = false;
            return TaskStatus.Success;
        }

        if (!A.attack_finished.Value)
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