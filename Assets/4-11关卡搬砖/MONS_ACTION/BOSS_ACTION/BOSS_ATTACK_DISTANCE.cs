using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections.Generic;
using UnityEngine.AI;

public class BOSS_ATTACK_DISTANCE : Conditional
{
    public SharedFloat fieldOfAttackAngle;//¹¥»÷ÅÐ¶¨·¶Î§½Ç¶È
    public SharedFloat CanAttackDistance;//¹¥»÷ÅÐ¶¨·¶Î§Ö±¾¶³¤¶È

    public SharedBool UsingLaser;
    public SharedVector3 offset;
    public SharedTransform target;
    public BehaviorTree behaviortree;

    public float dis;
  
    float timer;
    public BOSS_SIMPLEMOVE A;

    public override void OnStart()
    {
      
        behaviortree = gameObject.GetComponent<BehaviorTree>();
        target = GameObject.FindGameObjectWithTag("ACTOR").transform;
        
        UsingLaser= (SharedBool)behaviortree.GetVariable("Laser_Using");
        base.OnStart();

    }

    public override TaskStatus OnUpdate()
	{
        dis = Vector3.Distance(transform.position, target.Value.position);

        if (dis <=CanAttackDistance.Value&&!UsingLaser.Value)
        {
            A.SpeedUpRemove_ATK_DIS();
            return TaskStatus.Success;
        }
      
        
        return TaskStatus.Failure;
	}

    public override void OnDrawGizmos()
    {
        if (fieldOfAttackAngle == null || CanAttackDistance == null)
        {
            return;
        }


        MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, fieldOfAttackAngle.Value, CanAttackDistance.Value, false);
    }
}