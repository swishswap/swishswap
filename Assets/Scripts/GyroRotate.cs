using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;



 public class GyroRotate : MonoBehaviour {

	private float alpha, beta, gamma;
	private float x; 

	[DllImport("__Internal")]   
	private static extern void SetupGyroscope();
     
     void Start () 
     {
        // Input.gyro.enabled = true;

		SetupGyroscope ();

		alpha = 0;
		beta = 0;
		gamma = 0;

		x = 0;
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

 
     void Update () 
     {
		beta++;
		gameObject.transform.SetPositionAndRotation(new Vector3(x, 0, 0), Quaternion.Euler(new Vector3(alpha, beta, gamma)));
         //transform.Rotate (Input.gyro.rotationRateUnbiased.x, Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
         //Om koden ovan inte funkar, testa koden nedan:
         //transform.eulerAngles = new Vector3 (Input.gyro.rotationRateUnbiased.x, Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
      
         //Eller koden nedan:
        // transform.rotation = Input.gyro.attitude;
		//Debug.Log (transform.rotation);
     }
 }
