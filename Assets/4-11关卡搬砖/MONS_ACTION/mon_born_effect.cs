using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class mon_born_effect : Action
{
    public SharedFloat timer;
    private Collider mon_collider;
    private BattleManager MC_manager;
    private Actor mons_actor;

    public override void OnAwake()
    {
        mon_collider = gameObject.GetComponent<Collider>();
        mon_collider.enabled = false;
        mons_actor = GetComponent<Actor>();
        if (GameObject.Find("BattleManager"))
        {
            MC_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }
        if (PlayerPrefs.GetInt("Current_State") == 0)
        {
            mons_actor.maxHeal *= 1;
            mons_actor.heal = mons_actor.maxHeal;
        }
        else
        {
            switch (MC_manager.Game_Level)//怪物难度动态增加
            {
                case 0:
                    break;
                case 1:
                    mons_actor.maxHeal += 500;
                    mons_actor.heal = mons_actor.maxHeal;
                    mons_actor.attack += 5;
                    break;
                case 2:
                    mons_actor.maxHeal += 1000;
                    mons_actor.heal = mons_actor.maxHeal;
                    mons_actor.attack += 10;
                    break;
                case 3:
                    mons_actor.maxHeal += 1500;
                    mons_actor.heal = mons_actor.maxHeal;
                    mons_actor.attack += 15;
                    break;
                case 4:
                    mons_actor.maxHeal += 2000;
                    mons_actor.heal = mons_actor.maxHeal;
                    mons_actor.attack += 20;
                    break;
            }
        }
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