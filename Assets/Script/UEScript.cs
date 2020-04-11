using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UEScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public static void CreateUE(GameObject u)
    {
        GameObject uE = Instantiate(u, GameObject.Find("Canvas").transform);
    }

    public static void UpdateSilder(float value,float minValue,float maxValue,Slider s)
    {
        s.value = value;
        s.minValue = minValue;
        s.maxValue = maxValue;
    }

    public static void UpdateSilder(float value, Slider s)
    {
        s.value = value;
    }

    public static void UpdateText(string s,Text t)
    {
        t.text = s;
    }
}
