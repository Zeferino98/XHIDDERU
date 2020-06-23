using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	static bool spawn = false;
	public bool emSpawn = false;
	static int vida = 3;
	public int nVidas;
    public float walkSpeed = 4;
    public float runSpeed = 6;
    public float chrouchSpeed = 2;
	public CapsuleCollider playerCollider;
	float delayHabilidade = 60f;
	public float cooldownHabilidade;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
	public bool vivo = true;
	public bool funcionar = true;
	public bool somMorte = true;
    public bool crouching = false;
	public bool running = false;
	public bool walking = false;
	public bool atacando = false;
	public bool invisivel = false;
	public bool usandoSkill = false;
	public bool podePegar = false;
	public bool jogandoBait = false;
	public bool noMato = false;
	public bool dancar = false;

	public AudioClip invisibleSound;
	public AudioClip sheikPunch;
	public AudioClip airPunch;
	public AudioClip sheikMorte;
	public AudioClip sheikPassos;
	public AudioClip grama;

	private NIBBAIA nibba;
	public GameObject hitSystem;
	private ParticleSystem invisibleSystem;
	private SpriteRenderer render;
	public GameObject luz;
	public GameObject invisibleEffect;
	public GameObject Spawn;	
    Animator animator;
    Transform cameraT;

	// Use this for initialization
	void Start () {
		nVidas = vida;
		emSpawn = spawn;
		transform.position = Spawn.transform.position;
     	animator = GetComponent<Animator>();
     	cameraT = Camera.main.transform;
		playerCollider = GetComponent<CapsuleCollider> ();
		GameObject inimigo = GameObject.FindWithTag ("Inimigo");
		nibba = inimigo.GetComponent<NIBBAIA> ();
		invisibleSystem = invisibleEffect.GetComponent<ParticleSystem> ();
		render = GetComponentInChildren<SpriteRenderer> ();
		render.enabled = false;
		somMorte = true;
		luz.SetActive (false);
	}

    // Update is called once per frame
    void Update(){

		if (spawn) {
			funcionar = false;
			animator.SetBool ("Respawn", true);
		}

		if (funcionar && vivo) {
			// Inputs de Movimento
			Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			Vector2 inputDir = input.normalized;

			if (Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0) {
				walking = false;
			} else {
				walking = true;
			}

			// Direção do Personagem
			if (inputDir != Vector2.zero) {
				float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
			}

			// Rotação e aceleração para a frente do personagem
			if (!atacando) {
				transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);
			}

			if (cooldownHabilidade > 0) {
				cooldownHabilidade -= Time.deltaTime;
			}

			if (Input.GetButtonDown ("Invisivel") && usandoSkill == false && cooldownHabilidade <= 0) {
				GetComponent <AudioSource> ().PlayOneShot (invisibleSound);
				StartCoroutine (skillInvisivel ());
			}


			if (walking && !running && !crouching) {
				GetComponent<AudioSource> ().volume = 0.7f;
			}

			if (walking && running && !crouching) {
				GetComponent<AudioSource> ().volume = 1f;
			}

			if (walking && crouching) {
				GetComponent<AudioSource> ().volume = 0.4f;
			}

			if (!walking && crouching && !atacando && !jogandoBait) {
				GetComponent<AudioSource> ().volume = 0f;
			} else {
				GetComponent<AudioSource> ().volume = 1f;
			}

			// Controle de Movimento em Chrouching
			if (crouching == false) {
				playerCollider.center = new Vector3 (0, 0.94f, 0.12f);
				playerCollider.height = 1.85f;
				animator.SetBool ("isChrouch", false);
			}

			if (crouching == true) {
				playerCollider.center = new Vector3 (0, 0.5f, 0.12f);
				playerCollider.height = 1.33f;

				// Transição de animações em Chrouching
				animator.SetBool ("isChrouch", true);

				float animationSpeedChrouchPercent = ((crouching) ? 1 : 0f) * inputDir.magnitude;
				animator.SetFloat ("speedChrouchPercent", animationSpeedChrouchPercent, speedSmoothTime, Time.deltaTime);

				// Velocidade enquanto Chrouching
				float targetChrouch = chrouchSpeed * inputDir.magnitude;
				currentSpeed = (Mathf.SmoothDamp (currentSpeed, targetChrouch, ref speedSmoothVelocity, speedSmoothVelocity)) / 2;
			}

			if (Input.GetButtonDown ("Crouch")) {
				crouching = !crouching;
			}

			if (podePegar) {
				animator.SetBool ("Pegando", true);
				funcionar = false;
			}

			if (jogandoBait) {
				animator.SetBool ("Jogando", true);
				funcionar = false;
			}

			// Informações do Sprint e Velocidade
			float targetWalk = walkSpeed * inputDir.magnitude;
			currentSpeed = Mathf.SmoothDamp (currentSpeed, targetWalk, ref speedSmoothVelocity, speedSmoothTime);

			if ((Input.GetAxis ("Run") == -1) || (Input.GetButton ("Run")) && !atacando) {
				running = true;
			} else {
				running = false;
			}

			if (running && !crouching) {
				float targetSpeed = runSpeed * inputDir.magnitude;
				currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
			}

			// Transição de animações de Walk / Running
			float animationSpeedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
			animator.SetFloat ("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

			if (atacando) {
				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("CrossPunch")) {
					if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.8f) {  
						animator.SetBool ("Atacando", false);
						atacando = false;
					}
				}
			}

			Debug.DrawRay (new Vector3 (transform.position.x, transform.position.y+0.5f, transform.position.z), transform.TransformDirection (Vector3.forward) * 1.5f, Color.red);

			if (Input.GetButtonDown ("Attack") && !Input.GetButton ("ForcaMouse")) {
				Debug.Log ("Atacando");
				animator.SetBool ("Atacando", true);
				atacando = true;
				GetComponent<AudioSource> ().PlayOneShot (airPunch);
				RaycastHit range;
				if (Physics.Raycast (new Vector3 (transform.position.x, transform.position.y+0.5f, transform.position.z), transform.forward, out range, 2.5f)) {
					if (range.collider.gameObject.tag == "Inimigo") {
						GameObject inimigo = range.collider.gameObject;
						nibba = inimigo.GetComponent<NIBBAIA> ();
						if (nibba.desmaiado == false) {
							GetComponent<AudioSource> ().PlayOneShot (sheikPunch);
							GameObject particula = (GameObject)Instantiate (hitSystem, new Vector3 (inimigo.transform.position.x, 1.15f, inimigo.transform.position.z), inimigo.transform.rotation);
							Debug.Log ("Acertou");
							nibba.desmaiado = true;
						}
					}
				}
			}

		} 

		if (!vivo) {
			funcionar = false;
			podePegar = false;
			jogandoBait = false;
			animator.SetBool("Pegando", false);
			animator.SetBool("Jogando", false);
			Rigidbody fisica = GetComponent<Rigidbody> ();
			fisica.isKinematic = true;
			animator.SetFloat ("speedPercent", 0f);
			animator.SetFloat ("speedChrouchPercent", 0f);
			StartCoroutine (delayHit ());
		} 

		if (!funcionar && vivo) {

			if (dancar) {
				animator.SetBool ("Dancando", true);
			}

			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Pick"))
			{
				if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
				{
					podePegar = false;
					animator.SetBool("Pegando", false);
					funcionar = true;
				}
			}

			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Respawn"))
			{
				if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.3f) {
					render.enabled = true;
				}

				if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
				{
					spawn = false;
					emSpawn = false;
					animator.SetBool("Respawn", false);
					funcionar = true;
					render.enabled = false;
				}
			}

			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
			{
				if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
				{
					jogandoBait = false;
					animator.SetBool("Jogando", false);
					funcionar = true;
				}
			}
		}
	}

	void somPassos(){
		GetComponent <AudioSource> ().PlayOneShot (sheikPassos);
		if (noMato) {
			GetComponent <AudioSource> ().PlayOneShot (grama);
		}
	}

	IEnumerator delayHit()
	{
		yield return new WaitForSeconds (0.7f);
		animator.enabled = true;
		invisivel = false;
		animator.SetBool ("Morto", true);
		if (somMorte) {
			GetComponent<AudioSource> ().volume = 0.7f;
			GetComponent<AudioSource> ().PlayOneShot (sheikMorte);
			somMorte = false;
		}
		luz.SetActive (true);
		yield return new WaitForSeconds (3f);
		if (vida > 1) {
			vida--;
			spawn = true;
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		} else {
			vida = 3;
			SceneManager.LoadScene ("StartMenu");
		}
		yield break;
	}

	IEnumerator skillInvisivel(){
		invisivel = true;
		usandoSkill = true;
		cooldownHabilidade = delayHabilidade;
		invisibleSystem.Play ();
		yield return new WaitForSeconds (2.5f);
		invisivel = false;
		usandoSkill = false;
		yield break;
	}
}