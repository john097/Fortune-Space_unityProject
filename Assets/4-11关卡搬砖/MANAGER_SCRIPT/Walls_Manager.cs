using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls_Manager : MonoBehaviour
{
    private BattleManager W_Manager;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("BattleManager"))
        {
            W_Manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (W_Manager.BattleFinish)
        {
            gameObject.SetActive(false);
        }
    }
}
