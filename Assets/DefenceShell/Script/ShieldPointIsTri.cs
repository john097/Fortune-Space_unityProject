using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPointIsTri : MonoBehaviour
{
    public GameObject beHitParticle;
    public int InsCount;
    private Material mat;
    private GameObject gmself;
    /*private void OnCollisionEnter(Collision collision)
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
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            gmself = this.gameObject;
            if (gmself.transform.childCount<=InsCount) {
            var beHit = Instantiate(beHitParticle, gmself.transform) as GameObject;
            beHit.transform.parent = gmself.transform;
            var psr = beHit.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            mat = psr.material;
            mat.SetVector("_SphereCenter", other.gameObject.transform.position);
            Destroy(beHit, 1);
            }

        }
    }
}
