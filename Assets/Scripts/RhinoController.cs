using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RhinoController : MonoBehaviour {

    // Use this for initialization
    public Transform target;
    private NavMeshAgent navMeshAgent;
    private string roarTrigger = "roar";
    private string deadTrigger = "dead";
    private string crystalTag = "Crystal";

    //衝撞參數
    public float impactChargeTime = 1f;
    public Transform impactTarget;
    public float impactSpeed = 50f;
    public int impactDamge = 50;
    public ParticleSystem explosionEffect;
    public float walkingSpeed =3f;
    public Slider aimSlider;
    public float hitOff = 10f;
    //音效
    public AudioClip roarSound;
    public AudioClip deadSound;
    public AudioClip impactSound;

    private AudioSource audioSource;
    private Animator animator;

    private void Awake () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
	}
    private void OnEnable() {
        StartCoroutine("ProcessImpact");


    }
    private IEnumerator ProcessImpact() {
        aimSlider.gameObject.SetActive(true);
        aimSlider.value = aimSlider.minValue;
        aimSlider.maxValue = impactChargeTime;
        animator.SetTrigger(roarTrigger);
        while (aimSlider.value < aimSlider.maxValue) {
            aimSlider.value += Time.deltaTime;
            yield return null;

        }
        navMeshAgent.speed = impactSpeed;
        RaycastHit hit;
        float distance = Vector3.Distance(impactTarget.position, transform.position);
        Vector3 targetPos = impactTarget.position;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance)) {
            if (hit.collider.CompareTag(crystalTag)) {
                targetPos = hit.point;
                
            }

        }
        navMeshAgent.SetDestination(targetPos-transform.forward * hitOff);
    }

	// Update is called once per frame
	private void Update () {
        //navMeshAgent.SetDestination(target.position);
	}
    public void TriggerRoarSound() {
        audioSource.PlayOneShot(roarSound);

    }
}
