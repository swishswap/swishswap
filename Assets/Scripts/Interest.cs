using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interest : MonoBehaviour {

	// The camera.
	public Camera thecamera;

	// The angle from the rotation of the interesting object to the rotation of the camera.
	//private float angle = 0;

	private float limit = 120f;

	// Highlight graphics.
	public GameObject highlight;

	// The canvas.
	public GameObject canvas;

	// The instantiated highlight graphics.
	private GameObject _highlight;

	// The color of the highlight graphics.
	private Color32 color;

	private Color32 color_t;

	// The line image.
	private Image _line;

	public GameObject line;

	public GameObject text;

	// The screen space position the line is supposed to poin to.
	private Vector3 to;



	// Use this for initialization
	void Start () {
		_highlight = Instantiate (highlight) as GameObject;
		_highlight.transform.SetParent (canvas.transform, false);
		color = _highlight.transform.GetComponent<Image> ().color;

		GameObject lineobject = Instantiate (line) as GameObject;
		lineobject.transform.SetParent (canvas.transform, false);
		_line = lineobject.GetComponent<Image> ();

		//color = _line.color;
		_line.color *= new Color32 (1, 1, 1, 0);
		color_t = new Color32 (color.r, color.g, color.b, 0);
		to = text.transform.position;
	}



	// Update is called once per frame
	void Update () {

		// Get the angle from the interesting object to the camera.
		//float angle = Quaternion.Angle(this.transform.rotation, thecamera.transform.rotation);
		float angle = Vector3.Angle(this.transform.forward, thecamera.transform.forward);

		if(angle > limit) {
			//Debug.Log("interesting");

			// Get the screen position of the interesting object.
			Vector3 screenposition = thecamera.WorldToScreenPoint (this.transform.position);

			// Make the highlight graphics opaque.
			float diff = angle - limit;
			float norm = 1.0f- (diff / (180f - limit));
			Color lerpedcolor = Color32.Lerp (color, color_t, norm);

			_highlight.GetComponent<Image> ().color = lerpedcolor;

			 //Position the highlight graphics.
			_highlight.transform.GetComponent<RectTransform> ().position = new Vector2 (screenposition.x, screenposition.y);



			// Position the line image so that the line is pointing from the interesting object to the predetermined position.
			RectTransform imageRectTransform = _line.rectTransform;
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
			_highlight.GetComponent<Image> ().color = new Color32 (color.r, color.g, color.b, 0);
		}
	}
}
