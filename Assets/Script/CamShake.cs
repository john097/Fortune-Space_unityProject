using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public CinemachineCameraOffset vCam;
    private Vector3 currentOffset;
    private Vector3 t;
    private Vector3 changeOffset;
    public float shakeSpeed;
    public float recoverSpeed;
    public float time;
    public AnimationCurve aC;

    public float small;
    public float medium;
    public float big;

    public bool shaking;

    public enum ShakeIntensity
    {
        无,
        小,
        中,
        大
    }

    // Start is called before the first frame update
    void Start()
    {
        time = 0;

        if (transform.Find("CM vcam1"))
        {
            vCam = transform.Find("CM vcam1").gameObject.GetComponent<CinemachineCameraOffset>();
            currentOffset = vCam.m_Offset;
            changeOffset = currentOffset;

            if (recoverSpeed == 0)
            {
                recoverSpeed = 0.5f;
            }

        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (GameObject.Find("CM vcam1"))
        {
            vCam = GameObject.Find("CM vcam1").gameObject.GetComponent<CinemachineCameraOffset>();
            currentOffset = vCam.m_Offset;
            changeOffset = currentOffset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vCam)
        {
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    CameraShake(new Vector3(2,3,currentOffset.z),ShakeIntensity.中);
            //}

            ShakeFunc();
        }
    }

    private void ShakeFunc()
    {
        if (time < 1)
        {
            time += Time.deltaTime;
        }

        vCam.m_Offset = Vector3.Lerp(currentOffset - (changeOffset - currentOffset), changeOffset, aC.Evaluate(time));
    }

    public void CameraShake(Vector3 cO,ShakeIntensity sI)
    {
        Vector3 c = cO;
        c.z = currentOffset.z;

        switch (sI)
        {
            case ShakeIntensity.无:
                break;
            case ShakeIntensity.小:
                changeOffset = cO.normalized * 0.02f;
                break;
            case ShakeIntensity.中:
                changeOffset = cO.normalized * 0.04f;
                break;
            case ShakeIntensity.大:
                changeOffset = cO.normalized * 0.06f;
                break;
            default:
                break;
        }

        changeOffset.z = currentOffset.z;

        time = 0;
    }
}
