using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

	public GameObject interesting;

	public GameObject text;

	public GameObject line;

	// The camera.
	public Camera thecamera;

	private float limit = 120f;

	// The highlight graphics.
	public GameObject highlight;

	// The color of the highlight graphics.
	private Color32 color;
	private Color32 color_t;

	// The screen space position the line is supposed to poin to.
	private Vector3 to;

	// Use this for initialization
	void Start () {
		color = highlight.transform.GetComponent<Image> ().color;
		color_t = new Color32 (color.r, color.g, color.b, 0);
		to = text.GetComponent<RectTransform> ().position;
	}
	
	// Update is called once per frame
	void Update () {
		// Get the angle from the interesting object to the camera.
		//float angle = Quaternion.Angle(this.transform.rotation, thecamera.transform.rotation);
		float angle = Vector3.Angle(interesting.transform.forward, thecamera.transform.forward);

		if(angle > limit) {

			// Get the screen position of the interesting object.
			Vector3 screenposition = thecamera.WorldToScreenPoint (interesting.transform.position);

			// Make the highlight graphics opaque.
			float diff = angle - limit;
			float norm = 1.0f- (diff / (180f - limit));
			Color lerpedcolor = Color32.Lerp (color, color_t, norm);

			highlight.GetComponent<Image> ().color = lerpedcolor;

			//Position the highlight graphics.
			highlight.transform.GetComponent<RectTransform> ().position = new Vector2 (screenposition.x, screenposition.y);



			// Position the line image so that the line is pointing from the interesting object to the predetermined position.
			RectTransform imageRectTransform = line.GetComponent<Image>().rectTransform;
			Vector3 differenceVector = to - screenposition;

			imageRectTransform.sizeDelta = new Vector2( differenceVector.magnitude, 5f);
			imageRectTransform.pivot = new Vector2(0, 0.5f);
			imageRectTransform.position = screenposition;
			float screenangle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
			imageRectTransform.rotation = Quaternion.Euler(0,0, screenangle);

			imageRectTransform.GetComponent<Image> ().color = Color.Lerp (color, color_t, norm);

			text.transform.GetComponent<Text>().color = lerpedcolor;

		}

		else {
			highlight.GetComponent<Image> ().color = new Color32 (color.r, color.g, color.b, 0);
		}
	}

}
