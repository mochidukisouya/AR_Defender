using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class HeroController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float attackRadius;
    public RectTransform attackRangerRect;
    public LayerMask enemyLayerMask;
    public Color detectColor;
    public Color normalColor;
    public Image attackProbeCircle;
    public GameObject target;
    public float turnsmooth = 15f;

    public Animator animator;
    public string attackBool = "attack";
    public AudioSource audioSource;
    public AudioClip fireSound;
    public ParticleSystem gunshotEffect;

    // Use this for initialization

    private void Awake() {
        attackRadius = attackRangerRect.rect.width / 2;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target==null) 
            return;
        if (navMeshAgent.remainingDistance == 0)//離目標的剩餘距離==0
        {
            Vector3 lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnsmooth * Time.deltaTime);
            animator.SetBool(attackBool,true);
        }
        else {
            animator.SetBool(attackBool, false);
        }

        
    }

    public void Move(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
        navMeshAgent.isStopped = false;
    }
    public void FixedUpdate() {

        Collider[] allCollider= Physics.OverlapSphere(transform.position,attackRadius, enemyLayerMask);
        if (allCollider.Length > 0)
        {
            attackProbeCircle.color = detectColor;
            target = allCollider[0].gameObject;
        }
        else {
            attackProbeCircle.color = normalColor;
            target = null;
            animator.SetBool(attackBool, false);
        }
    }
    public void OnGunTrigger() {
        audioSource.PlayOneShot(fireSound);
        gunshotEffect.Play(); 
    }
}
