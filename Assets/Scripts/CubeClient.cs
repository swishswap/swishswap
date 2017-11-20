using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeClient : MonoBehaviour {

	private int connectionID;

	private int hostID;
	private int channelID;

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

		byte error;
		connectionID = NetworkTransport.Connect  (hostID, "85.24.230.143", 1234, 0, out error);
		NetworkError e = (NetworkError)error;


	}


	private const int BUFFER_SIZE = 32*1024;
	private byte[] buffer = new byte[BUFFER_SIZE];

	void FixedUpdate () {

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
				break;

			case NetworkEventType.DataEvent:
				//Debug.Log ("Data recieved: " + buffer [0]);
				gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, buffer[0], 0)));
				break;
			case NetworkEventType.DisconnectEvent:
				Debug.Log ("Disconnected from server.");
				break;
			}
		}
	}
}
