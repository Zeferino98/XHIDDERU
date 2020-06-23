using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancarBait : MonoBehaviour {

	private PlayerController sPlayer;

	public Rigidbody baitObject;
	public AudioClip throwSound;

	public float forcaLancamento;
	public float forca;
	float delayBait = 2.5f;
	float cooldownBait;
	public int contadorBait;

	// Use this for initialization
	void Start () {
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (cooldownBait > 0) {
			cooldownBait -= Time.deltaTime;
		}

		forca = Input.GetAxis ("Forca");

		if (Input.GetButton ("ForcaMouse")){
			forca = 1f;
		}
			
		if (Input.GetButtonDown ("Bait") && !Input.GetButton("Run") && cooldownBait <= 0 && contadorBait > 0 && forca > 0) {
				contadorBait--;
				cooldownBait = delayBait;
				sPlayer.jogandoBait = true;
				StartCoroutine (delayThrow ());
			}
	}

	IEnumerator delayThrow()
	{
		float lastForca = forca;
		yield return new WaitForSeconds (0.8f);
		GetComponent<AudioSource> ().PlayOneShot (throwSound);
		Rigidbody newBait = (Rigidbody)Instantiate (baitObject, transform.position, transform.rotation);
		Physics.IgnoreCollision(GameObject.FindWithTag("Player").GetComponent<CapsuleCollider>(), newBait.GetComponent<Collider>(), true);
		newBait.name = "bait";
		newBait.GetComponent<Rigidbody>().velocity = transform.TransformDirection(0, 0, forcaLancamento * lastForca);
		yield break;
	}
}
