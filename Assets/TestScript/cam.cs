using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    public Transform player;

    private Vector3 offset;
    public float camspeed;
    public float length;
    public float mousenstivity;
    // Start is called before the first frame update
    void Start()
    {
        offset = -Vector3.forward * 1.3f + Vector3.up * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            length += -Input.GetAxis("Mouse ScrollWheel") * mousenstivity;
        }
    }
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset * length, camspeed * 0.1f);
        
    }
}
