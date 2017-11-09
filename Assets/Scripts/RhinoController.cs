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
    private bool isImpacting;
    private Collider SwitchCollider;


    private void Awake () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        aimSlider.gameObject.SetActive(false);
        SwitchCollider = GetComponent<Collider>();
    }
    private void OnEnable() {
        
        GameObject go = GameObject.FindGameObjectWithTag(crystalTag);
        if (go!= null) {
            target = go.transform;
            StartCoroutine("ProcessState");

        }

    }
    //讓犀牛以水晶為圓心的半徑內隨機移動(Coroutine)
    private IEnumerator ProcessState() {
        while (target != null) {
            //隨機產生犀牛的目的地，讓其前往
            navMeshAgent.speed = walkingSpeed;
            float randomRad = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Vector3.Distance(impactTarget.position, transform.position);
            Vector3 randomPos = target.position + new Vector3(Mathf.Cos(randomRad), 0, Mathf.Sin(randomRad)) * distance;
            navMeshAgent.SetDestination(randomPos);
            yield return null;
            //等待犀牛走到目的地
            while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) {
                yield return null;
            }
            //判斷水晶是否還存在，若不存在使用 yield break 離開 Coroutine
            if (target == null)
                yield break;
            //開始攻擊 呼叫 private IEnumerator ProcessImpact()
            yield return StartCoroutine("ProcessImpact");
            //攻擊結束後等待2秒
            yield return new WaitForSeconds(2f);
        }

    }
    private IEnumerator ProcessImpact() {
        transform.LookAt(target);
        aimSlider.gameObject.SetActive(true);
        aimSlider.value = aimSlider.minValue;
        aimSlider.maxValue = impactChargeTime;
        animator.SetTrigger(roarTrigger);
        while (aimSlider.value < aimSlider.maxValue) {
            aimSlider.value += Time.deltaTime;
            yield return null;

        }
        aimSlider.gameObject.SetActive(false);
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
        yield return null;
        //等待犀牛衝到目的地
        isImpacting = true;
        while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance){
            yield return null;
        }
        isImpacting = false;
    }


    public void TriggerRoarSound() {
        audioSource.PlayOneShot(roarSound);

    }
    private void OnTriggerEnter(Collider other) {
        if (isImpacting) {
            PlayHitEffect();
            AttackableBehavior attackable = other.GetComponent<AttackableBehavior>();
            if (attackable) {
                attackable.Hurt(impactDamge);
            }
        }
    }
    private void PlayHitEffect() {
        audioSource.PlayOneShot(impactSound);
        explosionEffect.Play();
    }
    public void OnDead() {
        StopAllCoroutines();
        aimSlider.gameObject.SetActive(false);
        navMeshAgent.enabled = false;
        audioSource.PlayOneShot(deadSound);
        animator.SetTrigger(deadTrigger);
        SwitchCollider.enabled = false;
        


    }


}
