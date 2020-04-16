using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Treasure_Drop : MonoBehaviour
{
    private Actor mons_actor;
    private const string Prefabs = "Prefabs/Treasure_Prefab/";
    private GameObject treasure;
    private bool A;
    public float B;
    // Start is called before the first frame update
    void Start()
    {
        mons_actor = GetComponent<Actor>();
        treasure = Resources.Load(Prefabs + "Heal_Treasure") as GameObject;
        A = true;
        
       
    }

    // Update is called once per frame
    void Update()
    {

        if (!mons_actor.isAlive&&A)
        {
            
            
            if (B > 8)
            {
                B = Random.Range(0f, 11f);
                Instantiate(treasure, gameObject.transform.position, Quaternion.identity);
                A = false;
            }
           
        }
    }
}
