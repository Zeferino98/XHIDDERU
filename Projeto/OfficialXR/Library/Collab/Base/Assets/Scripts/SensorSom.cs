using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSom : MonoBehaviour {

	public SphereCollider sensorSom;
	private NIBBAIA nibba;
	private PlayerController player;
	public GameObject inimigo;

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
		if (player.walking == false || player.crouching == true) {
			sensorSom.enabled = false;
			sensorSom.radius = 3;
		} else if (player.walking == true && player.crouching == false && player.running == false) {
			sensorSom.enabled = true;
			sensorSom.radius = 3;
		} else if (player.walking == true && player.crouching == false && player.running == true) {
			sensorSom.enabled = true;
			sensorSom.radius = 6;
		}

		if (sensorSom.enabled == false) {
			nibba.ouvindo = false;
		}
	}

	void OnTriggerEnter (Collider collisionInfo){
		if (collisionInfo.gameObject.tag == "Inimigo"){
			inimigo = collisionInfo.gameObject;
			nibba = inimigo.GetComponent<NIBBAIA> ();
			nibba.ouvindo = true;
		}
	}

	void OnTriggerExit (Collider collisionInfo){
		nibba.ouvindo = false;
	}
}

