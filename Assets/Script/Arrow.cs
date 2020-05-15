using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject target;
    private Vector3 dir;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.zero;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 8f)
        {
            dir.x = target.transform.position.x;
            dir.y = gameObject.transform.position.y;
            dir.z = target.transform.position.z;
            gameObject.transform.LookAt(dir);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }


}
