using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowScript : MonoBehaviour
{
    public GameObject arrowPrefab;

    [HideInInspector]
        public GameObject[] target;

    private GameObject player;
    private GameObject[] playerArrows;

    void Start()
    {
        if (!arrowPrefab)
        {
            arrowPrefab = Resources.Load("Prefabs/Arrow") as GameObject;
        }
         target = GameObject.FindGameObjectsWithTag("TP_GATE");

        playerArrows = new GameObject[target.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartArrow();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            DestroyArrow();
        }
    }

    //创建角色指向传送门的箭头
    public void StartArrow()
    {
        target = GameObject.FindGameObjectsWithTag("TP_GATE");

        player = GameObject.FindGameObjectWithTag("Player");

        if (player && playerArrows.Length != 0)
        {
            foreach (var item in playerArrows)
            {
                if (item != null)
                {
                    DestroyImmediate(item);
                }
            }
        }

        playerArrows = new GameObject[target.Length];

        int i = 0;

        foreach (var item in target)
        {
            playerArrows[i] = Instantiate(arrowPrefab, player.transform);
            playerArrows[i].GetComponent<Arrow>().target = target[i];
            i++;
        }
    }

    //删除角色身上的指向箭头
    public void DestroyArrow()
    {
        if (playerArrows.Length != 0)
        {
            foreach (var item in playerArrows)
            {
                DestroyImmediate(item);
            }
        }
    }
}
