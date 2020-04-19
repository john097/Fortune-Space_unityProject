using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowScript : MonoBehaviour
{
    public GameObject arrowPrefab;

    [HideInInspector]
        public GameObject[] target;

    private GameObject player;

    void Start()
    {
        if (!arrowPrefab)
        {

        }
        target = GameObject.FindGameObjectsWithTag("TP_GATE");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartArrow()
    {
        foreach (var item in target)
        {
            
        }
    }
}
