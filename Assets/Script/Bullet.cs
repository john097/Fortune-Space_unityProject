using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //子弹类型枚举定义
    public enum bulletType
    {
        直线,
        环绕,
        锁定目标,
        射线检测
    }

    [Tooltip("子弹飞行轨迹类型")]
        public bulletType thisType;

    [Tooltip("子弹生存周期")]
        public float lifeTime;

    [Tooltip("子弹飞行速度")]
        public float speed;

    [Tooltip("子弹几秒后停下")]
        public float timeToStop;

    [Tooltip("子弹伤害")]
        public float damage;

    [Tooltip("子弹是否持续生效")]
        public bool isSustained;

    [Tooltip("子弹易伤倍率")]
        public float vulnerabilityPercent;

    [Tooltip("子弹伤害类型")]
        public Actor.specialType thisSpType;

    [Tooltip("子弹击退时以自身为基准")]
        public bool repelRelativeSelf;

    [Tooltip("子弹命中时击退距离")]
        public float repel;

    [Tooltip("子弹跟踪对象")]
        public GameObject lockOnTarget;

    [Tooltip("子弹是否跟随角色")]
        public bool isFollowActor;

    [Tooltip("子弹碰撞后是否销毁自身")]
        public bool destoryAfterCollision;

    [Tooltip("子弹与本体碰撞后销毁自身")]
        public bool destoryAfterCollisionWithActor;

    [Tooltip("原技能被打断之后是否销毁自身")]
        public bool destoryIfSkillBeCancel;

    [Tooltip("子弹与目标碰撞后销毁目标")]
        public bool destoryTarget;

    [Tooltip("是否需要击杀敌人才触发时停")]
        public bool killTargetMakeTimeSlow;

    [Tooltip("子弹击中目标时触发时停的倍率")]
        public float hitChangeTimeScale;

    [Tooltip("子弹击中目标时触发时停的时间")]
        public float hitChangeTimeScaleTime;

    [Tooltip("子弹是否触发技能变体")]
        public bool skillVariantEvent;

    [Tooltip("子弹给目标附加的Buff")]
        public GameObject[] buffPrefabs;

    [Tooltip("子子弹:在父子弹被销毁之后自动生成的子弹")]
        public GameObject[] bulletSons;

    [Tooltip("子弹生成特效")]
        public GameObject createEffect;

    [Tooltip("子弹碰撞特效")]
        public GameObject hitEffect;

    [Tooltip("子弹销毁特效")]
    public GameObject destroyEffect;

    private float lifeTimeTimer;
    private float recoverTimeScaleTimer;

    [HideInInspector]
        public Actor actor;

    [HideInInspector]
        public Skill skillParent;

    Rigidbody thisRigidbody;

    public float BUFF_CD;//***DISON.VER***
    public float buff_timer;//***DISON.VER***

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<Rigidbody>())
        {
            thisRigidbody = gameObject.GetComponent<Rigidbody>();
        }

        lifeTimeTimer = 0;

        if (!lockOnTarget)
        {
            BulletShoot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        LifeTimeFunc();

        LockOnTargetFunc();

        BulletRotate();

        if (destoryIfSkillBeCancel && actor.UsingSkill != skillParent)
        {
            Destroy(gameObject);
        }
    }

    //正常子弹发射函数
    private void BulletShoot()
    {
        if (isFollowActor)
        {
            this.gameObject.transform.parent = actor.gameObject.transform;
        }

        if (thisType == bulletType.直线)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * speed,ForceMode.Impulse);
        }
        else if (thisType == bulletType.射线检测)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 dir = Vector3.zero;

            if (Physics.Raycast(ray, out hit))
            {
                dir = new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z);
                //Debug.Log(dir);
                Debug.Log(gameObject.transform.position);
                dir = dir - gameObject.transform.position;
            }

            Ray shootRay = new Ray(gameObject.transform.position, dir);

            LayerMask layerMask = (1 << LayerMask.NameToLayer("Enemy"));

            if (Physics.Raycast(shootRay,out hit,2000,layerMask))
            {
                Debug.Log(hit.collider.name);
                Actor a = hit.collider.gameObject.GetComponent<Actor>();
                HitTargetFunc(a);
                Debug.DrawLine(shootRay.origin, hit.point, Color.green);
            }
        }

        if (createEffect)
        {
            CreateEffect(createEffect);
        }
    }

    //正常子弹环绕旋转函数
    private void BulletRotate()
    {
        if (thisType == bulletType.环绕)
        {
            gameObject.transform.RotateAround(actor.transform.position, Vector3.up, speed * Time.deltaTime);
        }
    }

    //锁定目标移动函数
    private void LockOnTargetFunc()
    {
        if (thisType == bulletType.锁定目标)
        {
            gameObject.transform.LookAt(lockOnTarget.transform.position);
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void LifeTimeFunc()
    {
        if (lifeTimeTimer >= timeToStop && timeToStop != 0)
        {
            if (thisRigidbody)
            {
                thisRigidbody.velocity = Vector3.zero;
            }
        }

        if (lifeTimeTimer >= lifeTime)
        {
            DestroyBullet();
        }
        else
        {
                lifeTimeTimer += Time.deltaTime * Time.timeScale;
        }
    }

    private void HitTargetFunc(Actor a)
    {
        //命中逻辑
        if (a.gameObject.layer == LayerMask.NameToLayer("Enemy")  || a.gameObject.layer == LayerMask.NameToLayer("Actor"))
        {
            float d = damage;

            //计算易伤倍率
            if (a.vulnerabilityBuff)
            {
                d = d * vulnerabilityPercent;
            }

            //克制关系
            if (thisSpType == Actor.specialType.远 && a.thisSpType == Actor.specialType.近)
            {
                d *= 2;
            }
            else if (thisSpType == Actor.specialType.近 && a.thisSpType == Actor.specialType.远)
            {
                d *= 2;
            }

            if (isSustained)
            {
                d *= Time.deltaTime;
            }

            //造成伤害
            a.TakeDamege(d);

            //击退
            if (!a.superArmorBuff && repel != 0)
            {
                Vector3 dir;
                if (repelRelativeSelf)
                {
                   dir  = a.gameObject.transform.position - this.gameObject.transform.position;
                }
                else
                {
                   dir = a.gameObject.transform.position - actor.gameObject.transform.position;
                }
                
                dir.y = 0;
                dir = dir.normalized * repel;

                
                a.thisRigidbody.AddForce(dir,ForceMode.Impulse);
            }

            //施加Buff
            if (buffPrefabs.Length != 0)
            {
                for (int i = 0; i < buffPrefabs.Length; i++)
                {
                    GameObject b = Instantiate(buffPrefabs[i]);
                    b.GetComponent<Buff>().SetTarget(a);
                    if (b.GetComponent<Buff>().thisType == Buff.buffType.斩杀20)
                    {
                        if (a.heal < (a.maxHeal * 0.2))
                        {
                            if (hitChangeTimeScaleTime > 0)
                            {
                                if (a.isAlive)
                                {
                                    ChangeTimeScaleFunc(hitChangeTimeScale, hitChangeTimeScaleTime);
                                    skillParent.coolDownTimer = skillParent.coolDownTime - 0.5f;
                                }
                            }
                        }
                    }
                    b.transform.parent = null;
                }
            }

            if (killTargetMakeTimeSlow)
            {
                if (!a || a.isAlive)
                {
                    if (hitChangeTimeScaleTime > 0)
                    {
                        ChangeTimeScaleFunc(hitChangeTimeScale, hitChangeTimeScaleTime);
                    }
                }
            }
            else
            {
                    if (hitChangeTimeScaleTime > 0)
                    {
                        ChangeTimeScaleFunc(hitChangeTimeScale, hitChangeTimeScaleTime);
                    }
            }

        }
    }

    private void CreateEffect(GameObject e)
    {
        GameObject a = Instantiate(e,gameObject.transform);
        a.transform.parent = null;
    }

    private void ChangeTimeScaleFunc(float i,float k)
    {
        if (i > 0)
        {
            actor.hitChangeTimeScaleTime = k;
            actor.recoverTimeScaleTimer = 0;
            Time.timeScale = i;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Actor a = collision.gameObject.GetComponent<Actor>();
        if (a)
        {
            HitTargetFunc(a);
        }

        if (skillVariantEvent)
        {
            skillParent.UseSkillVariant();
        }

        if (destoryTarget)
        {
            Destroy(collision.gameObject);
        }

        //碰撞后是否销毁自身
        if (destoryAfterCollision)
        {
            DestroyBullet();
        }

        if (destoryAfterCollisionWithActor && a.gameObject == actor.gameObject)
        {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Actor"))
        {
            Actor a = other.gameObject.GetComponent<Actor>();
            if (a)
            {
                HitTargetFunc(a);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Tool"))
        {
            Stage s = other.gameObject.GetComponent<Stage>();
            s.TakeDamege(damage);
        }

        if (hitEffect)
        {
            CreateEffect(hitEffect);
        }

        if (skillVariantEvent)
        {
            skillParent.UseSkillVariant();
        }

        if (destoryTarget)
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject && destoryAfterCollisionWithActor && other.gameObject == actor.gameObject)
        {
            DestroyBullet();
        }


        //碰撞后是否销毁自身
        if (destoryAfterCollision)
        {
            DestroyBullet();
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        //if (isSustained)
        //{
        //    Actor a = other.gameObject.GetComponent<Actor>();
        //    if (a)
        //    {
        //        HitTargetFunc(a);
        //    }

        //    if (skillVariantEvent)
        //    {
        //        skillParent.UseSkillVariant();
        //    }
        //}

        Actor a = other.gameObject.GetComponent<Actor>();
        buff_timer += Time.deltaTime;

        if (a && isSustained && buff_timer >= BUFF_CD)
        {
            HitTargetFunc(a);
            buff_timer = 0;
        }
        if (skillVariantEvent && isSustained && buff_timer >= BUFF_CD)
        {
            skillParent.UseSkillVariant();
            buff_timer = 0;
        }

    }

    private void DestroyBullet()
    {
        if (destroyEffect)
        {
            CreateEffect(destroyEffect);
        }


        if (bulletSons.Length != 0)
        {
            for (int i = 0; i < bulletSons.Length; i++)
            {
                if (bulletSons[i])
                {
                    GameObject bs = Instantiate(bulletSons[i], gameObject.transform.position, Quaternion.identity);
                    bs.GetComponent<Bullet>().lockOnTarget = actor.gameObject;
                    bs.GetComponent<Bullet>().actor = actor;
                    bs.transform.parent = null;
                }
            }
        }

        Destroy(this.gameObject);
    }
}