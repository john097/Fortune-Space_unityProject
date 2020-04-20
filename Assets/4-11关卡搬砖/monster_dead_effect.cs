using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_dead_effect : MonoBehaviour
{
    private Material A;
    private float effect_num=0;
    private float timer = 0;
    private Actor mons_actor;
    // Start is called before the first frame update
    void Start()
    {
        A=GetComponent<Renderer>().material;
       mons_actor= gameObject.transform.root.gameObject.GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mons_actor.isAlive)
        {
            timer += Time.deltaTime;
            if (timer > 4f)
            {
                timer = 4f;
            }
            effect_num = Mathf.Lerp(0, 1, timer);
            A.SetFloat("_Alpha_Dis", effect_num);
        }
        
    }

}
