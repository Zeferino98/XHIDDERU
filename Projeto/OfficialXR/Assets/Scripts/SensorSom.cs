using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSom : MonoBehaviour {

	public SphereCollider sensorSom;
	private NIBBAIA nibba;
	private PlayerController player;
	public GameObject inimigo;
	Vector3 posPlayer;

	// Use this for initialization
	void Start () {
		
		inimigo = GameObject.FindWithTag ("Inimigo");
		sensorSom = GetComponent<SphereCollider>();
		GameObject sheik = GameObject.FindWithTag ("Player");
		nibba = inimigo.GetComponent<NIBBAIA> ();
		player = sheik.GetComponent<PlayerController> ();
	}


	// Update is called once per frame
	void Update () {

		posPlayer = player.transform.position;

		if (player.walking == false || player.crouching == true) {
			sensorSom.enabled = false;
			sensorSom.radius = 4;
		} else if (player.walking == true && player.crouching == false && player.running == false) {
			sensorSom.enabled = true;
			sensorSom.radius = 4;
		} else if (player.walking == true && player.crouching == false && player.running == true) {
			sensorSom.enabled = true;
			sensorSom.radius = 8;
		}

		if (sensorSom.enabled == false) {
			nibba.ouvindo = false;
		}
	}

	void OnTriggerEnter (Collider collisionInfo){
		if (collisionInfo.gameObject.tag == "Inimigo" && player.invisivel == false){
			inimigo = collisionInfo.gameObject;
			nibba = inimigo.GetComponent<NIBBAIA> ();
			nibba.ouvindo = true;
		}

		if (collisionInfo.gameObject.tag == "Inimigo" && player.invisivel == true) {
			nibba.lastPos = new Vector3 (posPlayer.x, 0, posPlayer.z);
			nibba.baitado = true;
		}
	}

	void OnTriggerExit (Collider collisionInfo){
		nibba.ouvindo = false;
	}
}

