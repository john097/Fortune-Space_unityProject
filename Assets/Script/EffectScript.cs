using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public float lifeTime;
    private GameObject muzzle;


    // Start is called before the first frame update
    void Start()
    {
        muzzle = GameObject.Find("Muzzle");
        if (lifeTime != 0)
        {
            Destroy(gameObject, lifeTime);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = muzzle.transform.position;
    }
}
