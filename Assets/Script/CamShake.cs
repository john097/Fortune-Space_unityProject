using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private CinemachineCameraOffset vCam;
    private Vector3 currentOffset;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.GetChild(transform.childCount).GetComponent<CinemachineCameraOffset>())
        {
            vCam = transform.GetChild(transform.childCount).GetComponent<CinemachineCameraOffset>();
            currentOffset = vCam.m_Offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraShake(float x, float y, float z)
    {
        
    }
}
