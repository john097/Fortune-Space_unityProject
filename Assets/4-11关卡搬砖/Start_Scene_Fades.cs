using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Start_Scene_Fades : MonoBehaviour
{
    public Image[] Staff_Icon;
    public Text[] Staff_Name;
    public float Fade_Time;
    public float keep;
    public float dur;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(JIEGE_IN(dur));
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //——————杰哥
    IEnumerator JIEGE_IN(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[0].DOFade(1, Fade_Time);
        Staff_Name[0].DOFade(1, Fade_Time);
        StartCoroutine(JIEGE_OUT(keep));
    }
    IEnumerator JIEGE_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[0].DOFade(0, Fade_Time);
        Staff_Name[0].DOFade(0, Fade_Time);
        StartCoroutine(HAISHAN_IN(dur));
    }


    //——————海山
    IEnumerator HAISHAN_IN(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[1].DOFade(1, Fade_Time);
        Staff_Name[1].DOFade(1, Fade_Time);
        StartCoroutine(HAISHAN_OUT(keep));
    }
    IEnumerator HAISHAN_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[1].DOFade(0, Fade_Time);
        Staff_Name[1].DOFade(0, Fade_Time);
        StartCoroutine(ZHENYU_IN(dur));
    }


    //——————镇宇
    IEnumerator ZHENYU_IN(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[2].DOFade(1, Fade_Time);
        Staff_Name[2].DOFade(1, Fade_Time);
        StartCoroutine(ZHENYU_OUT(keep));
    }
    IEnumerator ZHENYU_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[2].DOFade(0, Fade_Time);
        Staff_Name[2].DOFade(0, Fade_Time);
        StartCoroutine(YIHAN_IN(dur));
    }


    //——————乙晗
    IEnumerator YIHAN_IN(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[3].DOFade(1, Fade_Time);
        Staff_Name[3].DOFade(1, Fade_Time);
        StartCoroutine(YIHAN_OUT(keep));
    }
    IEnumerator YIHAN_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[3].DOFade(0, Fade_Time);
        Staff_Name[3].DOFade(0, Fade_Time);
        StartCoroutine(DISHEN_IN(dur));
    }


    //——————迪深
    IEnumerator DISHEN_IN(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[4].DOFade(1, Fade_Time);
        Staff_Name[4].DOFade(1, Fade_Time);
        StartCoroutine(DISHEN_OUT(keep));
    }
    IEnumerator DISHEN_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[4].DOFade(0, Fade_Time);
        Staff_Name[4].DOFade(0, Fade_Time);
        StartCoroutine(STAFF_OUT(dur));
    }


    IEnumerator STAFF_OUT(float duration)
    {
        yield return new WaitForSeconds(duration);
        Staff_Icon[5].DOFade(0, Fade_Time);
        StartCoroutine(STAFF_FINISH(Fade_Time));

    }

    IEnumerator STAFF_FINISH(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
