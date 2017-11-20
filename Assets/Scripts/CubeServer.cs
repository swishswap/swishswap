using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeServer : MonoBehaviour {

	private List<int> clients;

	private int hostID;
	private int channelID;

	// Use this for initialization
	void Start () {

		clients = new List<int> ();

		NetworkTransport.Init ();
		NetworkServer.Reset ();

		ConnectionConfig config = new ConnectionConfig ();
		config.ConnectTimeout = 500;
		channelID = config.AddChannel (QosType.ReliableSequenced);
		HostTopology topology = new HostTopology (config, 20);

		hostID = NetworkTransport.AddWebsocketHost (topology, 1234);
		Debug.Log ("Server is open.");

	}


	private const int BUFFER_SIZE = 32*1024;
	private byte[] buffer = new byte[BUFFER_SIZE];

	private int iteration = 0;

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

			switch (eventType) {

			case NetworkEventType.ConnectEvent:
				Debug.Log ("NEW CLIENT CONNECTED!");
				clients.Add (connection);
				break;
			case NetworkEventType.DisconnectEvent:
				Debug.Log ("Client disconnected");
				clients.Remove (connection);
				break;
			}
		}


		buffer [0] = (byte)(iteration++);
		for (int i = 0; i < clients.Count; i++) {
			int client = clients [i];

			byte error;

			NetworkTransport.Send (hostID, client, channelID, buffer, 1, out error);
		}

	}
}
