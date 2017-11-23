using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class GyroCorrectionTest : MonoBehaviour {

	private float alpha, beta, gamma;

	[DllImport("__Internal")]
	private static extern void SetupGyroscope();

	// Use this for initialization
	void Start () {


		SetupGyroscope ();

		alpha = 0;
		beta = 0;
		gamma = 0;
	}
	
	public void setAlpha(int a){
		alpha = a;
	}

	public void setBeta(int b){
		beta = b;
	}

	public void setGamma(int g){
		gamma = g;
	}
		
	void FixedUpdate () {
		//iPhone, standing
		//gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(-beta, -gamma, alpha)));

		//iPhone, horizontal
		gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(gamma, -beta, alpha)));

	}
}
