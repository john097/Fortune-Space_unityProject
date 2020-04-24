using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private CinemachineCameraOffset vCam;
    private Vector3 currentOffset;
    private Vector3 t;
    private Vector3 changeOffset;
    public float shakeSpeed;
    public float recoverSpeed;

    private bool shaking;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.GetChild(transform.childCount).GetComponent<CinemachineCameraOffset>())
        {
            vCam = transform.GetChild(transform.childCount).GetComponent<CinemachineCameraOffset>();
            currentOffset = vCam.m_Offset;
            changeOffset = currentOffset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vCam)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                CameraShake(new Vector3(1, 1, currentOffset.z));
            }

            if (!shaking)
            {
                vCam.m_Offset = Vector3.Lerp(t, currentOffset, recoverSpeed);

            }
            else
            {
                vCam.m_Offset = Vector3.Lerp(t, changeOffset, shakeSpeed);
            }

            t = vCam.m_Offset;

            if (t == changeOffset)
            {
                shaking = false;
            }
        }

        
    }

    public void CameraShake(Vector3 cO)
    {
        changeOffset = cO;
        shaking = true;
    }
}
