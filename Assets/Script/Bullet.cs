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

    [Tooltip("子弹销毁自身的延迟")]
        public float destoryDelay;

    [Tooltip("子弹震动")]
        public CamShake.ShakeIntensity bulletHitShake;

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

    [Tooltip("子弹开火特效(枪口火焰)")]
        public GameObject createEffect;

    [Tooltip("子弹特效(子弹本体特效)")]
        public GameObject bulletEffect;

    [Tooltip("子弹特效生成时是否对齐枪口位置(非枪械类子弹不勾选)")]
        public bool alignTheMuzzlePosition;

    [Tooltip("子弹特效生成时是否根据角色偏移")]
        public bool alignTheActorPosition;

    [Tooltip("子弹碰撞特效(命中)")]
        public GameObject hitEffect;

    [Tooltip("子弹销毁特效(爆炸类子弹的引爆特效)")]
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
            CreateEffect(createEffect,alignTheMuzzlePosition,false);
            
        }

        if (bulletEffect)
        {
            CreateEffect(bulletEffect, alignTheMuzzlePosition, true);
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
            if (Time.timeScale != 1)
            {
                lifeTimeTimer += Time.deltaTime - Time.deltaTime * Time.timeScale;
            }
            else
            {
                lifeTimeTimer += Time.deltaTime;
            }
        }
    }

    private void HitTargetFunc(Actor a)
    {
        //命中逻辑
        if (a.gameObject.layer == LayerMask.NameToLayer("Enemy")  || a.gameObject.layer == LayerMask.NameToLayer("Actor"))
        {
            float d= damage;

            if (gameObject.layer!= LayerMask.NameToLayer("Particles"))
            {
               d+=actor.attack;
            }
            
            string dTipString = "";
            Color dTipColor = Color.white;
            GameObject dTip = null;

            //计算易伤倍率
            if (a.vulnerabilityBuff)
            {
                d = d * vulnerabilityPercent;
                dTipColor = Color.blue;
            }

            

            //克制关系
            if (thisSpType == Actor.specialType.远 && a.thisSpType == Actor.specialType.近)
            {
                d *= 2;
                dTipString = d + " Counter!";

                if (dTipColor == Color.blue)
                {
                    dTipColor += Color.red;
                }
                else
                {
                    dTipColor = Color.red;
                }

            }
            else if (thisSpType == Actor.specialType.近 && a.thisSpType == Actor.specialType.远)
            {
                d *= 2;
                dTipString = d + " Counter!";

                if (dTipColor == Color.blue)
                {
                    dTipColor += Color.red;
                }
                else
                {
                    dTipColor = Color.red;
                }
            }
            else
            {
                dTipString = d.ToString();
            }

            if (a.tag == "ENEMY" && a.transform.Find("HpBar"))
            {
                dTip = Instantiate(Resources.Load("UIPrefabs/T_DamageTip") as GameObject , a.transform.Find("HpBar").transform);
                dTip.GetComponent<UEScript>().damageTipText = dTipString;
                dTip.GetComponent<UEScript>().damageTipColor = dTipColor;
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

                    //斩杀判定
                    if (b.GetComponent<Buff>().thisType == Buff.buffType.斩杀20)
                    {
                        if (a.heal < (a.maxHeal * 0.2))
                        {
                            if (hitChangeTimeScaleTime > 0)
                            {
                                if (a.isAlive)
                                {
                                    ChangeTimeScaleFunc(hitChangeTimeScale, hitChangeTimeScaleTime);

                                    if (a.tag == "ENEMY")
                                    {
                                        actor.gameObject.GetComponent<Credit>().EliminateEvent();
                                    }
                                    
                                    skillParent.coolDownTimer = skillParent.coolDownTime - 0.5f;
                                    if (dTip != null)
                                    {
                                        dTip.GetComponent<UEScript>().damageTipText = "Eliminate!";
                                        dTip.GetComponent<UEScript>().damageTipColor = Color.black;
                                    }

                                    if (actor.isPlayer && GameObject.Find("CM vcam1"))
                                    {
                                        Vector3 c = Camera.main.WorldToScreenPoint(a.transform.position) - Camera.main.WorldToScreenPoint(transform.position);

                                        actor.FollowCamera.GetComponent<CamShake>().CameraShake(c, CamShake.ShakeIntensity.大);
                                    }

                                }
                            }
                        }
                    }


                    b.transform.parent = null;
                }
            }

            //时停判定
            if (killTargetMakeTimeSlow)
            {
                if (!a || !a.isAlive)
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
            //ChangeTimeScaleFunc(0.1f, 0.2f);

            //攻击命中时摄像机震动
            if (bulletHitShake != CamShake.ShakeIntensity.无 && actor.isPlayer && GameObject.Find("CM vcam1"))
            {
                Vector3 c = Camera.main.WorldToScreenPoint(a.transform.position)- Camera.main.WorldToScreenPoint(transform.position);

                actor.FollowCamera.GetComponent<CamShake>().CameraShake(c,bulletHitShake);
            }

        }
    }

    private void CreateEffect(GameObject e,bool alignMuzzle,bool followBullet)
    {
        GameObject a;

        if (alignMuzzle)
        {
            a = Instantiate(e, GameObject.Find("Muzzle").transform.position, Quaternion.identity);

            if (!followBullet)
            {
                Debug.Log("附加自爆脚本");
                a.AddComponent<EffectScript>();
            }

            a.transform.forward = gameObject.transform.forward;
        }
        else 
        {
            if (alignTheActorPosition)
            {
                a = Instantiate(e, actor.transform.position,actor.transform.rotation);
            }
            else
            {
                a = Instantiate(e, gameObject.transform);
            }
        }

        if (a.GetComponent<ShieldHitEffectScript>())
        {
            a.GetComponent<ShieldHitEffectScript>().bulletParent = gameObject;
            a.GetComponent<ShieldHitEffectScript>().ChangeShieldColor(Color.red);
        }

        if (followBullet)
        {
                a.transform.parent = gameObject.transform;
        }
        else 
        {
            a.transform.parent = null;
        }
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
        if (!isSustained)
        {
            if (hitEffect)
            {
                if (thisSpType == Actor.specialType.远)
                {
                    CreateEffect(hitEffect, false, false);
                }
                else
                {
                    Quaternion r = transform.rotation;
                    transform.Rotate(0, 180, 0);
                    GameObject a = Instantiate(hitEffect, other.ClosestPointOnBounds(other.gameObject.transform.position + new Vector3(0, 1, 0)), gameObject.transform.rotation);
                    //transform.position += new Vector3(0,50,0);
                    transform.rotation = r;
                }

            }

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
            else if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
            {
                if (hitChangeTimeScaleTime > 0)
                {
                    ChangeTimeScaleFunc(hitChangeTimeScale, hitChangeTimeScaleTime);
                }
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
        }
        


        //碰撞后是否销毁自身
        if (destoryAfterCollision)
        {
            DestroyBullet();
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (isSustained)
        {
            Actor a = other.gameObject.GetComponent<Actor>();
            if (a)
            {
                //HitTargetFunc(a);
                if (buff_timer >= BUFF_CD)
                {
                    HitTargetFunc(a);
                    buff_timer = 0;
                    
                }
                else
                {
                    buff_timer += Time.deltaTime;
                }

            }

            if (skillVariantEvent)
            {
                skillParent.UseSkillVariant();
            }
        }

        

    }

    private void DestroyBullet()
    {
        if (destroyEffect)
        {
            CreateEffect(destroyEffect,false,false);
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

        Destroy(this.gameObject,destoryDelay);
    }
}