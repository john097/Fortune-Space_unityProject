using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHitEffectScript : MonoBehaviour
{
    public GameObject bulletParent;
    private Renderer[] thisRenderers;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeShieldColor(Color c)
    {
        thisRenderers = bulletParent.GetComponentsInChildren<Renderer>();

        foreach (var item in thisRenderers)
        {
            item.material.SetColor("_Emissive", Color.Lerp(thisRenderers[0].material.GetColor("_Emissive"),c,2f));
        }
    }
}
