using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitPickup : MonoBehaviour {

	public float rotX = 30f;
	public float rotY = 50f;

	private LancarBait sLancarBait;
	private PlayerController sPlayer;
	public GameObject lancadorBait;
	public GameObject pickupSystem;
	public GameObject baitHUD;
	private BaitGUI baitGUI;
	private ParticleSystem baitEffect;

	// Use this for initialization
	void Start () {
		sLancarBait = lancadorBait.GetComponent<LancarBait> ();
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
		baitHUD = GameObject.Find ("Cargas");
		baitGUI = baitHUD.GetComponent<BaitGUI> ();
		baitEffect = baitHUD.GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (rotX * Time.deltaTime, rotY * Time.deltaTime, 0);
	}

	void OnTriggerStay (Collider colisao){
		if (colisao.tag == "Player") {
			if (Input.GetButtonDown ("Interagir")) {
				baitGUI.pegouBait = true;
				if (sLancarBait.contadorBait < 4) {
					baitEffect.Play ();
					sPlayer.podePegar = true;
					sLancarBait.contadorBait++;
					GameObject particula = (GameObject)Instantiate (pickupSystem, transform.position, transform.rotation);
					Destroy (gameObject);
				}
			}
		}
	}
}
