using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtectPointText : MonoBehaviour
{
    private BattleManager BM;
    public Text Text;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("BattleManager"))
        {
            BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }
       Text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BM.Crack_Progress>0)
        {
            Text.text = BM.Crack_Progress + "保护据点↓";
        }
        else
        {
            Text.text ="保护据点↓";
        }
        
    }
}
