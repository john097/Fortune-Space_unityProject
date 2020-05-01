using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagGetActor : MonoBehaviour
{
    private Cloth thisCloth;
    private GameObject actor;
    private SphereCollider sc;
    // Start is called before the first frame update
    void Start()
    {
        actor = GameObject.Find("ActorModel");
        thisCloth = gameObject.GetComponent<Cloth>();
        sc = actor.GetComponent<SphereCollider>();
        addCollider(ref thisCloth,sc);

    }
    private void addCollider(ref Cloth c, SphereCollider sc)
    {
        ClothSphereColliderPair[] cscp = new ClothSphereColliderPair[c.capsuleColliders.Length + 1]; //重新声明碰撞器数组
        for (int i = 0; i < c.sphereColliders.Length; i++)
        {
            cscp[i] = c.sphereColliders[i];                             //初始化碰撞器数组
        }
        cscp[cscp.Length - 1] = new ClothSphereColliderPair(sc);        //添加碰撞器
        c.sphereColliders = cscp;
    }
}
