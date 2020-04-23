using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
public class Challenge_Box_Room_Battle : MonoBehaviour
{
    private int this_box_type;
    private Flowchart flowchart;
    private bool Deal;
    private Actor actor;
    private Credit actor_credit;
    private int action_num;
    private const string Treasure_Prefabs = "Prefabs/Treasure_Prefab/";
    // Start is called before the first frame update

    public void SetBoxType(int a)
    {
        this_box_type = a;
    }

    void Start()
    {
        Deal = false;
        if(GameObject.Find("Actor"))
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
        if(flowchart.GetBooleanVariable("Bloody_Box_Deal")&&!Deal)
        {
            actor.TakeDamege(actor.maxHeal * 0.25f);
            //武器奖励
            GameObject Weapon;
            Weapon = Resources.Load(Treasure_Prefabs + "Weapon_Treasure") as GameObject;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(Weapon, pos, Quaternion.identity);

            Deal = true;
        }
        if (flowchart.GetBooleanVariable("Golden_Box_Deal") && !Deal)
        {
            //乐透抽奖
            if(actor_credit.playerCredit>= flowchart.GetFloatVariable("Lottery_ticket")&&flowchart.GetBooleanVariable("Lottery_paid"))
            {
                actor_credit.playerCredit -= (int)flowchart.GetFloatVariable("Lottery_ticket");
                flowchart.SetBooleanVariable("Lottery_paid", false);
            }

            if (flowchart.GetBooleanVariable("Lottery_Open"))
            {
                int Lottery_Bonus = Random.Range(0, 10);

                if (Lottery_Bonus <= 4)//安慰奖
                {
                    flowchart.SendFungusMessage("Lottery_Bonus_0");
                    actor_credit.playerCredit += (int)(flowchart.GetFloatVariable("Lottery_ticket")*0.1f);
                    Deal = true;
                }
                else if(Lottery_Bonus>=5&& Lottery_Bonus <= 8)//小奖
                {
                    flowchart.SendFungusMessage("Lottery_Bonus_1");
                    actor_credit.playerCredit += (int)(flowchart.GetFloatVariable("Lottery_ticket") * 1.5f);
                    Deal = true;
                }
                else//究极大奖
                {
                    flowchart.SendFungusMessage("Lottery_Bonus_2");
                    actor_credit.playerCredit += (int)flowchart.GetFloatVariable("Lottery_ticket");

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
            actor.TakeDamege(actor.maxHeal * 0.25f);
            action_num = Random.Range(0, 10);

            if (action_num <= 5)
            {
                //战斗
            }
            else if (action_num >= 6 && action_num <=8)
            {
                //获得金币，但会扣除血量
                actor.TakeDamege(actor.heal * 0.25f);

                GameObject Gold;
                Gold = Resources.Load(Treasure_Prefabs + "Gold_Treasure") as GameObject;
                Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Instantiate(Gold, pos, Quaternion.identity);
            }
            else
            {
                //武器奖励
                GameObject Weapon;
                Weapon = Resources.Load(Treasure_Prefabs + "Weapon_Treasure") as GameObject;
                Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Instantiate(Weapon, pos, Quaternion.identity);
            }
                Deal = true;
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
    

}
