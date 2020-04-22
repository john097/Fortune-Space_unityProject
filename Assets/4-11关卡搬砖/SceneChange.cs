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
    private bool Start_Change = false;
    private bool fade_out = false;
    private float timer;
    public float wait_time;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        black =GameObject.Find("Fade_IN_OUT_Image").GetComponent<Image>();
        black.DOFade(0, 2);
        
        if (PlayerPrefs.GetInt("Spawn_To_Level_1") == 1)
        {
            Start_Change = true;
            StartCoroutine("LoadScene2");
           

        }
        else
        {
            Start_Change = true;
            StartCoroutine("LoadScene");
            

            Debug.Log("!=1");
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

        
        
    }

    IEnumerator LoadScene()
    {
        
            switch (PlayerPrefs.GetInt("Current_State"))
            {
                case -2:
                
                PlayerPrefs.SetInt("Current_State", 0);
                async = SceneManager.LoadSceneAsync("Tutorial Scene");
                async.allowSceneActivation = false;

                while (Start_Change)
                {
                    
                    async.allowSceneActivation = false; 
                    if(timer> wait_time)
                    {
                        async.allowSceneActivation = true;
                        Start_Change = false;
                    }
                }
                
                    break;



                case -1:
                    PlayerPrefs.SetInt("Current_State", -2);
                    async = SceneManager.LoadSceneAsync("SpawnRoom");
                //async.allowSceneActivation = true;
                async.allowSceneActivation = false;

                break;

                case 0:
                    PlayerPrefs.SetInt("Current_State", 1);
                    async = SceneManager.LoadSceneAsync("Level 1 Scene");
                //async.allowSceneActivation = true;
                async.allowSceneActivation = false;

                break;
                case 1:
                    PlayerPrefs.SetInt("Current_State", 2);
                    async = SceneManager.LoadSceneAsync("Level 2  Scene 1");
                //async.allowSceneActivation = true;
                async.allowSceneActivation = false;

                break;

                case 2:
                    PlayerPrefs.SetInt("Current_State", 3);
                    async = SceneManager.LoadSceneAsync("Level 3");
                //async.allowSceneActivation = true;
                async.allowSceneActivation = false;

                break;
                case 3:
                    PlayerPrefs.SetInt("Current_State", 4);
                    async = SceneManager.LoadSceneAsync("BossRoom");
                //async.allowSceneActivation = true;
                async.allowSceneActivation = false;

                break;
            }
        
        

            yield return null;
        }

    IEnumerator LoadScene2()
    {
        PlayerPrefs.SetInt("Current_State", 1);
        async = SceneManager.LoadSceneAsync("Level 1 Scene");
        //async.allowSceneActivation = true;
        async.allowSceneActivation = false;


        yield return null;
    }

}
