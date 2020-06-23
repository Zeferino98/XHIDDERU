using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitGUI : MonoBehaviour {

	private SpriteRenderer render;
	private LancarBait sLancarBait;
	public GameObject lancadorBait;
	public Transform playerPos;

	public float offsetX;
	public float offsetY;

	private float forca;

	public bool pegouBait = false;

	public Sprite bait0;
	public Sprite bait1;
	public Sprite bait2;
	public Sprite bait3;
	public Sprite bait4;

	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
		sLancarBait = lancadorBait.GetComponent<LancarBait> ();
		GameObject sheik = GameObject.FindWithTag("Player");
		render.enabled = false;
		playerPos = sheik.GetComponent<Transform> ();
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

		if (pegouBait) {
			StartCoroutine (baitGUIEnable ());
		}

		if (sLancarBait.contadorBait == 0) {
			render.sprite = bait0;
		} else if (sLancarBait.contadorBait == 1) {
			render.sprite = bait1;
		} else if (sLancarBait.contadorBait == 2) {
			render.sprite = bait2;
		} else if (sLancarBait.contadorBait == 3) {
			render.sprite = bait3;
		} else if (sLancarBait.contadorBait == 4) {
			render.sprite = bait4;
		}
	}

	IEnumerator baitGUIEnable (){
		render.enabled = true;
		yield return new WaitForSeconds (2);
		pegouBait = false;
		if (forca <= 0) {
			render.enabled = false;
		}
		yield break;
	}

}
