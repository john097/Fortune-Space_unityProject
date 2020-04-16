using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Room_PointLight : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Current_State") == 4)
        {
            player = GameObject.Find("Actor");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Current_State" ) == 4)
        {
            gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }
    }
}
