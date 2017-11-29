using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTextScript : MonoBehaviour {

	Animator anim;
	//parameters
	int downHash = Animator.StringToHash("Down");
	//animations
	int snowUpHash = Animator.StringToHash("Base Layer.SymbolUp");
	int snowDownHash = Animator.StringToHash("Base Layer.SymbolDown");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {

		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		//Change to touchinput
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == snowUpHash){
			anim.SetBool (downHash, true);
		}

		//when snowDownHash go to IEnumerator snowup
		if(stateInfo.nameHash == snowDownHash){
			StartCoroutine(snowUp());
		}
			
	}

	IEnumerator snowUp()
	{
		yield return new WaitForSeconds(10);
		anim.SetBool (downHash, false);
	}
}
