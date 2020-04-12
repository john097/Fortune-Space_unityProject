using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class mon_skill : Action
{
    private Actor mons_actor;

    GameObject player;

    public bool isdead;//判断是否死亡
    public SharedBool skill_finished;//判断是否攻击完毕
    
    public float Atk_C_A;//攻击硬值时间（攻击期间无法移动）
    public float Atk_cd;//攻击间隔
    float timer;//计时器

    public bool s;


   
    public MON_ROTATE_ATK C;

    public override void OnStart()
	{
        player = GameObject.Find("Actor");

        skill_finished = true;
        s = false;
        mons_actor = GetComponent<Actor>();
        Atk_C_A = mons_actor.Skills_0[1].castActionTime + mons_actor.Skills_0[1].castTime;//获取攻击硬值时间
        Atk_cd = mons_actor.Skills_0[1].coolDownTime + Atk_C_A;
        
    }

	public override TaskStatus OnUpdate()
	{

        isdead = mons_actor.isDead;


        if (isdead == false && skill_finished.Value == false)
        {
            mons_actor.Skills_0[1].UseSkill();
            s = false;
            StartCoroutine(Remove());
        }
        if (s == true)
        {
            return TaskStatus.Success;
        }

        
        return TaskStatus.Running;

        
	}
    IEnumerator Remove()
    {

        yield return new WaitForSeconds(Atk_C_A);
        C.lerp_tm = 0f;
        skill_finished = true;
        s = true;
    }




}