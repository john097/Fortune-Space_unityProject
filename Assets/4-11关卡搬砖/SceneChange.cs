using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneChange : MonoBehaviour
{
    private AsyncOperation async = null;
    Image black;
    private float progressValue;
    private bool Start_Change = true;
    private bool fade_out = false;
    private float timer;
    public float wait_time;

    public Transform tp_zoom;
    public Transform actor;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        black =GameObject.Find("Fade_IN_OUT_Image").GetComponent<Image>();
        black.DOFade(0, 2);

        if (GameObject.Find("Actor"))
        {
            player = GameObject.Find("Actor");
            actor = GameObject.Find("Actor").transform;
            actor.position = new Vector3(tp_zoom.position.x, tp_zoom.position.y,tp_zoom.position.z);
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
            
         
       
        if(timer> wait_time - 1 && !fade_out)
        {
            black.DOFade(1, 1);
            fade_out = true;
        }

        if (timer > wait_time&& Start_Change)
        {

            if (PlayerPrefs.GetInt("Spawn_To_Level_1") == 1)
            {
                StartCoroutine("LoadScene2");
                Start_Change = false;

            }
            else
            {
                StartCoroutine("LoadScene");
                Start_Change = false;
                Debug.Log("!=1");
            }
        }
        
        
    }

    IEnumerator LoadScene()
    {
        
            switch (PlayerPrefs.GetInt("Current_State"))
            {
                case -2:
                
                PlayerPrefs.SetInt("Current_State", 0);
                async = SceneManager.LoadSceneAsync("Tutorial Scene");
                async.allowSceneActivation = true;

                    break;



                case -1:
                    PlayerPrefs.SetInt("Current_State", -2);
                    async = SceneManager.LoadSceneAsync("SpawnRoom");
                async.allowSceneActivation = true;

                break;

                case 0:
                    PlayerPrefs.SetInt("Current_State", 1);
                    async = SceneManager.LoadSceneAsync("Level 1 Scene");
                async.allowSceneActivation = true;

                break;
                case 1:
                if (PlayerPrefs.GetInt("Player_Dead") == 1)
                {
                    PlayerPrefs.DeleteKey("Player_Dead");
                    PlayerPrefs.SetInt("Current_State", -2);
                    async = SceneManager.LoadSceneAsync("SpawnRoom");
                    async.allowSceneActivation = true;
                    
                }
                else
                {
                    PlayerPrefs.SetInt("Current_State", 3);
                    async = SceneManager.LoadSceneAsync("Level3Mod");
                    async.allowSceneActivation = true;
                }
                

                break;

                case 2:
                    PlayerPrefs.SetInt("Current_State", 3);
                    async = SceneManager.LoadSceneAsync("Level3Mod");
                async.allowSceneActivation = true;

                break;

                case 3:
                if (PlayerPrefs.GetInt("Player_Dead") == 1)
                {
                    PlayerPrefs.DeleteKey("Player_Dead");
                    PlayerPrefs.SetInt("Current_State", -2);
                    async = SceneManager.LoadSceneAsync("SpawnRoom");
                    async.allowSceneActivation = true;
                }
                else
                {
                    PlayerPrefs.SetInt("Current_State", 4);
                    async = SceneManager.LoadSceneAsync("BossRoom");
                    async.allowSceneActivation = true;
                }
                    

                break;
                case 4:
                    Destroy(player);
                    PlayerPrefs.SetInt("Current_State", -1);
                    async = SceneManager.LoadSceneAsync("SpawnRoom");
                    async.allowSceneActivation = true;

                break;
        }
        
        

            yield return null;
        }

    IEnumerator LoadScene2()
    {
        PlayerPrefs.SetInt("Current_State", 1);
        async = SceneManager.LoadSceneAsync("Level 1 Scene");
        async.allowSceneActivation = true;


        yield return null;
    }

}
