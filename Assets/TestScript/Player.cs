using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController cc;
    Vector3 dir = Vector3.zero;
    public float speed = 1;
    private float tspeed;
    public float jumpspeed = 100;
    public LayerMask layerMask;
    public enum ControllType {vector,transform }
    public ControllType controlltype;
    public Animator an;

    public LineRenderer Aimline;

    // Start is called before the first frame update
    void Start()
    {
        cc = this.GetComponent<CharacterController>();
        tspeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //KeytoAnimation();
        Move();

        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            cc.Move(Vector3.up * jumpspeed);
        }

        Look();
        
    }

    private void Move()
    {
        if (Input.GetAxis("Vertical") <= -0.1)
        {
            speed = tspeed * 0.8f;
        }
        else
        {
            speed = tspeed;
        }
        switch (controlltype)
        {
            case ControllType.vector:
                dir = -Vector3.forward * Input.GetAxis("Horizontal") * speed +
            Vector3.right * Input.GetAxis("Vertical") * speed;
                break;
            case ControllType.transform:
                dir = transform.right * Input.GetAxis("Horizontal") * speed +
            transform.forward * Input.GetAxis("Vertical") * speed;
                break;
            default:
                break;
        }


        cc.SimpleMove(dir);
    }

    void Look()
    {
        Ray ray;
        RaycastHit raycastHit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 100f, layerMask))
        {
            Aimline.SetPosition(0, transform.position + Vector3.up * -1);
            Aimline.SetPosition(1, raycastHit.point);
            this.transform.LookAt(raycastHit.point, Vector3.up);
            Quaternion q = new Quaternion();
            q.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            transform.rotation = q;
        }
    }
    void KeytoAnimation()
    {
        switch (controlltype)
        {
            case ControllType.vector:
                an.SetFloat("_GoundSpeed", Vector3.Magnitude(dir) * Input.GetAxis("Vertical"));
                an.SetFloat("_VecticalSpeed", cc.velocity.y);
                an.SetBool("Gounded", cc.isGrounded);
                break;
            case ControllType.transform:
                an.SetFloat("_GoundSpeed", Vector3.Magnitude(dir) * Input.GetAxis("Vertical"));
                an.SetFloat("_VecticalSpeed", cc.velocity.y);
                an.SetBool("Gounded", cc.isGrounded);
                break;
            default:
                break;
        }
        //an.SetFloat("WALK",Mathf.Abs(Input.GetAxis("Horizontal")+Input.GetAxis("Vertical")));
        
    }
    
}
