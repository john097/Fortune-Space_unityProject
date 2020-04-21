﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;
public class Start_Scene_Change : MonoBehaviour
{
    private Actor actor;
    private GameObject actor_g;
    public GameObject Defeat_UI;
    private string scene_name;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();

        scene_name = SceneManager.GetActiveScene().name;
        if (scene_name == "Start_Scene")
        {
            PlayerPrefs.SetInt("Current_State", -1);
        }
        else if(scene_name == "SpawnRoom")
        {
            PlayerPrefs.SetInt("Current_State", -2);
        }
        else
        {
            actor = GameObject.Find("Actor").GetComponent<Actor>();
            actor_g = GameObject.Find("Actor");
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(gameObject.scene.name != "Start_Scene")
        //{
        //    if (gameObject.scene.name != "SpawnRoom ")
        //    {
        //        if (!actor.isAlive)
        //        {
        //            Defeat_UI.SetActive(true);

        //            if (Input.anyKeyDown)
        //            {
        //                Destroy(actor_g);
        //                SceneManager.LoadScene("Start_Scene");
        //            }
        //        }
        //    }
               
        //}



    }

    public void Start_Button_Onclick()
    {
        Debug.Log("asd");
        SceneManager.LoadScene("Loading_Scene");
    }

    public void Setting_Button_Onclick()
    {
        //打开设置界面
    }
}
