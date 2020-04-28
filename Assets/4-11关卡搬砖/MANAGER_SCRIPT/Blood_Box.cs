using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blood_Box : MonoBehaviour
{

    private Actor actor;
    GameObject player_collider;

    private const string Prefabs = "Prefabs/Treasure_Prefab/";

    public bool Finish=false;
    private GameObject treasure;

    private Material A;
    private float effect_num = 0;
    private float timer = 0;
    private float rand_rotate;
    public GameObject costimage;
    // Start is called before the first frame update
    void Start()
    {
        rand_rotate = Random.Range(0, 361);
        transform.Rotate(0, rand_rotate,0);

        costimage= gameObject.transform.GetChild(0).gameObject;

        if (GameObject.Find("Actor"))
        {
            actor = GameObject.Find("Actor").GetComponent<Actor>();
            player_collider = GameObject.Find("Actor");
        }
        
        A = GetComponent<Renderer>().material;
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
        if (actor.heal <= actor.maxHeal*0.25f)
        {
            Debug.Log("您的生命值不够");
        }
        else
        {
            actor.heal -= actor.maxHeal * 0.25f;
            treasure = Resources.Load(Prefabs + "Weapon_Treasure") as GameObject;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);

            Instantiate(treasure, pos, Quaternion.identity);
            Finish = true;
            GetComponent<BoxCollider>().enabled = false;
            costimage.SetActive(false);
            StartCoroutine(Destroy_This(2f));
        }
    }

    IEnumerator Destroy_This(float duration)
    {
        yield return new WaitForSeconds(duration);
        DestroyImmediate(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
      if(other.gameObject == player_collider)
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
