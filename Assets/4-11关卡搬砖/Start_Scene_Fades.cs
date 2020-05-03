using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Start_Scene_Fades : MonoBehaviour
{
    public Image[] Staff_Icon;
    public Text[] Staff_Name;
    public Text[] Staff_Job;
    public float Fade_Time;
    public float keep;
    public float dur;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DISEIGNER_IN(1));
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //——————程序&策划
    IEnumerator DISEIGNER_IN(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[0].DOFade(1, Fade_Time);
        Staff_Name[0].DOFade(1, Fade_Time);
        Staff_Job[0].DOFade(1, Fade_Time);
        Staff_Icon[1].DOFade(1, Fade_Time);
        Staff_Name[1].DOFade(1, Fade_Time);
        Staff_Job[1].DOFade(1, Fade_Time);
        StartCoroutine(DISEIGNER_OUT(keep));
    }
    IEnumerator DISEIGNER_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[0].DOFade(0, Fade_Time);
        Staff_Name[0].DOFade(0, Fade_Time);
        Staff_Job[0].DOFade(0, Fade_Time);
        Staff_Icon[1].DOFade(0, Fade_Time);
        Staff_Name[1].DOFade(0, Fade_Time);
        Staff_Job[1].DOFade(0, Fade_Time);
        StartCoroutine(ARTIST_IN(dur));
    }


    //——————美术
    IEnumerator ARTIST_IN(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[2].DOFade(1, Fade_Time);
        Staff_Name[2].DOFade(1, Fade_Time);
        Staff_Job[2].DOFade(1, Fade_Time);
        Staff_Icon[3].DOFade(1, Fade_Time);
        Staff_Name[3].DOFade(1, Fade_Time);
        Staff_Job[3].DOFade(1, Fade_Time);
        Staff_Icon[4].DOFade(1, Fade_Time);
        Staff_Name[4].DOFade(1, Fade_Time);
        Staff_Job[4].DOFade(1, Fade_Time);
        StartCoroutine(ARTIST_OUT(keep));

    }
    IEnumerator ARTIST_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[2].DOFade(0, Fade_Time);
        Staff_Name[2].DOFade(0, Fade_Time);
        Staff_Job[2].DOFade(0, Fade_Time);
        Staff_Icon[3].DOFade(0, Fade_Time);
        Staff_Name[3].DOFade(0, Fade_Time);
        Staff_Job[3].DOFade(0, Fade_Time);
        Staff_Icon[4].DOFade(0, Fade_Time);
        Staff_Name[4].DOFade(0, Fade_Time);
        Staff_Job[4].DOFade(0, Fade_Time);
        StartCoroutine(STAFF_OUT(dur));
    }


   

    IEnumerator STAFF_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[5].DOFade(0, Fade_Time+3);
        StartCoroutine(STAFF_FINISH(Fade_Time + 2));

    }

    IEnumerator STAFF_FINISH(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
