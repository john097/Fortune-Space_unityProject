using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimIconFollowMouse : MonoBehaviour
{

    public Camera camera1;
    public Vector3 v3;
    // Start is called before the first frame update
    void Start()
    {
        camera1 = Camera.main;
    }
    void Update()
    {
        v3.x = camera1.ScreenToWorldPoint(Input.mousePosition).x;
        v3.y = camera1.ScreenToWorldPoint(Input.mousePosition).y;
        transform.position = v3;
        

    }
}
