using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject target;
    private Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        dir.x = target.transform.position.x;
        dir.z = target.transform.position.z;
        gameObject.transform.LookAt(dir);
    }


}
