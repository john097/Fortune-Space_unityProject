using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MON_AGENT : MonoBehaviour
{
    NavMeshAgent follow_path;
    CapsuleCollider capsuleCollider;


    public int MaxHp=100;
    public int CurrentHp;
    public bool isDead;

    


    // Start is called before the first frame update
    void Start()
    {
        follow_path = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        CurrentHp = MaxHp;
        

    }

    // Update is called once per frame
    void Update()
    {
        
     
    }

    public NavMeshAgent GetNavAgent()
    {
        return this.follow_path;
    }

    public void StartFollow(Vector3 target)//自动寻路（怪物跟随玩家）
    {
        follow_path.enabled = true;
        follow_path.SetDestination(target);
    }

    public void StopFollow()//停止跟踪
    {
        follow_path.enabled = false;
    }



    public void TakeDamage(int amount, Vector3 hitPoint)//被攻击时计算受到的伤害
    {
        if (isDead)
        {
            return;
        }

        CurrentHp -= amount;

        if (CurrentHp <= 0)
        {
            Death();//调用死亡函数
        }

    }

   public void Death()//怪物死亡，删除对象
    {
        GameObject.Find("BattleManager").GetComponent<BattleManager>().Mon_Dead();
        
        isDead = true;
        capsuleCollider.isTrigger = true;
        Destroy(gameObject, 0.1f);
    }

    }
