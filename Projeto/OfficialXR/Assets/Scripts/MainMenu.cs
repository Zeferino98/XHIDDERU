using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject start;
	public GameObject quit;

	void Start(){
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		start = GameObject.Find ("StartButton");
		quit = GameObject.Find ("QuitButton");
    }

    IEnumerator espera()
    {
		start.gameObject.SetActive (false);
		quit.gameObject.SetActive (false);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Tutorial");
        yield break;
    }

	public void PlayGame(){
        StartCoroutine(espera());
	}

	public void QuitGame(){
		Debug.Log ("Arregou");
		Application.Quit ();
	}

}
