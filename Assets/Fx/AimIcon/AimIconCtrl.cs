using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIconCtrl : MonoBehaviour
{
    private float cursorPosX;
    private float cursorPosY;
    private float screenMaxX;
    private float screenMaxY;
    private float norX;
    private float norY;
    public float NmultX=-6.68f;
    public float NmultY=-4.0f;
    public float multX=6.9f;
    public float multY=3.75f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        screenMaxX = Screen.width;
        screenMaxY = Screen.height;
        cursorPosX = Input.mousePosition.x;
        cursorPosY = Input.mousePosition.y;
        norX = cursorPosX / screenMaxX;
        norY = cursorPosY / screenMaxY;
        
        transform.localPosition = new Vector3(Mathf.Lerp(NmultX, multX, norX), Mathf.Lerp(NmultY, multY, norY), 5);

    }
}
