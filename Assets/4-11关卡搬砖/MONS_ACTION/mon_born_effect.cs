using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class mon_born_effect : Action
{
    public SharedFloat timer;
    private Collider mon_collider;

    public override void OnAwake()
    {
        mon_collider = gameObject.GetComponent<Collider>();
        mon_collider.enabled = false;
        base.OnAwake();
    }

    public override void OnStart()
	{
       
	}

	public override TaskStatus OnUpdate()
	{
        timer.Value += Time.deltaTime;

        if (timer.Value < 1f)
        {
            //播放出生特效
            return TaskStatus.Running;
        }
        else
        {
            mon_collider.enabled = true;
        }
		return TaskStatus.Success;
	}
}