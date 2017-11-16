 using UnityEngine;
 using System.Collections;
 public class GyroRotate : MonoBehaviour {
     
     void Start () 
     {
         Input.gyro.enabled = true;
     }
 
     void Update () 
     {
         transform.Rotate (Input.gyro.rotationRateUnbiased.x, Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
     }
 }
