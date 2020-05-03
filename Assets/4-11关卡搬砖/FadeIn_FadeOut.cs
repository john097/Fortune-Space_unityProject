using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Fungus;

public class FadeIn_FadeOut : MonoBehaviour
{
    Image black;
    
    private float timer;

    private Flowchart flowchart;
    private bool flow_found = false;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("Flowchart1"))
        {
            flowchart = GameObject.Find("Flowchart1").GetComponent<Flowchart>();
            flow_found = true;
        }

        timer = 0;
        black = GetComponent<Image>();
        black.DOFade(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (flow_found)
        {
            if (flowchart.GetBooleanVariable("SceneChange"))
            {

            }
        }
    }
}
