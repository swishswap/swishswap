using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class CubeClient : MonoBehaviour {

	private int connectionID;

	private int hostID;
	private int channelID;

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

		byte error;
		connectionID = NetworkTransport.Connect  (hostID, "130.229.147.21", 1234, 0, out error);
		NetworkError e = (NetworkError)error;


		SetupGyroscope ();

		alpha = 0;
		beta = 0;
		gamma = 0;
	}
	
	public void setAlpha(int a){
		alpha = a;
	}

	public void setBeta(int b){
		beta = b;
	}

	public void setGamma(int g){
		gamma = g;
	}

	private const int BUFFER_SIZE = 32*1024;
	private byte[] buffer = new byte[BUFFER_SIZE];

	void FixedUpdate () {

		byte error;

		buffer [0] = (byte)alpha;
		buffer [1] = (byte)beta;
		buffer [2] = (byte)gamma;
		NetworkTransport.Send (hostID, connectionID, channelID, buffer, 3, out error);

		while (true) {
			int host;
			int connection;
			int channel;
			int dataSize;

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
			case NetworkEventType.DisconnectEvent:
				Debug.Log ("Disconnected from server.");
				break;
			}
		}


	}
}
