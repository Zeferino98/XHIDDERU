using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndStage : MonoBehaviour {

	private PlayerController sPlayer;

	public AudioClip endStageSound;

	public bool fimFase;

	// Use this for initialization
	void Start () {
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider collider){
		if (collider.tag == ("Player")){
			fimFase = true;
			sPlayer.funcionar = false;
			sPlayer.dancar = true;
			GetComponent<AudioSource> ().PlayOneShot (endStageSound);
			GetComponentInChildren<ParticleSystem> ().Play();
			StartCoroutine (delayFim ());
		}
	}

	IEnumerator delayFim(){
		yield return new WaitForSeconds (11);
		SceneManager.LoadScene ("Credits");
		yield break;
	}

}
