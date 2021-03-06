﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeServer : MonoBehaviour {

	private List<Client> clients;

	private int hostID;
	private int channelID;

	private Quaternion currentRotation;
	private Quaternion target;

	public Material [] materials;
	private int materialIndex = 0;

	public GameObject[] leather;
	private Renderer[] leatherRenderers;
	Animator anim;
	GameObject box;
	int intHash = Animator.StringToHash("tInt");
	int removeHash = Animator.StringToHash("Base Layer.RemoveBox");



	// Use this for initialization
	void Start () {
		box = GameObject.Find ("Box");
		anim = box.GetComponent<Animator>();



		clients = new List<Client> ();

		NetworkServer.Reset ();
		NetworkTransport.Init ();


		ConnectionConfig config = new ConnectionConfig ();
		config.ConnectTimeout = 500;
		channelID = config.AddChannel (QosType.ReliableSequenced);
		HostTopology topology = new HostTopology (config, 20);

		hostID = NetworkTransport.AddWebsocketHost (topology, 1234);
		//hostID = NetworkTransport.AddHost (topology, 1234);
		Debug.Log ("Server is open.");

		currentRotation = buildQuaternion (0, 0, 0);
		target = buildQuaternion(0, 0, 0);

		leatherRenderers = new Renderer[leather.Length];
		for (int i = 0; i < leather.Length; i++) {
			leatherRenderers [i] = leather [i].GetComponent<Renderer> ();
		}
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


				bool shouldCalibrate = buffer [6] == 1;
				bool swipeLeft = buffer [7] == 1;
				bool swipeRight = buffer [7] == 2;
				bool swipeUp = buffer [7] == 3;

				//Debug.Log (alpha + ", " + beta + ", " + gamma + ", shouldCalibrate=" + shouldCalibrate);

				Client c = getClient (connection);

				if (shouldCalibrate) {
					c.calibrate (alpha, beta, gamma);
					Debug.Log ("Calibrating.");
				}

				if (swipeLeft) {
					Debug.Log ("left is detected");

					materialIndex--;
					if (materialIndex < 0) {
						materialIndex = materials.Length - 1;
					}
				}

				if (swipeRight) {
					Debug.Log ("RIGHT is detected");
					materialIndex++;
					if (materialIndex >= materials.Length) {
						materialIndex = 0;
					}
				}

				if (swipeUp) {
					Debug.Log ("UP is detected");
					anim.SetInteger (intHash, 1);
				}

				for (int i = 0; i < leatherRenderers.Length; i++) {
					leatherRenderers [i].material = materials [materialIndex];
				}

				AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
				if (stateInfo.nameHash == removeHash) {
					target = c.correct (alpha, beta, gamma);
				}
				break;

			case NetworkEventType.DisconnectEvent:
				Debug.Log ("Client disconnected");
				clients.Remove (getClient (connection));
				break;
			}
		}


		if (!Input.GetKey (KeyCode.Space)) {

			float angle = Quaternion.Angle (currentRotation, target);

			float sensitivity = 0.01f;
			float v = angle * sensitivity;
			currentRotation = Quaternion.Slerp (currentRotation, target, 0.5f * v / (v + 1.0f));
		} else {
			currentRotation = target;
		}

		if (Input.GetKey (KeyCode.K)) {
			Debug.Log ("Kicking " + clients.Count + " connected clients.");
			for (int i = 0; i < clients.Count; i++) {
				byte error;
				NetworkTransport.Disconnect (hostID, clients[i].getConnectionID(), out error);
			}
			clients.Clear ();
		}

		if (clients.Count == 0) {
			anim.SetInteger (intHash, 0);
			target = buildQuaternion (90, 0, 0);
		}

		gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), currentRotation);
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

	private static Quaternion buildQuaternion(float alpha, float beta, float gamma){

		Quaternion rotAlpha = Quaternion.AngleAxis (-alpha, new Vector3 (0, 1, 0));
		Quaternion rotBeta = Quaternion.AngleAxis (-beta, new Vector3 (1, 0, 0));
		Quaternion rotGamma = Quaternion.AngleAxis (-gamma, new Vector3 (0, 0, 1));

		return rotAlpha * rotBeta * rotGamma;
	}

	private class Client {

		private int connectionID;
		private Quaternion calibration;

		public Client(int connectionID) {
			this.connectionID = connectionID;
			calibrate(0, 0, 0);
		}

		public void calibrate(float rotX, float rotY, float rotZ){
			calibration = Quaternion.Inverse(buildQuaternion(rotX, rotY, rotZ));
		}

		public Quaternion correct(float rotX, float rotY, float rotZ){
			return calibration * buildQuaternion(rotX, rotY, rotZ) * buildQuaternion(225, 180, 0);
		}

		public int getConnectionID(){
			return connectionID;
		}
			
	}
}
