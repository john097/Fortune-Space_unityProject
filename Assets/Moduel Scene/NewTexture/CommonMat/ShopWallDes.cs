using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWallDes : MonoBehaviour
{
    public AnimationCurve ac;
    private GameObject shopArea;
    private bool fresh = false;
    private float time;
    private bool trigger;
    private bool revert;
    private void Start()
    {
        shopArea = this.gameObject;
    }
    private void Update()
    {
        if (fresh) {
            foreach (Transform item in transform)
            {
                time += Time.deltaTime;
                item.GetComponent<Renderer>().material.SetFloat("_ShopWallAlpha", ac.Evaluate(time));
            }
        }
        else
        {
            foreach (Transform item in transform)
            {
                time -= Time.deltaTime;
                item.GetComponent<Renderer>().material.SetFloat("_ShopWallAlpha", ac.Evaluate(time));
            } 
        }
        time = Mathf.Clamp(time, 0, 1);

    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            fresh = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            fresh = false;
        }
    }
}
