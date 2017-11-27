using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeClockPointer: MonoBehaviour {

	public GameObject secondObj;
	public GameObject minuteObj;
	public GameObject hourObj;

	private Transform secondPointer;
	private Transform minutePointer;
	private Transform hourPointer;

	private double seconds;
	private double minutes;
	private double hours;

	private TimeSpan timeNow;

	private float secondDegree;
	private float minuteDegree;
	private float hourDegree;

	void Start(){

		secondDegree = 360 / 60;
		minuteDegree = 360 / 60;
		hourDegree = 360 / 12;

		secondPointer = secondObj.GetComponent<Transform>();
		minutePointer = minuteObj.GetComponent<Transform>();
		hourPointer = hourObj.GetComponent<Transform>();

	}

	void Update(){
	
		timeNow = DateTime.Now.TimeOfDay;
		//Debug.Log (timeNow);

		seconds = timeNow.TotalSeconds;
		minutes = timeNow.TotalMinutes;
		hours = timeNow.TotalHours;
		//Debug.Log (second);
		//Debug.Log (minute);
		//Debug.Log (hour);

		//minutePointer.transform.Rotate (0.0f, minute * 6, 0.0f); funkar ej för att transform.Rotate tar in vector3...

		secondPointer.localRotation = Quaternion.Euler (0f, 0f, (float)(seconds * secondDegree));
		minutePointer.localRotation = Quaternion.Euler (0f, 0f, (float)(minutes * minuteDegree));
		hourPointer.localRotation = Quaternion.Euler (0f, 0f, (float)(hours * hourDegree));

	}

}
	
