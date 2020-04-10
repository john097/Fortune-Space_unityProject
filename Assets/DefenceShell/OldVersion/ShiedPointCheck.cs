using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiedPointCheck : MonoBehaviour
{
    
    public GameObject beHitParticle;
    private Material mat;
    private GameObject gmself;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            gmself = this.gameObject;
            var beHit = Instantiate(beHitParticle, gmself.transform) as GameObject;
            var psr = beHit.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            mat = psr.material;
            mat.SetVector("_SphereCenter", collision.contacts[0].point);
            Destroy(beHit, 2);
            
        }
    }
}
