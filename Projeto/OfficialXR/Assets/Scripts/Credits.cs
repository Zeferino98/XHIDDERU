using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {

    public GameObject aparece;
    public GameObject thx;

    // Use this for initialization

    IEnumerator credits()
    {
        yield return new WaitForSeconds(7.35f);
        aparece.gameObject.SetActive(true);
        yield break;
    }

    IEnumerator creditsthx()
    {
        yield return new WaitForSeconds(10.5f);
        thx.gameObject.SetActive(true);
        yield break;
    }

    void Start () {
        
        StartCoroutine(credits());
        StartCoroutine(creditsthx());

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
