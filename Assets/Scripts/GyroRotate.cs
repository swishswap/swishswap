 using UnityEngine;
 using UnityEngine.iOS;
 using System.Collections;
 public class GyroRotate : MonoBehaviour {
     
     void Start () 
     {
         Input.gyro.enabled = true;
     }
 
     void Update () 
     {
         transform.Rotate (Input.gyro.rotationRateUnbiased.x, Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
         //Om koden ovan inte funkar, testa koden nedan:
         //transform.eulerAngles = new Vector3 (Input.gyro.rotationRateUnbiased.x, Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
      
         //Eller koden nedan:
         //transform.rotation = Input.gyro.attitude;
     }
 }
