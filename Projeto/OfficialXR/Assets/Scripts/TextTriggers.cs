using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTriggers : MonoBehaviour {

	private TutorialControler controler;
	private PlayerController sPlayer;
	private NIBBAIA sNibba;
	private float forca;
	public bool sobrepor = true;
	bool HUDtext = false;
	bool emText = false;

	// Use this for initialization
	void Start () {
		GameObject tc = GameObject.Find ("TutorialController");
		controler = tc.GetComponent<TutorialControler> ();
		GameObject sheik = GameObject.FindWithTag("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
		GameObject inimigo = GameObject.FindWithTag ("Inimigo");
		sNibba = inimigo.GetComponent<NIBBAIA> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (sNibba.detectado == true) {
			controler.textOn = true;
			controler.text = "Beware of the enemy! If they see or hear you they won't stop until you lose them or they catch you first!";
		} if (sNibba.perderVista == true) {
			controler.textOn = false;
		}

		forca = Input.GetAxis ("Forca");

		if (Input.GetButton ("ForcaMouse")){
			forca = 1f;
		}

		if (forca > 0) {
			HUDtext = true;
		} else {
			HUDtext = false;
		}

		if (HUDtext) {
			sobrepor = false;
			controler.textOn = true;
			controler.text = "This is your HUD! The blue circle shows how many 'Bait' you have (max. 4) and the green circle shows your 'Invisibility Skill' cooldown!";
		} else {
			sobrepor = true;
		}

		if (!HUDtext && !emText) {
			controler.textOn = false;
		}

	}

	void OnTriggerStay (Collider other){
		Debug.Log (other.name);

		if (other.name == "DekuText" && sobrepor) {
			emText = true;
			controler.textOn = true;
			controler.text = "These are 'Baits'. Calculate your force by holding 'LT' and then press 'RB' to throw a bait to trick the enemies. Press 'A' nearby a Giant bait to fill your bait bar by 1";
		} else if (other.name == "EnemyText" && sobrepor) {
			emText = true;
			controler.textOn = true;
			controler.text = "Beware of the enemy ahead! If they see or hear you they won't stop until you lose them or they catch you first!";
		} else if (other.name == "InvisibleText" && sobrepor) {
			emText = true;
			controler.textOn = true;
			controler.text = "Press 'B' to crouch. You'll make no noise while walking and the Enemies won't see you on grass!";
		} else if (other.name == "StartText" && sobrepor) {
			controler.textOn = true;
			emText = true;
			if (sPlayer.nVidas == 3) {
				controler.text = "Use the 'left stick' to move and press 'RT' to sprint.                       Press 'Start' to exit tutorial.";
			} else {
				controler.text = "Don't let him touch you! If you lose all your 3 lives you'll return to the Main Menu";
			}
		}
	}

	void OnTriggerExit (Collider other){
		if (other.name == "DekuText") {
			emText = false;
			controler.textOn = false;
		} else if (other.name == "EnemyText") {
			emText = false;
			controler.textOn = false;
		} else if (other.name == "InvisibleText") {
			emText = false;
			controler.textOn = false;
		} else if (other.name == "StartText") {
			emText = false;
			controler.textOn = false;
			if (sPlayer.nVidas == 3) {
				StartCoroutine (invSkill ());
			} else {
				StartCoroutine (attackText ());
			}
		}
	}

	IEnumerator invSkill (){
		emText = true;
		controler.textOn = true;
		controler.text = "Tip: Press 'Y' to use your 'Invisibility skill'! (1min cooldown)";
		yield return new WaitForSeconds (3);
		controler.textOn = false;
		yield break;
	}

	IEnumerator attackText (){
		emText = true;
		controler.textOn = true;
		controler.text = "Tip: Press 'x' to attack! Hit an enemy from behind to take them out for 5 seconds!";
		yield return new WaitForSeconds (3);
		controler.textOn = false;
		yield break;
	}

}
