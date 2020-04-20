using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimIconFollowMouse : MonoBehaviour
{
    public Camera camera1;
    public Vector3 v3;

    public float recoverSpeed;
    private Renderer thisRenderer;
    private float[] f;
    private Color[] c;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        thisRenderer = gameObject.GetComponent<Renderer>();
        f = new float[2];
        c = new Color[2];
        f[0] = thisRenderer.material.GetFloat("_AimIConSize");
        c[0] = thisRenderer.material.GetColor("_AimColor");
        f[1] = f[0];
        c[1] = c[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            ShootingIconAnim(0.4f,Color.red);
        }

        v3.x = camera1.ScreenToWorldPoint(Input.mousePosition).x;
        v3.y = camera1.ScreenToWorldPoint(Input.mousePosition).y;
        transform.position = v3;

        thisRenderer.material.SetFloat("_AimIConSize",Mathf.Lerp(f[1],f[0],recoverSpeed*Time.deltaTime));
        thisRenderer.material.SetColor("_AimColor",Color.Lerp(c[1],c[0], recoverSpeed * Time.deltaTime));

        f[1] = thisRenderer.material.GetFloat("_AimIConSize");
        c[1] = thisRenderer.material.GetColor("_AimColor");
    }


    public void ShootingIconAnim(float sf,Color sc)
    {
        f[1] = sf;
        c[1] = sc;
    }
}
