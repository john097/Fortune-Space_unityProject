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
        os.transform.eulerAngles = new Vector3(Random.Range(0,60), Random.Range(0, 60), Random.Range(0, 60));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            text.text = v_rs[(int)Random.Range(0, v_rs.Length)];
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            os.transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
        if (Input.GetMouseButton(2))
        {
            os.transform.eulerAngles += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * 20;
        }
        
        os.transform.Rotate(Vector3.up * Time.deltaTime * speed);
        os.transform.Rotate(Vector3.left * speed * Time.deltaTime);
    }
}
