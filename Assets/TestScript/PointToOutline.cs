using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToOutline : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray;
            RaycastHit raycastHit;
            
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out raycastHit,100f))
            {
                if (raycastHit.transform.name=="Cube")
                {

                }
                Debug.Log(raycastHit.point);
                
            }
        }
        
        
    }
}
