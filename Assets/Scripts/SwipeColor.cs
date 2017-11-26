using UnityEngine;
using System.Collections;

public class SwipeColor : MonoBehaviour 
{
	private Renderer rend;
	Vector2 touchBegin;
	Vector2 touchEnd;
	Vector2 swipeVector;

	void Start(){
		rend = GetComponent<Renderer> ();
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

				swipeVector = new Vector2(touchEnd.x - touchBegin.x, touchEnd - touchBegin.y);

				swipeVector.Normalize();

				//Up swipe
				if(swipeVector.y > 0 && swipeVector.x > -0.5f && swipeVector.x < 0.5f)
				{
					rend.material.color = Color.blue;
				}
				//Down swipe
				if(swipeVector.y < 0 && swipeVector.x > -0.5f && swipeVector.x < 0.5f)
				{
					rend.material.color = Color.red;
				}
				//Left swipe
				if(swipeVector.x < 0 && swipeVector.y > -0.5f && swipeVector.y < 0.5f)
				{
					rend.material.color = Color.yellow;
				}
				//Right swipe
				if(swipeVector.x > 0 && swipeVector.y > -0.5f && swipeVector.y < 0.5f)
				{
					rend.material.color = Color.green;
				}
			}
		}
	}
}
