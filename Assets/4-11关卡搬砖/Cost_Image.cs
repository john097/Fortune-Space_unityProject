using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost_Image : MonoBehaviour
{
    GameObject followPlayerCamera;
    // Start is called before the first frame update
    void Start()
    {
        followPlayerCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>().FollowCamera;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(followPlayerCamera.transform);
    }
}
