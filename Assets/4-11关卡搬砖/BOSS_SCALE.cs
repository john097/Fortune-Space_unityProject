using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_SCALE : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(5, 5, 5);
    }
}
