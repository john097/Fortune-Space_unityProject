using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Search_Tutorial_Talk : MonoBehaviour
{
    GameObject player_collider;
    private Dialog_Manager d_manager;
    private bool A = true;
    private bool B = true;
    private Flowchart flowchart;
    public Transform target;
    public GameObject Airwall;
    // Start is called before the first frame update
    void Start()
    {
        flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();

        if (GameObject.Find("Actor"))
        {
            player_collider = GameObject.Find("Actor");
        }

        if (GameObject.Find("BattleManager"))
        {
            d_manager = GameObject.Find("BattleManager").GetComponent<Dialog_Manager>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 30);

        if (flowchart.GetIntegerVariable("Tutorial_Process") == 5)
        {
            
            Airwall.SetActive(false);
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player_collider&&A)
        {
            d_manager.Tutorial_Process_Talk();
            A = false;
        }
    }

}
