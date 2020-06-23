using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NIBBAIA : MonoBehaviour {

	private PlayerController sPlayer;
	private NavMeshAgent agent;

	public Transform player;
	public Transform[] pontos;

	Animator animator;

	public Vector3 lastPos;

	int pontoDest = 0;

	public float rayX = 0.79f;
	public float rayY = 1.11f;
	public float rayZ = -1.24f;
	public float rayDistance = 10f;
	public float rayThicc = 1.67f;

	public float walkSpeed = 3;
	public float runSpeed = 6;
	public bool detectando = false;
	public bool ouvindo = false;
	public bool vendo = false;
	public bool detectado = false;
	public bool funcionando = true;
	public bool desmaiado = false;
	public bool socando = false;
	public bool patrulhando = true;
	public bool perderVista = false;
	public bool baitado = false;
	public bool girando = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		GameObject sheik = GameObject.FindWithTag ("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
		player = sheik.GetComponent<Transform> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.autoBraking = false;
		ProximoPonto();
	}
	
	// Update is called once per frame
	void Update () {
		if (funcionando) {
			RaycastHit ver;
			RaycastHit hit;
		 
			Vector3 origem = new Vector3 (transform.position.x + rayX, transform.position.y + rayY, transform.position.z + rayZ);

			if (Physics.SphereCast (origem, rayThicc, transform.forward, out ver, rayDistance)) {
				if (ver.collider.gameObject.tag == "Player") {
					vendo = true;
					detectado = true;
				} else if (ver.collider.gameObject.tag == "Player" && ver.collider.gameObject.tag == "Inimigo") {
					vendo = true;
				} else {
					vendo = false;
				}
				Debug.Log (ver.collider.gameObject.tag);
			} else {
				vendo = false;
			}

			if (Physics.Raycast (origem, transform.forward, out hit, 1.5f)) {
				if (hit.collider.gameObject.tag == "Player") {
					Debug.Log ("Colidindo");
					animator.SetBool ("Socando", true);
					socando = true;
					sPlayer.vivo = false;
					funcionando = false;
					detectado = false;
				}
			}

			if (ouvindo) {
				detectando = true;
			} else if (vendo) {
				detectando = true;
			} else {
				detectando = false;
			}

			if (detectando) {
				lastPos = new Vector3 (player.transform.position.x, 0, player.transform.position.z);
				agent.destination = lastPos;
				agent.enabled = true;
				Debug.Log ("Detectando!");
				detectado = true;
				perderVista = false;
			}

			if (baitado) {
				agent.enabled = true;
				agent.destination = lastPos;
				Debug.Log ("Baitado");
				detectado = true;
				perderVista = false;
			}

			if (sPlayer.vivo == false) {
				detectando = false;
				detectado = false;
			}

			if (detectado == true) {
				patrulhando = false;
				girando = true;
				if (vendo) {
					agent.speed = runSpeed;
					animator.SetBool ("Correndo", true);
					animator.SetBool ("Parado", false);
					animator.SetBool ("Andando", false);
					animator.SetBool ("Procurando", false);
					baitado = false;
				} else if (baitado) {
					animator.SetBool ("Correndo", false);
					animator.SetBool ("Parado", false);
					animator.SetBool ("Andando", true);
					animator.SetBool ("Procurando", false);
				}

				if ((transform.position.x > lastPos.x-1) && (transform.position.y > lastPos.y-1) && (transform.position.x < lastPos.x+1) && (transform.position.y < lastPos.y+1)) {
					perderVista = true;
					detectado = false;
					baitado = false;
				}

			} else if (perderVista) {
				agent.enabled = false;
				animator.SetBool ("Andando", false);
				animator.SetBool ("Correndo", false);
				animator.SetBool ("Parado", true);
				if (animator.GetCurrentAnimatorStateInfo(0).IsName ("Idle")) {
					if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.2f) {  
						animator.SetBool ("Parado", false);
						animator.SetBool ("Procurando", true);
						perderVista = false;
					}
				}
			 } else {
				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("IdleLook")) {
					if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1f) {  
						animator.SetBool ("Procurando", false);
						patrulhando = true;
						agent.enabled = true;
						agent.destination = pontos [pontoDest].position;
					}
				}
			}

			if (detectado || perderVista) {
				Vector3 targetDir = lastPos - transform.position;
				float step = 4 * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0f);
				transform.rotation = Quaternion.LookRotation (newDir);
			}

			if (desmaiado) {
				StartCoroutine (desmaio ());
			}

			if (!desmaiado) {
				animator.SetBool ("Desmaiado", false);
			}

			if (patrulhando) {
				agent.speed = walkSpeed;
				agent.enabled = true;
				animator.SetBool ("Parado", false);
				animator.SetBool ("Correndo", false);
				animator.SetBool ("Andando", true);
				if (!agent.pathPending && agent.remainingDistance < 0.5f) {
					ProximoPonto ();
				}	
			} 
		} else {
			agent.enabled = false;
			if (socando) {
				if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1f) {  
					animator.SetBool ("Socando", false);
					animator.SetBool ("Correndo", false);
					animator.SetBool ("Parado", true);
					socando = false;
				}
			}
		}
	}

	IEnumerator desmaio(){
		Debug.Log ("IEnumerator");
		funcionando = false;
		animator.SetBool ("Parado", false);
		animator.SetBool ("Desmaiado", true);
		yield return new WaitForSeconds (5);
		desmaiado = false;
		funcionando = true;
		animator.SetBool ("Parado", true);
		animator.SetBool ("Desmaiado", false);
		animator.SetBool ("Correndo", false);
		yield break;
	}

	void ProximoPonto (){
		if (pontos.Length == 0) {
			return;
		}
		agent.destination = pontos [pontoDest].position;
		pontoDest = (pontoDest + 1) % pontos.Length;
	}
}