using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTextScript : MonoBehaviour {

	Animator anim;
	int downHash = Animator.StringToHash("Down");
	int snowUpHash = Animator.StringToHash("Base Layer.SymbolUp");
	int snowDownHash = Animator.StringToHash("Base Layer.SymbolDown");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {

		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		//Change to touch
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == snowUpHash){
			anim.SetBool (downHash, true);
		}

		if(stateInfo.nameHash == snowDownHash){
			StartCoroutine(snowUp());
		}
			
	}

	IEnumerator snowUp()
	{
		yield return new WaitForSeconds(5);
		anim.SetBool (downHash, false);
	}
}
