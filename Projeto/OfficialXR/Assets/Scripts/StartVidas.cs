using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVidas : MonoBehaviour {

	private PlayerController sPlayer;

	static bool firstTry = true;
	public GameObject vida1;
	public GameObject vida2;
	public GameObject vida3;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (true);
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
	}

	// Update is called once per frame
	void Update () {

		if (sPlayer.nVidas == 3) {
			firstTry = true;
		}

		if (firstTry) {
			StartCoroutine (mostrarVidas ());
		}

		if (!firstTry) {
			if (sPlayer.emSpawn == false) {
				gameObject.SetActive (false);
			}
		}

		if (sPlayer.nVidas == 3) {
			vida1.SetActive (true);
			vida2.SetActive (true);
			vida3.SetActive (true);
		} else if (sPlayer.nVidas == 2) {
			vida1.SetActive (true);
			vida2.SetActive (true);
			vida3.SetActive (false);
		} else if (sPlayer.nVidas == 1) {
			vida1.SetActive (true);
			vida2.SetActive (false);
			vida3.SetActive (false);
		}
	}

	IEnumerator mostrarVidas(){
		yield return new WaitForSeconds (3);
		firstTry = false;
		gameObject.SetActive(false);
	}
}
