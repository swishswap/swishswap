using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeServer : MonoBehaviour {

	private List<Client> clients;

	private int hostID;
	private int channelID;

	// Use this for initialization
	void Start () {

		clients = new List<Client> ();

		NetworkTransport.Init ();
		NetworkServer.Reset ();

		ConnectionConfig config = new ConnectionConfig ();
		config.ConnectTimeout = 500;
		channelID = config.AddChannel (QosType.ReliableSequenced);
		HostTopology topology = new HostTopology (config, 20);

		hostID = NetworkTransport.AddWebsocketHost (topology, 1234);
		//hostID = NetworkTransport.AddHost (topology, 1234);
		Debug.Log ("Server is open.");

	}

	float alpha;
	float beta;
	float gamma;

	void OnGUI() {
		GUILayout.Label ("Alpha: " + alpha);
		GUILayout.Label ("Beta: " + beta);
		GUILayout.Label ("Gamma: " + gamma);
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

			switch (eventType) {

			case NetworkEventType.ConnectEvent:
				Debug.Log ("NEW CLIENT CONNECTED!");
				clients.Add (new Client(connection));
				break;

			case NetworkEventType.DataEvent:
				//Debug.Log ("Data recieved: " + buffer [0]);
				//For a standing phone -beta, -gamma, alpha
				alpha = ((buffer [0] << 8) | buffer [1]) / 10f;
				beta = ((buffer [2] << 8) | buffer [3]) / 10f;
				gamma = ((buffer [4] << 8) | buffer [5]) / 10f;

				if (beta > 180) {
					beta -= 360;
				}
				if (gamma > 180) {
					gamma -= 360;
				}


				float rotX = -beta;
				float rotY = -alpha;
				float rotZ = gamma;


				bool touching = buffer [6] == 1;

				Debug.Log (alpha + ", " + beta + ", " + gamma + ", touching=" + touching);

				Client c = getClient (connection);

				if (touching) {
					c.calibrate (rotX, rotY, rotZ);
				}

				//For a standing phone -beta, -gamma, alpha
				gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), c.correct(rotX, rotY, rotZ));
				//For a horizontal phone gamma, -beta, alpha
				//gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(buffer[2], -buffer[1], buffer[0])));
				break;

			case NetworkEventType.DisconnectEvent:
				Debug.Log ("Client disconnected");
				clients.Remove (getClient(connection));
				break;
			}
		}

	}


	private Client getClient(int connectionID){
		for (int i = 0; i < clients.Count; i++) {
			Client client = clients [i];
			if (client.getConnectionID () == connectionID){
				return client;
			}
		}
		return null;
	}

	private class Client {

		private int connectionID;
		private float calX, calY, calZ;

		public Client(int connectionID) {
			this.connectionID = connectionID;
		}

		public void calibrate(float rotX, float rotY, float rotZ){
			//calibration = Quaternion.Inverse(Quaternion.Euler (rotX, rotY, rotZ));
			calX = rotX;
			calY = rotY;
			calZ = rotZ;
		}

		public Quaternion correct(float rotX, float rotY, float rotZ){
			return Quaternion.Euler (rotX - calX, rotY - calY, rotZ - calZ);
		}

		public int getConnectionID(){
			return connectionID;
		}
			
	}
}
