using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterType { ENEMY, HERO, HEALTHPOTION, MANAPOTION }

public class HeroAI_Controller : MonoBehaviour
{
    private static HeroAI_Controller _instance;
    public static HeroAI_Controller MyInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<HeroAI_Controller>();
            }
            return _instance;
        }
    }


    [Header("Objects")]                                             // have to be changed autamtic in Range of AI bzw. nearest to 
    public Transform targetLookAt;                                  // Enemyto Target
    public Transform targetMoveTo;                                  // Changed by AI_MoveTo
    public Transform idleObject;                                    // start Object. DELETE ME LATER

    private Character targetEnemy;

    [Header("Scoring System")]
    [SerializeField] private float score;                           // score calculated to choose action
    [SerializeField] private bool veto = false;                     // if true action can not be executed, if true utility = 0

    [Header("Sense")]
    public float checkRadius = 25.0f;
    public float fearDistance = 5.0f;
    public float turnSpeed = 5.5f;
    public LayerMask checkLayers;

    // others
    private NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    // Get and Set Target etc
    public float MyScore
    {
        get { return score; }
        set { score = value; }
    }
    public bool MyVeto
    {
        get { return veto; }
        set { veto = value; }
    }
    public Character MyTargetEnemy
    {
        get { return targetEnemy; }
    }

    // ---------------------------------------------

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        targetLookAt = idleObject;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SearchEnemyTarget();

        // save for Idle
        if (targetLookAt == null)
        {
            targetLookAt = idleObject;
        }

        float distanceToTarget = Vector3.Distance(this.transform.position, targetLookAt.transform.position);
    }

    // LEAVE IT IN BECAUSE IS DONE ALL THE TIME
    // Search for new Target to LookAt
    public void SearchEnemyTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, checkRadius, checkLayers);
        Array.Sort(colliders, new DistanceComparer(transform));
        if (colliders.Length != 0)
        {
            targetLookAt = colliders[0].transform;
            targetEnemy = colliders[0].GetComponent<Character>();
            LookAt(targetLookAt.transform);
        }
    }
    //Rotate HeroAI to Target
    void LookAt(Transform target)
    {
        Vector3 direction = target.position.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    // Draw CheckRadius of Hero Sense for Debugging
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, checkRadius);
    }
}
