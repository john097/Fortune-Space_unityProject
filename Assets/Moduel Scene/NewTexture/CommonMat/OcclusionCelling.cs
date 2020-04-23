using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCelling : MonoBehaviour
{
    public AnimationCurve ac;
    private Material material;
    bool trigger;
    bool revert;
    float time = 0;

    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            trigger = !trigger;
        }
        if (trigger)
        {
            if (!revert)
            {
                time += Time.deltaTime;
                
                material.SetFloat("_ShopWallAlpha", ac.Evaluate(time));
            }
            else
            {
                time -= Time.deltaTime;
                material.SetFloat("_ShopWallAlpha", ac.Evaluate(time));
            }
            time = Mathf.Clamp(time, 0, 1);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            trigger = true;
            revert = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            trigger = true;
            revert = true;
        }
    }
    
}
