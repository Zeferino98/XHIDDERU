using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbusto : MonoBehaviour {

	private PlayerController sPlayer;

	// Use this for initialization
	void Start () {
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
		this.gameObject.layer = 2;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerStay (Collider collisionInfo){
		Debug.Log (collisionInfo.tag);
		if (collisionInfo.tag == ("Player")) {
			sPlayer.noMato = true;
			if (sPlayer.crouching == true) {
				sPlayer.invisivel = true;
			} else if (sPlayer.usandoSkill == false) {
				sPlayer.invisivel = false;
			}
		}
	}

	void OnTriggerExit (Collider collisionInfo){
		if (collisionInfo.tag == ("Player") && sPlayer.usandoSkill == false){
			sPlayer.invisivel = false;
			sPlayer.noMato = false;
		}
	}
}
