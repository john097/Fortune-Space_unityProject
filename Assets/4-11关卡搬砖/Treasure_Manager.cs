using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Manager : MonoBehaviour
{
    public GameObject[] Treasure_Rooms;
    private const string Prefabs = "Prefabs/Treasure_Prefab/";
    private bool a=true;
    public int T_nums;
    public int T_type_num;
    private float check_num;
    private GameObject treasure;

    //tp_gate = Resources.Load(Prefabs + "TP_GATE") as GameObject;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Current_State")== 0|| PlayerPrefs.GetInt("Current_State") == 4)
        {
            a = false;
        }

        if (a)
        {
            Treasure_Rooms = GameObject.FindGameObjectsWithTag("Treasure_Room");

            for (int i = 0; i < Treasure_Rooms.Length; i++)
            {
                T_nums = Random.Range(1, 6);
                

                Bound bound = getBound(Treasure_Rooms[i].transform);


                

                for (int j = 0; j < T_nums; j++)
                {
                    T_type_num = Random.Range(0, 3);

                    switch (T_type_num)
                    {
                        case 0:
                            treasure = Resources.Load(Prefabs + "Gold_Treasure") as GameObject;
                            break;
                        case 1:
                            treasure = Resources.Load(Prefabs + "Heal_Treasure") as GameObject;
                            break;
                        case 2:
                            treasure = Resources.Load(Prefabs + "Weapon_Treasure") as GameObject;
                            break;
                    }

                    Vector3 pos = new Vector3(bound.getRandomX(), bound.y + 2, bound.getRandomZ());
                    Instantiate(treasure, pos, Quaternion.identity);
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Bound getBound(Transform tf)
    {
        Vector3 center = tf.GetComponent<BoxCollider>().bounds.center;
        Vector3 extents = tf.GetComponent<BoxCollider>().bounds.extents;
        Vector3 dL = new Vector3(center.x - extents.x, center.y, center.z - extents.z);
        Vector3 dR = new Vector3(center.x + extents.x, center.y, center.z - extents.z);
        Vector3 sR = new Vector3(center.x + extents.x, center.y, center.z + extents.z);
        Vector3 sL = new Vector3(center.x - extents.x, center.y, center.z + extents.z);
        Bound bound = new Bound(dL, dR, sR, sL, center, center.y);

        return bound;
    }

}
