using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;
public class Exit_Button : MonoBehaviour
{
    public GameObject Exit_Menu;
    private Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Flowchart1"))
        {
            flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Current_State") != -1)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        flowchart.SetBooleanVariable("IS_TALKING", true);
        Exit_Menu.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        flowchart.SetBooleanVariable("IS_TALKING",false);
        Exit_Menu.SetActive(false);
    }

    public void Main_Menu()
    {
        PlayerPrefs.SetInt("Current_State", -3);
        PlayerPrefs.SetInt("First_In", 1);
        SceneManager.LoadScene("Loading_Scene");
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        PlayerPrefs.SetInt("First_In", 0);
        UnityEditor.EditorApplication.isPlaying = false;
#else
        PlayerPrefs.SetInt("First_In", 0);
         Application.Quit();
#endif
    }

}
