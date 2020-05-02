using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle_Lv_Bar_Move : MonoBehaviour
{
    private Vector2 start;
    private Vector2 end;
    private Text Level_Text;
    private BattleManager BM;
    private float move_time;
    private float timeMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        start = gameObject.GetComponent<RectTransform>().position;
        end = GameObject.Find("Battle_Lv_right_pos").GetComponent<RectTransform>().position;
        Level_Text = gameObject.transform.GetChild(0).GetComponent<Text>();
        if (GameObject.Find("BattleManager"))
        {
            BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }
        move_time = 450f;
        StartCoroutine(MoveObject(start, end, move_time));
    }

    // Update is called once per frame
    void Update()
    {
        Level_Text.text="" + BM.Game_Level;
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
}
