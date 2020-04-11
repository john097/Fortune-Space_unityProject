using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPointManager : MonoBehaviour
{

    private GameObject Player;
    private Actor actor;
    private BattleManager DP_manager;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Actor");
        actor = Player.GetComponent<Actor>();

        DP_manager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Player&& Input.GetKeyDown(actor.specialInteractiveKey))
        {
            DP_manager.Special_Battle_Start();
            gameObject.SetActive(false);
        }
    }

}
