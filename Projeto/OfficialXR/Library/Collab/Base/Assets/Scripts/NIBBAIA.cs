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
	public float movSpeed;
	public float rotSpeed;

	float rotStep;
	float movStep;

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

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		GameObject sheik = GameObject.FindWithTag ("Player");
		sPlayer = sheik.GetComponent<PlayerController> ();
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
				lastPos = player.transform.position;
				Debug.Log ("Detectando!");
				detectado = true;
				perderVista = false;
			}

			if (baitado) {
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
				Vector3 targetDir = lastPos - transform.position;
				rotStep = rotSpeed * Time.deltaTime;
				movStep = movSpeed * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, rotStep, 0.0f);
				transform.rotation = Quaternion.LookRotation (newDir);
				if (vendo) {
					movSpeed = 3f;
					transform.position = Vector3.MoveTowards (transform.position, lastPos, movStep);
					animator.SetBool ("Correndo", true);
					animator.SetBool ("Parado", false);
					animator.SetBool ("Andando", false);
					animator.SetBool ("Procurando", false);
					baitado = false;
				} else if (baitado) {
					movSpeed = 2f;
					transform.position = Vector3.MoveTowards (transform.position, lastPos, movStep);
					animator.SetBool ("Correndo", false);
					animator.SetBool ("Parado", false);
					animator.SetBool ("Andando", true);
					animator.SetBool ("Procurando", false);
				} else if (transform.position != lastPos) {
					transform.position = Vector3.MoveTowards (transform.position, lastPos, movStep);
				}

				if (transform.position == lastPos) {
					perderVista = true;
					detectado = false;
					baitado = false;
				}
			} else if (perderVista) {
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
					}
				}
			}

			if (desmaiado) {
				StartCoroutine (desmaio ());
			}

			if (!desmaiado) {
				animator.SetBool ("Desmaiado", false);
			}

			if (patrulhando) {
				agent.enabled = true;
				animator.SetBool ("Parado", false);
				animator.SetBool ("Correndo", false);
				animator.SetBool ("Andando", true);
				if (!agent.pathPending && agent.remainingDistance < 0.5f) {
					ProximoPonto ();
				}	
			} else {
				agent.enabled = false;
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