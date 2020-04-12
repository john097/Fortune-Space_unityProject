using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [HideInInspector]
        public Actor target;

    [Tooltip("Buff生存周期")]
        public float lifeTime;

    [Tooltip("Buff几秒一跳(只对hot、dot生效)")]
        public float interval;

    public enum buffType
    {
        止步,
        沉默,
        减速,
        易伤,
        回血,
        持续伤害,
        冷却缩减,
        霸体,
        斩杀20,
        装备切换
    }

    [Tooltip("当前Buff的类型")]
        public buffType thisType;

    [Tooltip("数值类Buff的生效百分比")]
        public float percent;

    private float lifeTimeTimer;
    private float intervalTimer;
    private float previousValues;

    // Start is called before the first frame update
    void Start()
    {
        intervalTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        LifeTimeFunc();

        BuffIntervalTimeFunc();
    }

    private void LifeTimeFunc()
    {
        if (!target || !target.isAlive)
        {
            DestroyBuff();
        }

        if (lifeTimeTimer >= lifeTime)
        {
            DestroyBuff();
        }
        else
        {
            lifeTimeTimer += Time.deltaTime;
        }
    }

    private void BuffIntervalTimeFunc()
    {
        //Debug.Log("伤害生效");
        if (thisType == buffType.回血 || thisType == buffType.持续伤害)
        {
            if (intervalTimer >= interval)
            {
                target.BuffAction(thisType);
                intervalTimer = 0;
            }
            else
            {
                intervalTimer += Time.deltaTime;
            }
        }
    }

    public void SetTarget(Actor a)
    {
        target = a;
        target.SetBuff(this.gameObject.GetComponent<Buff>());
    }

    public void DestroyBuff()
    {
        if (target.imprisonmentBuff && target.imprisonmentBuff.gameObject == this.gameObject)
        {
            target.imprisonmentBuff = null;
        }
        else if (target.silenceBuff && target.silenceBuff.gameObject == this.gameObject)
        {
            target.silenceBuff = null;
        }
        else if (target.slowDownBuff && target.slowDownBuff.gameObject == this.gameObject)
        {
            target.slowDownBuff = null;
        }
        else if (target.vulnerabilityBuff && target.vulnerabilityBuff.gameObject == this.gameObject)
        {
            target.vulnerabilityBuff = null;
        }
        else if (target.healOverBuff && target.healOverBuff.gameObject == this.gameObject)
        {
            target.healOverBuff = null;
        }
        else if (target.damageOverTimeBuff && target.damageOverTimeBuff.gameObject == this.gameObject)
        {
            target.damageOverTimeBuff = null;
        }
        else if (target.coolDownBuff && target.coolDownBuff.gameObject == this.gameObject)
        {
            target.coolDownBuff = null;
        }
        Destroy(this.gameObject);
    }
}
