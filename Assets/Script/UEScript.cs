using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UE : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    private void CreateUE(GameObject u)
    {
        GameObject uE = Instantiate(u, GameObject.Find("Canvas").transform);
    }

    public void UpdateSilder(float i,float minValue,float maxValue,Slider s)
    {
        s.value = i;
        s.minValue = minValue;
        s.maxValue = maxValue;
    }

    public void UpdateSilder(float i,Slider s)
    {
        s.value = i;
    }

    public void UpdateText(string s,Text t)
    {
        t.text = s;
    }
}
