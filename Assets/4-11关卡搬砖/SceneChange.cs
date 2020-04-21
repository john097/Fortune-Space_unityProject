using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private AsyncOperation async = null;
    private float progressValue;
    private bool a = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Spawn_To_Level_1") == 1)
        {
            StartCoroutine("LoadScene2");
            
        }
        else
        {
            StartCoroutine("LoadScene");
            Debug.Log("!=1");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    PlayerPrefs.SetInt("Current_State", 0);
                    async = SceneManager.LoadSceneAsync("Tutorial Scene");
                    async.allowSceneActivation = true;

                    break;

                case 0:
                    PlayerPrefs.SetInt("Current_State", 1);
                    async = SceneManager.LoadSceneAsync("Level 1 Scene");
                    async.allowSceneActivation = true;

                    break;
                case 1:
                    PlayerPrefs.SetInt("Current_State", 2);
                    async = SceneManager.LoadSceneAsync("Level 2  Scene 1");
                    async.allowSceneActivation = true;
                    break;

                case 2:
                    PlayerPrefs.SetInt("Current_State", 3);
                    async = SceneManager.LoadSceneAsync("Level 3");
                    async.allowSceneActivation = true;
                    break;
                case 3:
                    PlayerPrefs.SetInt("Current_State", 4);
                    async = SceneManager.LoadSceneAsync("BossRoom");
                    async.allowSceneActivation = true;
                    break;
            }
        
        

            yield return null;
        }

    IEnumerator LoadScene2()
    {

        yield return null;
    }

}
