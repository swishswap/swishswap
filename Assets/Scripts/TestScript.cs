using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TestScript : MonoBehaviour {

	[DllImport("__Internal")]
	private static extern int TheGreatestFunctionEver ();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (TheGreatestFunctionEver());
	}
}
