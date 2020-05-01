using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeWorldoTri : MonoBehaviour
{
    public GameObject zwb;
    public GameObject zwpe;
    private Animation zwbAnim;
    private Animation zwpeAnim;
    // Start is called before the first frame update
    void Start()
    {
        zwbAnim = zwb.GetComponent<Animation>();
        zwpeAnim = zwpe.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            zwbAnim.CrossFade("ZeWorldoBallToLarge", 1f);
            zwpeAnim.CrossFade("ZeWorldoPostEffectToLarge", 1f);
            
        }
        
    }
}
