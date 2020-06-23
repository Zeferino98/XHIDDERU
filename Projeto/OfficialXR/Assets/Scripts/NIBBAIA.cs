using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NIBBAIA : MonoBehaviour
{

    private PlayerController sPlayer;
    private NavMeshAgent agent;
	private ParticleSystem hitEffect;
	private EndStage fimfase;

	public AudioClip passosNibba;
	public AudioClip passosNibbaRun;
	public AudioClip nibbaPunch;

	public GameObject hitSystem;
	public GameObject stunEffect;
	public GameObject groundEffect;
	public GameObject endgame;
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
	public float tempoEspera = 0.1f;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject sheik = GameObject.FindWithTag("Player");
        sPlayer = sheik.GetComponent<PlayerController> ();
        player = sheik.GetComponent<Transform> ();
        agent = GetComponent<NavMeshAgent> ();
        agent.autoBraking = false;
		agent.destination = pontos [pontoDest].position;
		hitEffect = hitSystem.GetComponent<ParticleSystem> ();
		GameObject endgame = GameObject.Find ("EndStage");
		fimfase = endgame.GetComponent<EndStage> ();
		lastPos = agent.destination;
    }

    // Update is called once per frame
    void Update()
    {
		if (funcionando)
        {
            RaycastHit hit;

			if (fimfase.fimFase == true) {
				funcionando = false;
				animator.SetBool ("Parado", true);
				animator.SetBool ("Correndo", false);
				animator.SetBool ("Andando", false);
			}

            Vector3 origem = new Vector3(transform.position.x + rayX, transform.position.y + rayY, transform.position.z + rayZ);

			if (Physics.Raycast(new Vector3 (origem.x, 0.5f, origem.z), transform.forward, out hit, 1.5f))
            {
				if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Colidindo");
                    animator.SetBool("Socando", true);
                    socando = true;
                    sPlayer.vivo = false;
                    funcionando = false;
                    detectado = false;
					StartCoroutine (delayHit ());
                }
            }

            if (ouvindo)
            {
                detectando = true;
            }
            else if (vendo)
            {
                detectando = true;
				baitado = false;
            }
            else
            {
                detectando = false;
            }

            if (detectando)
            {
				agent.enabled = true;
                lastPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
                agent.destination = lastPos;
                Debug.Log("Detectando!");
                detectado = true;
                perderVista = false;
            }

            if (baitado)
            {
                agent.enabled = true;
                agent.destination = lastPos;
                Debug.Log("Baitado");
                detectado = true;
                perderVista = false;
            }

            if (sPlayer.vivo == false)
            {
                detectando = false;
                detectado = false;
            }

            if (detectado == true)
            {
				agent.enabled = true;
				agent.destination = lastPos;
				perderVista = false;
				if (baitado) {
					patrulhando = false;
					agent.speed = walkSpeed;
					animator.SetBool ("Correndo", false);
					animator.SetBool ("Parado", false);
					animator.SetBool ("Andando", true);
					animator.SetBool ("Procurando", false);
					if ((transform.position.x > lastPos.x - 1.3f) && (transform.position.z > lastPos.z - 1.3f) && (transform.position.y > lastPos.y - 1.3f) && (transform.position.x < lastPos.x + 1.3f) && (transform.position.y < lastPos.y + 1.3f) && (transform.position.z < lastPos.z + 1.3f))
					{
						perderVista = true;
						detectado = false;
						baitado = false;
					}
				} else {
					patrulhando = false;
					girando = true;
					agent.speed = runSpeed;
					animator.SetBool ("Correndo", true);
					animator.SetBool ("Parado", false);
					animator.SetBool ("Andando", false);
					animator.SetBool ("Procurando", false);
					baitado = false;
					if ((transform.position.x > lastPos.x - 0.5f) && (transform.position.z > lastPos.z - 0.5f) && (transform.position.y > lastPos.y - 0.5f) && (transform.position.x < lastPos.x + 0.5f) && (transform.position.y < lastPos.y + 0.5f) && (transform.position.z < lastPos.z + 0.5f))
					{
						perderVista = true;
						detectado = false;
						baitado = false;
					}
				}
            }
            
			if (baitado || patrulhando) {
				GetComponent<AudioSource> ().pitch = 1.7f;
				if (!perderVista && agent.enabled == true) {
					if (GetComponent <AudioSource> ().isPlaying == false) {
						GetComponent <AudioSource> ().PlayOneShot (passosNibba);
					}
				}
			}

			if (detectado) {
				if (!perderVista && agent.enabled == true) {
					if (GetComponent <AudioSource> ().isPlaying == false) {
						GetComponent <AudioSource> ().PlayOneShot (passosNibbaRun);
					}
				}
			}

			if (perderVista) {
                agent.enabled = false;
                animator.SetBool("Andando", false);
                animator.SetBool("Correndo", false);
                animator.SetBool("Parado", true);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
                    {
                        animator.SetBool("Parado", false);
                        animator.SetBool("Procurando", true);
                        perderVista = false;
                    }
                }
            } else {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("IdleLook"))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        animator.SetBool("Procurando", false);
                        patrulhando = true;
                        agent.enabled = true;
                        agent.destination = pontos[pontoDest].position;
                    }
                }
            }

            if (detectado || perderVista) {
                Vector3 targetDir = lastPos - transform.position;
                float step = 10 * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }

            if (desmaiado)
            {
                StartCoroutine(desmaio());
            }

            if (!desmaiado)
            {
                animator.SetBool("Desmaiado", false);
            }

            if (patrulhando)
            {
				if (pontos.Length <= 1) {
					if ((transform.position.x > pontos [pontoDest].position.x - 1.3f) && (transform.position.z > pontos [pontoDest].position.z - 1.3f) && (transform.position.x < pontos [pontoDest].position.x + 1.3f) && (transform.position.z < pontos [pontoDest].position.z + 1.3f)) {
						agent.enabled = false;
						animator.SetBool ("Parado", true);
						animator.SetBool ("Correndo", false);
						animator.SetBool ("Andando", false);
						transform.rotation = pontos [pontoDest].rotation;
					} else {
						agent.enabled = true;
						animator.SetBool ("Parado", false);
						animator.SetBool ("Correndo", false);
						animator.SetBool ("Andando", true);
					}
				} else {
					agent.speed = walkSpeed;
					agent.enabled = true;
					animator.SetBool ("Parado", false);
					animator.SetBool ("Correndo", false);
					animator.SetBool ("Andando", true);
					if (pontos.Length > 1) {
						if (!agent.pathPending && agent.remainingDistance < 0.5f) {
							ProximoPonto ();
						}
					}
				}
            }
        }
        else
        {
            agent.enabled = false;
            if (socando)
            {
				if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1f) {
					animator.SetBool ("Socando", false);
					animator.SetBool ("Correndo", false);
					animator.SetBool ("Parado", true);
					socando = false;
				}
            }
        }
    }

    IEnumerator desmaio()
    {
        Debug.Log("IEnumerator");
		agent.enabled = false;
		patrulhando = false;
		perderVista = false;
		detectado = false;
		detectando = false;
		baitado = false;
		girando = false;
        funcionando = false;
        animator.SetBool("Parado", false);
        animator.SetBool("Desmaiado", true);
		transform.position = new Vector3 (transform.position.x, -0.8f, transform.position.z);
		GameObject particulaStun = (GameObject)Instantiate (stunEffect, new Vector3 (transform.position.x+1, 0.5f, transform.position.z-1), transform.rotation);
		GameObject particulaGround = (GameObject)Instantiate (groundEffect, new Vector3 (transform.position.x+1, 0, transform.position.z-1), transform.rotation);
        yield return new WaitForSeconds(10);
		Destroy (particulaStun);
		Destroy (particulaGround);
		agent.enabled = true;
		transform.position = new Vector3 (transform.position.x, 0.06666677f, transform.position.z);
		perderVista = true;
		desmaiado = false;
        funcionando = true;
        animator.SetBool("Parado", true);
        animator.SetBool("Desmaiado", false);
        animator.SetBool("Correndo", false);
        yield break;
    }

	IEnumerator delayHit()
	{
		yield return new WaitForSeconds (0.7f);
		GetComponent <AudioSource> ().PlayOneShot (nibbaPunch);
		hitEffect.Play ();
		yield break;
	}

    void ProximoPonto ()
	{
		if (pontos.Length == 0) {
			return;
		}

		agent.enabled = false;
		animator.SetBool ("Andando", false);
		animator.SetBool ("Correndo", false);
		animator.SetBool ("Parado", true);
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= tempoEspera) {
				animator.SetBool ("Parado", false);
				animator.SetBool ("Procurando", true);
			}
		} else {
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("IdleLook")) {
				if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1f) {
					animator.SetBool ("Procurando", false);
					pontoDest = (pontoDest + 1) % pontos.Length;
					agent.enabled = true;
					agent.destination = pontos [pontoDest].position;
					patrulhando = true;
				}
			}
		}
	}
}
        