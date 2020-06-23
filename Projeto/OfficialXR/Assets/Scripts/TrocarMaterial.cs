using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrocarMaterial : MonoBehaviour {

	private PlayerController sPlayer;
	public Material materialInvisivel;
	public Material material;

	// Use this for initialization
	void Start () {
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (sPlayer.invisivel == true) {
			this.GetComponent<SkinnedMeshRenderer> ().material = materialInvisivel;
		} else {
			this.GetComponent<SkinnedMeshRenderer> ().material = material;
		}
	}
}
