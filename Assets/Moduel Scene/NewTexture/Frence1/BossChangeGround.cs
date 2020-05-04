using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChangeGround : MonoBehaviour
{
    public GameObject ground;
    private Material groundMat;
    // Start is called before the first frame update
    void Start()
    {
        groundMat = ground.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        groundMat.SetVector("_Pos", gameObject.transform.position);
    }
}
