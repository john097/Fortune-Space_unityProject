using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Effect : MonoBehaviour
{
    [SerializeField] private Text textToUse;
    private Text text_english;
    [SerializeField] private bool useThisText = false;
    [Tooltip("false - Fades Out, true = Fades In")]
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOnStart = false;
    [SerializeField] private float timeMultiplier;
    private float timer;
    private float move_time;
    private bool A = true;

    private Vector2 start;
    private Vector2 middle;
    private Vector2 end;

    // Start is called before the first frame update
    void Start()
    {
        start = gameObject.GetComponent<RectTransform>().position;
        middle = GameObject.Find("text_middle").GetComponent<RectTransform>().position;
        end = GameObject.Find("text_end").GetComponent<RectTransform>().position;
        text_english= gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();

        move_time = timeMultiplier;

        if (useThisText)
        {
            textToUse = GetComponent<Text>();
        }
        if (fadeOnStart)
        {
            if (fadeIn)
            {
                StartCoroutine(FadeInText(timeMultiplier, textToUse));
                StartCoroutine(FadeInText(timeMultiplier, text_english));

                StartCoroutine(MoveObject(start, middle,move_time));
            }
            else
            {
                StartCoroutine(FadeOutText(timeMultiplier, textToUse));
                StartCoroutine(FadeOutText(timeMultiplier, text_english));

                StartCoroutine(MoveObject(middle,end , move_time));
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2&&A)
        {
            StartCoroutine(FadeOutText(timeMultiplier, textToUse));
            StartCoroutine(FadeOutText(timeMultiplier, text_english));

            StartCoroutine(MoveObject(middle, end, move_time));
            A = false;
        }
    }

    private IEnumerator MoveObject(Vector2 startPos, Vector2 endPos, float time)
    {
        var dur = 0.0f;
        while (dur <= time)
        {
            dur += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, endPos, dur / time);
            yield return null;
        }
    }



    private IEnumerator FadeInText(float timeSpeed, Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime * timeSpeed));
            yield return null;
        }

        
    }

    private IEnumerator FadeOutText(float timeSpeed, Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }

    public void FadeInText(float timeSpeed = -1.0f)
    {
        if (timeSpeed <= 0.0f)
        {
            timeSpeed = timeMultiplier;
        }
        StartCoroutine(FadeInText(timeSpeed, textToUse));
    }

    public void FadeOutText(float timeSpeed = -1.0f)
    {
        if (timeSpeed <= 0.0f)
        {
            timeSpeed = timeMultiplier;
        }
        StartCoroutine(FadeOutText(timeSpeed, textToUse));
    }

}
