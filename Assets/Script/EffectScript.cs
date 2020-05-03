﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public float lifeTime;
    private GameObject muzzle;
    public bool notAlignMuzzle;


    // Start is called before the first frame update
    void Start()
    {
        if (!notAlignMuzzle)
        {
            muzzle = GameObject.Find("Muzzle");
        }
        
        if (lifeTime != 0)
        {
            Destroy(gameObject, lifeTime);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (muzzle)
        {
            transform.position = muzzle.transform.position;
        }
    }
}
