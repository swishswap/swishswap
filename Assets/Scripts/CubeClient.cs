using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class CubeClient : MonoBehaviour {

	private int connectionID;

	private int hostID;
	private int channelID;

	private bool connecting;
	private bool connected;
	private int attempt;

	private float alpha, beta, gamma;

	Vector2 touchBegin;
	Vector2 touchEnd;
	Vector2 swipeVector;

	Animator anim;
	//parameters
	int sceneHash = Animator.StringToHash("Scene");
	//animations
	int camOneHash = Animator.StringToHash("Base Layer.MainCamera1");
	int camTwoHash = Animator.StringToHash("Base Layer.MainCamera2");
	int camFourHash = Animator.StringToHash("Base Layer.MainCamera4");
	int camCalHash = Animator.StringToHash("Base Layer.MainCameraCal");

	[DllImport("__Internal")]
	private static extern void SetupGyroscope();

	[DllImport("__Internal")]
	private static extern int ALittlePig ();

	// Use this for initialization
	void Start () {


		NetworkTransport.Init ();
		NetworkServer.Reset ();

		ConnectionConfig config = new ConnectionConfig ();
		config.ConnectTimeout = 500;
		channelID = config.AddChannel (QosType.ReliableSequenced);
		HostTopology topology = new HostTopology (config, 20);

		hostID = NetworkTransport.AddHost (topology, 54321, null);

		Debug.Log ("Client initialized, attempting to connect.");

		connecting = false;
		connected = false;

		#if UNITY_WEBGL
			gameObject.transform.SetPositionAndRotation (new Vector3 (0, 0, 0), Quaternion.Euler (0, 45, 0));
			SetupGyroscope ();
			//ALittlePig();
		#endif

		alpha = 0;
		beta = 0;
		gamma = 0;

		anim = GetComponent<Animator>();
	}
	
	public void setAlpha(int a){
		alpha = a;
		if (alpha < 0) {
			alpha += 3600;
		}
	}

	public void setBeta(int b){
		beta = b;
		if (beta < 0) {
			beta += 3600;
		}
	}

	public void setGamma(int g){
		gamma = g;
		if (gamma < 0) {
			gamma += 3600;
		}
	}

	private const int BUFFER_SIZE = 32*1024;
	private byte[] buffer = new byte[BUFFER_SIZE];

	void OnGUI() {
		GUILayout.Label ("Connecting: " + connecting);
		GUILayout.Label ("Connected: " + connected);
		GUILayout.Label ("Attempt: " + attempt);

		GUILayout.Label ("Touch Begin: " + touchBegin);
		GUILayout.Label ("Touch End: " + touchEnd);
		GUILayout.Label ("Swipe vector: " + swipeVector);

		GUILayout.Label ("Alpha: " + alpha);
		GUILayout.Label ("Beta: " + beta);
		GUILayout.Label ("Gamma: " + gamma);


	}

	void FixedUpdate () {

		/*alpha += 3f;
		beta += 6f;
		gamma += 9f;*/

		if (!connected && !connecting) {
			byte error;
			connectionID = NetworkTransport.Connect(hostID, "130.229.131.44", 1234, 0, out error);
			connecting = true;
			attempt++;
			Debug.Log ("Attempting to connect.");
		}

		if (connected) {

			int swipeDirection = Swipe ();

			bool shouldCalibrate = false;

			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

			//Change to touchinput

			if(swipeDirection == 5 && stateInfo.nameHash == camCalHash){
				shouldCalibrate = true;
				anim.SetInteger (sceneHash, 1);
			}

			//Change to swipeinput (up)
			if(swipeDirection == 3 && stateInfo.nameHash == camOneHash){
				anim.SetInteger (sceneHash, 2);
			}

			//Change to tiltinput
			if(swipeDirection == 4 && stateInfo.nameHash == camTwoHash){
				anim.SetInteger (sceneHash, 3);
				StartCoroutine(coolToColor());
			}

			//Change to swipeinput (left/right)
			if((swipeDirection == 1 || swipeDirection == 2) && stateInfo.nameHash == camFourHash){
				anim.SetInteger (sceneHash, 5);
			}

			byte error;

			buffer [0] = (byte)((int)alpha >> 8);
			buffer [1] = (byte)alpha;

			buffer [2] = (byte)((int)beta >> 8);
			buffer [3] = (byte)beta;

			buffer [4] = (byte)((int)gamma >> 8);
			buffer [5] = (byte)gamma;


			if (shouldCalibrate) {
				buffer [6] = (byte)1;
			} else {
				buffer [6] = (byte)0;
			}

			buffer [7] = (byte)swipeDirection;

			NetworkTransport.Send (hostID, connectionID, channelID, buffer, 8, out error);
		}

		while (true) {
			int host;
			int connection;
			int channel;
			int dataSize;

			byte error;

			NetworkEventType eventType = NetworkTransport.Receive (out host, out connection, out channel, buffer, BUFFER_SIZE, out dataSize, out error);

			if (error != 0) {
				Debug.Log ((NetworkError)error);
			}

			if (eventType == NetworkEventType.Nothing) {
				break;
			}

			Debug.Log (eventType);
			switch (eventType) {

			case NetworkEventType.ConnectEvent:
				Debug.Log ("Successfully connected to server.");
				connecting = false;
				connected = true;
				break;
			case NetworkEventType.DisconnectEvent:
				Debug.Log ("Disconnected from server.");
				connecting = false;
				connected = false;
				break;
			}
		}


	}

	public int Swipe()
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

				float deltaX = touchEnd.x - touchBegin.x;
				float deltaY = touchEnd.y - touchBegin.y;

				swipeVector = new Vector2(deltaX, deltaY);

				swipeVector.Normalize();

				//Left swipe
				if (deltaX < -10.0f && swipeVector.x < -0.5f && swipeVector.y > -0.5f && swipeVector.y < 0.5f) {
					return 1;
				}
				//Right swipe
				else if (deltaX > 10.0f && swipeVector.x > 0.5f && swipeVector.y > -0.5f && swipeVector.y < 0.5f) {
					return 2;
				}

				//Up swipe
				else if (deltaY < -10.0f && swipeVector.y > 0.5f && swipeVector.x > -0.5f && swipeVector.x < 0.5f) {
					return 3;
				}
				//Down swipe
				else if (deltaY > 10.0f && swipeVector.y < -0.5f && swipeVector.x > -0.5f && swipeVector.x < 0.5f) {
					return 4;
				} 
				//Touch without swiping
				else {
					return 5;
				}
			}
		}
		return 0;
	}

	IEnumerator coolToColor()
	{
		yield return new WaitForSeconds(5);
		anim.SetInteger (sceneHash, 4);
	}

}
