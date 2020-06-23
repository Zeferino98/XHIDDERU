using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour {

	public SphereCollider barulho;
	private NIBBAIA nibba;
	public GameObject inimigo;
	public AudioClip hitBait;

	bool primeiroToque;

	// Use this for initialization
	void Start () {
		primeiroToque = true;
		inimigo = GameObject.FindGameObjectWithTag ("Inimigo");
		barulho.enabled = false;
	}

	void OnCollisionEnter (Collision collision){
		if (collision.collider.tag != ("Player")) {
			if (!primeiroToque) {
				barulho.enabled = false;
			}
			if (primeiroToque) {
				GetComponent <AudioSource> ().PlayOneShot (hitBait);
				Debug.Log ("Bait");
				Destroy (gameObject, 5);
				barulho.enabled = true;
			}
			primeiroToque = false;
		}
	}

	void OnTriggerEnter (Collider collider){
		if (collider.tag == ("Inimigo")){
			Debug.Log ("Baitado");
			inimigo = collider.gameObject;
			nibba = inimigo.GetComponent<NIBBAIA> ();
			nibba.lastPos = new Vector3 (transform.position.x, 0, transform.position.z);
			nibba.baitado = true;
			barulho.enabled = false;
		}
	}
}
