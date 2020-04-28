using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold_Box : MonoBehaviour
{
    public Text Cost_Text;
    public int ticket;
    private Credit actor;
    private const string Prefabs = "Prefabs/Treasure_Prefab/";

    public bool Finish = false;
    private GameObject treasure;
    private GameObject treasure2;
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
        switch (PlayerPrefs.GetInt("Current_State"))
        {
            case 1:
                ticket = Random.Range(50, 101);
                break;
            case 2:
                ticket = Random.Range(80, 131);
                break;
            case 3:
                ticket = Random.Range(100, 151);
                break;


        }
        Cost_Text.text = "$" + ticket;
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
        if (actor.playerCredit < ticket)
        {
            Debug.Log("您的金币不够");
        }
        else
        {
            actor.playerCredit -= ticket;
            rand_num = Random.Range(0, 10);
            if (rand_num >= 5)
            {
                treasure = Resources.Load(Prefabs + "Heal_Treasure") as GameObject;
                Vector3 pos = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
                Instantiate(treasure, pos, Quaternion.identity);
            }
            else
            {
                treasure2 = Resources.Load(Prefabs + "Weapon_Treasure") as GameObject;
                Vector3 pos2 = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
                Instantiate(treasure2, pos2, Quaternion.identity);
            }
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
