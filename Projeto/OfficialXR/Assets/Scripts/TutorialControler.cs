using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = UnityEngine.UI.Text;
using UnityEngine.SceneManagement;

public class TutorialControler : MonoBehaviour {

	private Text txt;

	public GameObject texto;

	public bool textOn;
	public string text; 

	// Use this for initialization
	void Start () {
		texto = GameObject.Find("Textos");
		txt = texto.GetComponent<Text> ();
		textOn = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (textOn) {
			txt.enabled = true;
			txt.text = text;
		} else {
			txt.enabled = false;
		}

		if (Input.GetButtonDown ("Start")) {
			SceneManager.LoadScene ("Stage1");
		}
	}
}
