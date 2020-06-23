using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nibbaHUD : MonoBehaviour {

	private SpriteRenderer render;
	private bool jaViu;
	private bool jaPerdeu;
	private bool podeApagar = true;
	public Sprite viu;
	public Sprite perdeu;

	public AudioClip perderVisao;
	public AudioClip ver;

	private NIBBAIA sNibba;

	// Use this for initialization
	void Start () {
		sNibba = GetComponentInParent<NIBBAIA> ();
		render = GetComponent<SpriteRenderer> ();
		render.enabled = false;
		podeApagar = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform.position, Vector3.up);

		if (sNibba.patrulhando) {
			jaViu = false;
			jaPerdeu = false;
			if (podeApagar) {
				render.enabled = false;
			}
		}

		if (sNibba.funcionando == false) {
			render.enabled = false;
		}

		if (sNibba.socando == true) {
			render.enabled = false;
		}

		if (sNibba.perderVista && !jaPerdeu) {
			jaViu = false;
			if (GetComponent <AudioSource> ().isPlaying == false) {
				GetComponent <AudioSource> ().PlayOneShot (perderVisao);
			}
			StartCoroutine (perdeuEnable ());
			jaPerdeu = true;

		}

		if ((sNibba.vendo || sNibba.baitado) && !jaViu && sNibba.desmaiado == false) {
			jaPerdeu = false;
			if (GetComponent <AudioSource> ().isPlaying == false) {
				GetComponent <AudioSource> ().PlayOneShot (ver);
			}
			StartCoroutine (viuEnable ());
			jaViu = true;
		}

	}

	IEnumerator perdeuEnable(){
		render.sprite = perdeu;
		render.enabled = true;
		podeApagar = false;
		yield return new WaitForSeconds (3);
		if (podeApagar) {
			render.enabled = false;
		}
		podeApagar = true;
		yield break;
	}

	IEnumerator viuEnable(){
		render.sprite = viu;
		render.enabled = true;
		podeApagar = false;
		if (sNibba.funcionando == false) {
			render.enabled = false;
		}
		yield return new WaitForSeconds (3);
		if (podeApagar) {
			render.enabled = false;
		}
		podeApagar = true;
		yield break;
	}

}
