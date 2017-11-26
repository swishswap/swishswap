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

	[DllImport("__Internal")]
	private static extern void SetupGyroscope();

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
		#endif

		alpha = 0;
		beta = 0;
		gamma = 0;
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
	}

	void FixedUpdate () {

		/*alpha += 3f;
		beta += 6f;
		gamma += 9f;*/

		if (!connected && !connecting) {
			byte error;
			connectionID = NetworkTransport.Connect(hostID, "85.24.230.143", 1234, 0, out error);
			connecting = true;
			attempt++;
			Debug.Log ("Attempting to connect.");
		}

		if (connected) {
			byte error;

			buffer [0] = (byte)((int)alpha >> 8);
			buffer [1] = (byte)alpha;

			buffer [2] = (byte)((int)beta >> 8);
			buffer [3] = (byte)beta;

			buffer [4] = (byte)((int)gamma >> 8);
			buffer [5] = (byte)gamma;


			Touch[] touches = Input.touches;
			if (touches.Length > 0) {
				buffer [6] = (byte)1;
			} else {
				buffer [6] = (byte)0;
			}

			NetworkTransport.Send (hostID, connectionID, channelID, buffer, 7, out error);
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
}
