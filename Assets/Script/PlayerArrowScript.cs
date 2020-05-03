using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerArrowScript : MonoBehaviour
{
    public GameObject arrowPrefab;

    [HideInInspector]
        public GameObject target;

    private GameObject player;
    private GameObject playerArrows;

    public int target_type;
    public bool search_done;
    public bool start_search;
    public bool search_cooldown;
    public bool arrow_born;
    public bool audio_go=true;
    public bool audio_back = true;
    public AudioMixer MIXER;
    public float AUDIO;
    void Start()
    {
        if (!arrowPrefab)
        {
            arrowPrefab = Resources.Load("Prefabs/Arrow") as GameObject;
        }
        //target = GameObject.FindGameObjectsWithTag("TP_GATE");
        

        //playerArrows = new GameObject[target.Length];

        search_done = false;
        start_search = false;
        search_cooldown = true;
        arrow_born = false;
        //MIXER.GetFloat("Main_Cutoff",out AUDIO);

    }

    // Update is called once per frame
    void Update()
    {

        if (!audio_go)
        {
            AUDIO = Mathf.Lerp(15000, 1000, 3);
            //MIXER.SetFloat("Main_Cutoff", AUDIO);
        }
        if (!audio_back)
        {
            AUDIO = Mathf.Lerp(1000, 15000, 3);
            //MIXER.SetFloat("Main_Cutoff", AUDIO);
        }


        if (search_cooldown&&Input.GetKeyDown(KeyCode.T))
        {
            audio_go = false;
            StartCoroutine(AUDIO_BACK(3));
            start_search = true;
            search_cooldown = false;


        }

        if (start_search)
        {
            if (search_done&&!arrow_born)
            {
                StartArrow();
                StartCoroutine(Search_Skill_Remove(5f));
                arrow_born = true;
            }
        }

        
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    DestroyArrow();
        //}
    }
    IEnumerator AUDIO_BACK(float duration)
    {
        yield return new WaitForSeconds(duration);
        audio_go = true;
        audio_back = false;
        StartCoroutine(AUDIO_FINISH(3));
    }
    IEnumerator AUDIO_FINISH(float duration)
    {
        yield return new WaitForSeconds(duration);
        audio_back = true;
    }

    IEnumerator Search_Skill_Remove(float duration)
    {
        yield return new WaitForSeconds(duration);
        start_search = false;
        search_done = false;
        search_cooldown = true;
        arrow_born = false;
        DestroyArrow();
    }

    //创建角色指向传送门的箭头
    public void StartArrow()
    {
        //target = GameObject.FindGameObjectsWithTag("TP_GATE");

        player = GameObject.FindGameObjectWithTag("Player");

        //if (player && playerArrows.Length != 0)
        //{
        //    foreach (var item in playerArrows)
        //    {
        //        if (item != null)
        //        {
        //            DestroyImmediate(item);
        //        }
        //    }
        //}

        //playerArrows = new GameObject[target.Length];

        //int i = 0;

        //foreach (var item in target)
        //{
        //    playerArrows[i] = Instantiate(arrowPrefab, player.transform);
        //    playerArrows[i].GetComponent<Arrow>().target = target[i];
        //    i++;
        //}

        playerArrows = Instantiate(arrowPrefab, player.transform);
        playerArrows.GetComponent<Arrow>().target = target;
    }

    //删除角色身上的指向箭头
    public void DestroyArrow()
    {
        //if (playerArrows.Length != 0)
        //{
        //    foreach (var item in playerArrows)
        //    {
        //        DestroyImmediate(item);
        //    }
        //}
        DestroyImmediate(playerArrows);
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (!search_done)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Tool"))
            {
                switch (other.gameObject.tag)
                {
                    case "TP_GATE":
                        target = other.gameObject;
                        Debug.Log("asd");
                        search_done = true;
                        break;
                    case "Blood_Box":
                        target = other.gameObject;
                        search_done = true;
                        break;
                    case "Gold_Box":
                        target = other.gameObject;
                        Debug.Log("asd");
                        search_done = true;
                        break;
                    case "Miracle_Box":
                        target = other.gameObject;
                        search_done = true;
                        break;
                    case "Power_Box":
                        target = other.gameObject;
                        search_done = true;
                        break;
                }
            }
        }

    }

}
