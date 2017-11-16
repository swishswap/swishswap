using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour {

	//public GameObject obj;

	public Material[] materials = new Material[3];
	//public Material material1;
	//public Material material2;
	private Renderer rend;
	private Material currentMaterial;
	int index;
	private bool mousebutton;

	// Use this for initialization
	void Start () {
		
		rend = GetComponent<Renderer> ();
		currentMaterial = GetComponent<Renderer> ().material;
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//TESTA OnMouseUp eller Input.GetMouseButtonUp

		if (Input.GetMouseButtonDown(0)) {
			//Debug.Log ("A key or mouse click has been detected");

			Destroy(currentMaterial);
			rend.material = new Material(materials[index]);
			index++;
			index = index % 3;
		}
	}
}
