using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFxRandom : MonoBehaviour
{
    Material cloud_mat;
    // Start is called before the first frame update
    void Start()
    {

        cloud_mat = this.gameObject.GetComponent<Renderer>().material;

        InvokeRepeating("ChangeRepeat", 1, 5);
    }
    void ChangeRepeat() {
        cloud_mat.SetFloat("_RandomFx", 1);
        cloud_mat.SetFloat("_ElectricMult",0.5f);
        Invoke("delayZeroOne", 0.05f);

    }
    void delayZeroOne()
    {
        cloud_mat.SetFloat("_RandomFx", 0);
        cloud_mat.SetFloat("_ElectricMult", 1);
        Invoke("delayOneZero", 0.05f);
    }
    void delayOneZero()
    {
        cloud_mat.SetFloat("_RandomFx", 1);
        cloud_mat.SetFloat("_ElectricMult", 0.5f);
        Invoke("delayZero", 0.05f);
    }
    void delayZero()
    {
        cloud_mat.SetFloat("_RandomFx", 0);
        cloud_mat.SetFloat("_ElectricMult", 1);
    }

}
