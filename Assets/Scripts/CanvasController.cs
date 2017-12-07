using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

	// The object of interest.
	public GameObject interestingObject;

	// The text object which describes the interesting object.
	public GameObject descriptiveText;

	// A UI line.
	public GameObject lineGraphics;

	// The camera.
	public Camera theCamera;

	// Determines the minimum angle (at which to start sgowing the graphics) from the object of interest to the camera.
	private float limit = 75f;

	// The highlight graphics.
	public GameObject highlightGraphics;

	// The color of the highlight graphics.
	private Color32 color;
	private Color32 color_t;

	// The screen space position the line is supposed to point to.
	private Vector3 to;

	// Use this for initialization
	void Start () {
		color = Color.white;
		color_t = Color.white * new Color32 (16, 16, 16, 0);
		to = descriptiveText.GetComponent<RectTransform> ().position - new Vector3 (descriptiveText.GetComponent<RectTransform> ().sizeDelta.x *2.25f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		// Get the angle from the interesting object to the camera.
		float angle = Vector3.Angle(interestingObject.transform.forward, theCamera.transform.forward * -1.0f);

		if(angle < limit) {

			// Get the screen position of the interesting object.
			Vector3 screenposition = theCamera.WorldToScreenPoint (interestingObject.transform.position);

			// Determine how opaque the graphics should be.
			float norm =  angle / limit;
			Color lerpedcolor = Color32.Lerp (color, color_t, norm);

			// Change the opacity of the highlight graphics.
			highlightGraphics.GetComponent<Image> ().color = lerpedcolor;

			//Position the highlight graphics.
			highlightGraphics.transform.GetComponent<RectTransform> ().position = new Vector2 (screenposition.x, screenposition.y);



			// Position the line image so that the line is pointing from the interesting object to the text position.
			RectTransform imageRectTransform = lineGraphics.GetComponent<Image>().rectTransform;
			Vector3 differenceVector = to - screenposition;

			imageRectTransform.sizeDelta = new Vector2( differenceVector.magnitude, 5f);
			imageRectTransform.pivot = new Vector2(0, 0.5f);
			imageRectTransform.position = screenposition;
			float screenangle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
			imageRectTransform.rotation = Quaternion.Euler(0,0, screenangle);

			// Change the opacity of the line.
			imageRectTransform.GetComponent<Image> ().color = Color.Lerp (color, color_t, norm);

			// Change the opacity of the text.
			descriptiveText.transform.GetComponent<Text>().color = lerpedcolor;

		}

		else {
			highlightGraphics.GetComponent<Image> ().color = color_t;
		}
	}

}
