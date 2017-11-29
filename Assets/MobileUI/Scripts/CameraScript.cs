using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	Animator anim;
	//parameters
	int sceneHash = Animator.StringToHash("Scene");
	//animations
	int camOneHash = Animator.StringToHash("Base Layer.MainCamera1");
	int camTwoHash = Animator.StringToHash("Base Layer.MainCamera2");
	int camFourHash = Animator.StringToHash("Base Layer.MainCamera4");
	int camCalHash = Animator.StringToHash("Base Layer.MainCameraCal");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}


	// Update is called once per frame
	void Update () {

		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		//Change to touchinput
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == camCalHash){
			anim.SetInteger (sceneHash, 1);
		}

		//Change to swipeinput (up)
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == camOneHash){
			anim.SetInteger (sceneHash, 2);
		}

		//Change to tiltinput
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == camTwoHash){
			anim.SetInteger (sceneHash, 3);
			StartCoroutine(coolToColor());
		}

		//Change to swipeinput (left/right)
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == camFourHash){
			anim.SetInteger (sceneHash, 5);
		}
			
	}


	IEnumerator coolToColor()
	{
		yield return new WaitForSeconds(5);
		anim.SetInteger (sceneHash, 4);
	}


}
