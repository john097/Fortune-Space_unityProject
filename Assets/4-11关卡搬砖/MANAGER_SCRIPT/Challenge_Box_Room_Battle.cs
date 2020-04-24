using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
public class Challenge_Box_Room_Battle : MonoBehaviour
{
    public int this_box_type;
    private Flowchart flowchart;
    public bool Deal;
    public bool Choose=false;
    private Actor actor;
    private Credit actor_credit;
    private int action_num;
    private const string Treasure_Prefabs = "Prefabs/Treasure_Prefab/";

    private Material A;
    private float effect_num = 0;
    private float timer = 0;
    private float timer1 = 0;
    private bool born;
    public bool not_me = false;
    public bool lottery_paid_talk = false;

    // Start is called before the first frame update

    public void SetBoxType(int a)
    {
        this_box_type = a;
    }

    void Start()
    {
        Deal = false;
        born = false;
        A = GetComponent<Renderer>().material;

        if (GameObject.Find("Actor"))
        {
            actor = GameObject.Find("Actor").GetComponent<Actor>();
            actor_credit= GameObject.Find("Actor").GetComponent<Credit>();
        }
        

        if (GameObject.Find("Flowchart1"))
        {
            flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!born)
        {
            timer1 += Time.deltaTime;
            if (timer1 > 2f)
            {
                timer1 = 2f;
                born = true;

            }
            effect_num = Mathf.Lerp(1, 0, timer1);
            A.SetFloat("_Alpha_Dis", effect_num);
        }
        if (not_me)
        {
            timer += Time.deltaTime;
            if (timer > 4f)
            {
                timer = 4f;
                not_me = false;
            }
            effect_num = Mathf.Lerp(0, 1, timer);
            A.SetFloat("_Alpha_Dis", effect_num);
        }

        if (flowchart.GetBooleanVariable("Bloody_Box_Deal")&&!Deal)
        {
            Choose = true;
            float damage = actor.heal * 0.25f;
            actor.TakeDamege(damage);
            //武器奖励
            GameObject Weapon;
            Weapon = Resources.Load(Treasure_Prefabs + "Weapon_Treasure") as GameObject;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(Weapon, pos, Quaternion.identity);

            Deal = true;
        }
        if (flowchart.GetBooleanVariable("Golden_Box_Deal") && !Deal)
        {
            if (actor_credit.playerCredit < 100)
            {
                flowchart.SetBooleanVariable("Golden_Box_Deal", false);
                flowchart.SendFungusMessage("Money_Needed");
            }
            
            if(!lottery_paid_talk&& actor_credit.playerCredit >= 100)
            {
                flowchart.SendFungusMessage("Lottery_Paid_Talk");
                lottery_paid_talk = true;
            }

            Choose = true;
            
            //乐透抽奖
            if (actor_credit.playerCredit>= flowchart.GetFloatVariable("Lottery_ticket")&&flowchart.GetBooleanVariable("Lottery_paid"))
            {
                actor_credit.playerCredit -= (int)flowchart.GetFloatVariable("Lottery_ticket");
                flowchart.SetBooleanVariable("Lottery_paid", false);
            }

            if (flowchart.GetBooleanVariable("Lottery_Open"))
            {
                int Lottery_Bonus = Random.Range(0, 10);

                if (Lottery_Bonus <= 5)//安慰奖
                {
                    flowchart.SendFungusMessage("Lottery_Bonus_0");
                    actor_credit.playerCredit += (int)(flowchart.GetFloatVariable("Lottery_ticket")*0.4f);
                    Deal = true;
                }
                else if(Lottery_Bonus>=6&& Lottery_Bonus <= 8)//小奖
                {
                    flowchart.SendFungusMessage("Lottery_Bonus_1");
                    actor_credit.playerCredit += (int)(flowchart.GetFloatVariable("Lottery_ticket") * 1.25f);
                    Deal = true;
                }
                else//究极大奖
                {
                    flowchart.SendFungusMessage("Lottery_Bonus_2");
                    actor_credit.playerCredit += (int)(flowchart.GetFloatVariable("Lottery_ticket") * 2f);

                    GameObject Weapon;
                    Weapon = Resources.Load(Treasure_Prefabs + "Weapon_Treasure") as GameObject;
                    Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Instantiate(Weapon, pos, Quaternion.identity);

                    Deal = true;
                }
                
            }
            
            
            
        }
        if (flowchart.GetBooleanVariable("Random_Box_Deal") && !Deal)
        {
            Choose = true;
            actor.TakeDamege(actor.maxHeal * 0.25f);
            action_num = Random.Range(0, 10);

            if (action_num <= 5)
            {
                //战斗
                flowchart.SendFungusMessage("Random_Box_Fight");

                Debug.Log("战斗");
                Deal = true;
            }
            else if (action_num >= 6 && action_num <=8)
            {
                //回复血量
                flowchart.SendFungusMessage("Random_Box_Heal");
                float damage = actor.maxHeal * 0.25f;
                actor.TakeDamege(-damage);

                Deal = true;
            }
            else
            {
                //武器奖励
                flowchart.SendFungusMessage("Random_Box_Weapon");

                GameObject Weapon;
                Weapon = Resources.Load(Treasure_Prefabs + "Weapon_Treasure") as GameObject;
                Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Instantiate(Weapon, pos, Quaternion.identity);

                Deal = true;
            }
                
        }
    }
    
    public void Box_Event()
    {
        switch (this_box_type)
        {
            case 0:
                flowchart.SendFungusMessage("Bloody_Box_Start");
                break;
            case 1:
                flowchart.SendFungusMessage("Golden_Box_Start");
                break;
            case 2:
                flowchart.SendFungusMessage("Random_Box_Start");
                break;
        }
    }
    

    public void Fadeout()
    {
        not_me = true;
    }

}
