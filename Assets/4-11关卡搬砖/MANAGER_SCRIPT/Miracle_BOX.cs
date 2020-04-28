using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miracle_BOX : MonoBehaviour
{
    private Credit actor;
    private const string Prefabs = "Prefabs/Treasure_Prefab/";
    private const string Prefabs2 = "Prefabs/Trap_prefab/";
    private const string Prefabs3 = "Prefabs/MONS_PREFAB/";
    public bool Finish = false;
    private GameObject trap;
    private GameObject treasure;
    private GameObject treasure2;
    private GameObject Monster;
    private Material A;
    private float effect_num = 0;
    private float timer = 0;
    private float rand_num;
    private float rand_rotate;
    public GameObject costimage;
    GameObject player_collider;
    // Start is called before the first frame update
    void Start()
    {
        rand_rotate = Random.Range(0, 361);
        transform.Rotate(0, rand_rotate,0);
        costimage = gameObject.transform.GetChild(0).gameObject;
        if (GameObject.Find("Actor"))
        {
            actor = GameObject.Find("Actor").GetComponent<Credit>();
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
        
         rand_num = Random.Range(0, 10);
            if (rand_num <=2)//毒雾
            {
                trap = Resources.Load(Prefabs2 + "Bt_Poison_Fog") as GameObject;
                Vector3 pos = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
                Instantiate(trap, pos, Quaternion.identity);
            Finish = true;
            GetComponent<BoxCollider>().enabled = false;
            costimage.SetActive(false);
            StartCoroutine(Destroy_This(2f));
        }
            else if(rand_num>2&&rand_num<=4)//金币
            {
                treasure2 = Resources.Load(Prefabs + "Gold_Treasure") as GameObject;
                Vector3 pos2 = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
                Instantiate(treasure2, pos2, Quaternion.identity);
            Finish = true;
            GetComponent<BoxCollider>().enabled = false;
            costimage.SetActive(false);
            StartCoroutine(Destroy_This(2f));
        }
            else if (rand_num > 4 && rand_num <= 7)//宝箱怪
            {
                Monster = Resources.Load(Prefabs3 + "MONS-BOMBER") as GameObject;
                Vector3 pos2 = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
                Instantiate(Monster, pos2, Quaternion.identity);
            Finish = true;
            GetComponent<BoxCollider>().enabled = false;
            costimage.SetActive(false);
            StartCoroutine(Destroy_This(2f));
        }
            else//武器
            {
                treasure = Resources.Load(Prefabs + "Weapon_Treasure") as GameObject;
                Vector3 pos2 = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
                Instantiate(treasure, pos2, Quaternion.identity);
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
