using UnityEngine;
using System.Collections;

public class SwipeColor : MonoBehaviour 
{
	public Material [] materials = new Material[3];
	private Renderer rend;
	Vector2 touchBegin;
	Vector2 touchEnd;
	Vector2 swipeVector;
	int index;

	void Start(){
		rend = GetComponent<Renderer> ();
		index = 0;
	}

	void Update() 
	{
		Swipe ();
	}

	public void Swipe()
	{
		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				touchBegin = new Vector2(t.position.x,t.position.y);
			}
			if(t.phase == TouchPhase.Ended)
			{
				touchEnd = new Vector2(t.position.x,t.position.y);

				swipeVector = new Vector2(touchEnd.x - touchBegin.x, touchEnd.y - touchBegin.y);

				swipeVector.Normalize();

				//Up swipe
				/*if(swipeVector.y > 0 && swipeVector.x > -0.5f && swipeVector.x < 0.5f)
				{
					rend.material = materials[0];
				}
				//Down swipe
				if(swipeVector.y < 0 && swipeVector.x > -0.5f && swipeVector.x < 0.5f)
				{
					rend.material = materials[1];
				}*/

				//Left swipe
				if(swipeVector.x < 0 && swipeVector.y > -0.5f && swipeVector.y < 0.5f)
				{
					index++;
					if (index > 2) {
						index = 0;
					}
					rend.material = materials[index];
				}
				//Right swipe
				if(swipeVector.x > 0 && swipeVector.y > -0.5f && swipeVector.y < 0.5f)
				{
					index--;
					if (index < 0) {
						index = 2;
					}
					rend.material = materials[index];
				}
			}
		}
	}
}