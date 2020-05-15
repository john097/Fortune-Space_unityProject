using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;
public class Start_Scene_Change : MonoBehaviour
{
    private Actor actor;
    private GameObject actor_g;
    public GameObject Defeat_UI;
    public GameObject setting_UI;
    private string scene_name;
    private float dead_timer;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.DeleteKey("Spawn_To_Level_1");
        
        scene_name = SceneManager.GetActiveScene().name;

        if (scene_name == "Start_Scene")
        {
            PlayerPrefs.SetInt("Current_State", -1);

        }
        else if (scene_name == "SpawnRoom")
        {
            PlayerPrefs.SetInt("Current_State", -2);
            if (GameObject.Find("Search_point"))
            {
                GameObject.Find("Search_point").SetActive(false);
            }
            actor = GameObject.Find("Actor").GetComponent<Actor>();
            actor_g = GameObject.Find("Actor");

        }
        else
        {
            actor = GameObject.Find("Actor").GetComponent<Actor>();
            actor_g = GameObject.Find("Actor");

        }

        switch (scene_name)
        {
            case "Tutorial Scene":
                PlayerPrefs.SetInt("Current_State", 0);
                
                break;
            case "Level 1 Scene":
                PlayerPrefs.SetInt("Current_State", 1);
                
                break;
            case "Level 2  Scene 1":
                PlayerPrefs.SetInt("Current_State", 2);
                
                break;
            case "Level3Mod":
                PlayerPrefs.SetInt("Current_State", 3);
                
                break;
            case "BossRoom":
                PlayerPrefs.SetInt("Current_State", 4);
                
                break;
        }



    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.scene.name != "Start_Scene")
        {
            if (gameObject.scene.name != "SpawnRoom ")
            {
                if (!actor.isAlive)
                {
                    Defeat_UI.SetActive(true);
                    dead_timer += Time.deltaTime;
                    if (Input.anyKeyDown&&dead_timer>=3f)
                    {
                        Destroy(actor_g);
                        SceneManager.LoadScene("Loading_Scene");
                    }
                }
            }

        }



    }

    public void ProtectFailed()
    {
        actor.isAlive = false;
    }

    public void Start_Button_Onclick()
    {
        Debug.Log("asd");
        SceneManager.LoadScene("Loading_Scene");
    }

    public void Setting_Button_Onclick()
    {
        //打开设置界面
        setting_UI.SetActive(!setting_UI.activeSelf);
    }
}
