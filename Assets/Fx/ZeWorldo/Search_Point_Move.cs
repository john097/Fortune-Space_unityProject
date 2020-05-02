using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search_Point_Move : MonoBehaviour
{
    public Transform actor;
    public bool A=false;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Actor"))
        {
            actor = GameObject.Find("Actor").transform;
            A = true;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        if (A)
        {
            transform.position = new Vector3(actor.position.x, actor.position.y, actor.position.z);
        }
    }
}
