using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDetection : MonoBehaviour {

	public float radius = 3;
	public float smoothTime = 0.5f;
	private PlayerController sPlayer;

	// Use this for initialization
	void Start () {
		GameObject sheik = GameObject.FindWithTag ("Player");
		sPlayer = sheik.GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
		ParticleSystem particula = GetComponent<ParticleSystem> ();
		var em = particula.emission;
		if (sPlayer.vivo == false) {
			em.enabled = false;
		} else {
			em.enabled = true;
		}
		var sh = particula.shape;
		var sp = particula.main;
		if (sPlayer.running == true && sPlayer.walking == true && sPlayer.crouching == false) {
			sp.startSize = 0.13f;
			radius = Mathf.Clamp (radius + smoothTime * Time.deltaTime, 3f, 8f);
			sh.radius = radius;
		} else if (sPlayer.walking == true && sPlayer.running == false && sPlayer.crouching == false) {
			sp.startSize = 0.13f;
			radius = Mathf.Clamp (radius + smoothTime * Time.deltaTime, 0.5f, 4f);
			sh.radius = radius;
		} else if (sPlayer.crouching == true && sPlayer.walking == true) {
			sp.startSize = 0.05f;
			radius = Mathf.Clamp (radius + smoothTime * Time.deltaTime, 0.8f, 1f);
			sh.radius = radius;
		} else {
			sp.startSize = 0.02f;
			radius = Mathf.Clamp (radius - smoothTime * Time.deltaTime, 0.5f, 8f);
			sh.radius = radius;
		}
	}
}
