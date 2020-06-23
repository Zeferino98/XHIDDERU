using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visores : MonoBehaviour {

	public float distancia = 30f;

	private PlayerController sPlayer;
	private NIBBAIA sNibba;
	private Color cor;
	public GameObject[] visores;
	public bool visorAtivo = false;

	// Use this for initialization
	void Start () {
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController>();
		sNibba = GetComponentInParent<NIBBAIA> ();
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Vector3 forward = transform.TransformDirection (Vector3.forward) * distancia;

		if (Physics.Raycast (new Vector3 (transform.position.x, transform.position.y, transform.position.z), transform.forward, out hit, distancia)) {
			if (hit.collider.gameObject.tag == "Player" && sPlayer.invisivel == false) {
				sNibba.perderVista = false;
				sNibba.vendo = true;
				cor = Color.green;
				visorAtivo = true;
			} else if (visores [0].GetComponent<Visores> ().visorAtivo == false && visores [1].GetComponent<Visores> ().visorAtivo == false && visores [2].GetComponent<Visores> ().visorAtivo == false && visores [3].GetComponent<Visores> ().visorAtivo == false && visores [4].GetComponent<Visores> ().visorAtivo == false) {
				cor = Color.red;
				sNibba.vendo = false;
			} else {
				cor = Color.red;
				visorAtivo = false;
			}

			Debug.Log(hit.collider.gameObject.tag);
			Debug.DrawRay (transform.position, forward, cor);
		} else if (sNibba.vendo == false) {
			cor = Color.red;
			Debug.DrawRay (transform.position, forward, cor);
			sNibba.vendo = false;
		}
	}
}
