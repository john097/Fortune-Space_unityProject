using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Box : MonoBehaviour
{
    private Actor actor;
    GameObject player_collider;

    public bool Finish = false;
    private GameObject treasure;

    private Material A;
    private float effect_num = 0;
    private float timer = 0;
    private float rand_rotate;
    public GameObject costimage;

    public int bonus_num;
    // Start is called before the first frame update
    void Start()
    {
        rand_rotate = Random.Range(0, 361);
        transform.Rotate(0, rand_rotate, 0);

        costimage = gameObject.transform.GetChild(0).gameObject;

        if (GameObject.Find("Actor"))
        {
            actor = GameObject.Find("Actor").GetComponent<Actor>();
             
            player_collider = GameObject.Find("Actor");
        }
        A = GetComponent<Renderer>().material;

        bonus_num = Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (Finish)
        {
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                timer = 2f;
            }
            effect_num = Mathf.Lerp(0, 1, timer);
            A.SetFloat("_Alpha_Dis", effect_num);

        }
    }

    public void Box_Event()
    {
        switch (bonus_num)
        {
            case 0://增加生命回复
                actor.healRecover += 2;
                Debug.Log("你的生命回复小幅度增加了");
                GetComponent<BoxCollider>().enabled = false;
                costimage.SetActive(false);
                Finish = true;
                StartCoroutine(Destroy_This(2f));
                break;
            case 1://增加攻击力
                actor.attack += 50;
                Debug.Log("你的攻击力小幅度增加了");
                GetComponent<BoxCollider>().enabled = false;
                costimage.SetActive(false);
                Finish = true;
                StartCoroutine(Destroy_This(2f));
                break;
            case 2://增加移动速度
                actor.speed += 0.2f;
                Debug.Log("你的移动速度小幅度增加了");
                GetComponent<BoxCollider>().enabled = false;
                costimage.SetActive(false);
                Finish = true;
                StartCoroutine(Destroy_This(2f));
                break;
            case 3://增加最大生命值
                actor.maxHeal += 50;
                Debug.Log("你的最大生命值小幅度增加了");
                GetComponent<BoxCollider>().enabled = false;
                costimage.SetActive(false);
                Finish = true;
                StartCoroutine(Destroy_This(2f));
                break;
        }
    }

    IEnumerator Destroy_This(float duration)
    {
        yield return new WaitForSeconds(duration);
        DestroyImmediate(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player_collider)
        {
            costimage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player_collider)
        {
            costimage.SetActive(false);
        }
    }
}
