using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE : MonoBehaviour
{
    public Quaternion mon_rotation;//转身前角度
    public Quaternion lookat_rotation;//准备朝向的角度.
    public Transform target;
    public float per_second_rotate=1080.0f;//转身速度（每秒转多少度）
    public float lerp_speed = 0.0f;//旋转角度越大，lerp变化速度应该越慢
    public float lerp_tm = 0.0f;//lerp的动态参数

    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        mon_rotation = transform.rotation;
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        lookat_rotation = transform.rotation;
        float rotate_angle = Quaternion.Angle(mon_rotation, lookat_rotation);
        // 获得lerp速度
        lerp_speed = per_second_rotate / rotate_angle;
        lerp_tm = 0.0f;

        lerp_tm += Time.deltaTime * lerp_speed;
        transform.rotation = Quaternion.Lerp(mon_rotation, lookat_rotation, lerp_tm);
        if (lerp_tm >= 1)
        {
            transform.rotation = lookat_rotation;
            // 此时, 转身完毕, 已经对着目标物体
        }


    }
}
