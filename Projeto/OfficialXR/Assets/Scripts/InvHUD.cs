using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvHUD : MonoBehaviour {

	private SpriteRenderer render;
	private PlayerController sPlayer;
	private Transform playerPos;
	public Sprite inv0;
	public Sprite inv1;
	public Sprite inv2;
	public Sprite inv3;
	public Sprite inv4;
	public float offsetX = 0.54f;
	public float offsetY = 1.6f;
	private float forca;

	public GameObject baitHUD;

	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
		playerPos = baitHUD.GetComponent<Transform> ();
		render.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform.position, Vector3.up);
		transform.position = new Vector3 (playerPos.transform.position.x + offsetX, playerPos.transform.position.y + offsetY, playerPos.transform.position.z);

		forca = Input.GetAxis ("Forca");

		if (Input.GetButton ("ForcaMouse")){
			forca = 1f;
		}

		if (forca > 0) {
			render.enabled = true;
		} else {
			render.enabled = false;
		}

		if (sPlayer.cooldownHabilidade <= 0) {
			render.sprite = inv4;
		} else if (sPlayer.cooldownHabilidade <= 15 && sPlayer.cooldownHabilidade > 0) {
			render.sprite = inv3;
		} else if (sPlayer.cooldownHabilidade > 15 && sPlayer.cooldownHabilidade <= 30) {
			render.sprite = inv2;
		} else if (sPlayer.cooldownHabilidade > 30 && sPlayer.cooldownHabilidade <= 45) {
			render.sprite = inv1;
		} else if (sPlayer.cooldownHabilidade > 45 && sPlayer.cooldownHabilidade <= 60) {
			render.sprite = inv0;
		}
	}
}
