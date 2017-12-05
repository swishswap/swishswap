using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	Animator anim;
	int intHash = Animator.StringToHash("tInt");
	int idleHash = Animator.StringToHash("Base Layer.IdleBox");
	int openHash = Animator.StringToHash("Base Layer.OpenBox");
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (Input.GetKeyDown (KeyCode.Space) && stateInfo.nameHash == idleHash) {
			anim.SetInteger (intHash, 1);
		}
		if (Input.GetKeyDown (KeyCode.Space) && stateInfo.nameHash == openHash) {
			anim.SetInteger (intHash, 2);
		}
	}
}
