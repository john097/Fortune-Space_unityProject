using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana_Move : MonoBehaviour
{
	private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
		anim=this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		float v=Input.GetAxis("Vertical");
		float h=Input.GetAxis("Horizontal");
		anim.SetFloat("Speed",v);
		if(v!=0.0f){
			anim.SetBool("Running",true);
			anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 1);
			anim.SetLayerWeight(anim.GetLayerIndex("StandingAttack"), 0);
		}
		else{
			anim.SetBool("Running",false);
			anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 0);
			anim.SetLayerWeight(anim.GetLayerIndex("StandingAttack"), 1);
		}
		if(Input.GetMouseButtonDown(0)){
			anim.SetBool("Attacking",true);
		}else{
			anim.SetBool("Attacking",false);
		}
    }
}
