using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class S_Loading_System : MonoBehaviour
{
    public GameObject os;
    public string[] v_rs;
    public Text text;

    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        text.text = v_rs[(int)Random.Range(0, v_rs.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            text.text = v_rs[(int)Random.Range(0, v_rs.Length)];
        }

        os.transform.Rotate(Vector3.up * Time.deltaTime * speed);
        os.transform.Rotate(Vector3.left * speed * Time.deltaTime);
    }
}
