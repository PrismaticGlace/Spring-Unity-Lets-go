using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CloseUpEnemy : MonoBehaviour {

    public bool targetInRange = false;
    public bool isAttacking;
    [Range(1, 2)] public int attackChoice;
    [Space]
    public GameObject target;
    public Transform targetTrans;
    protected Animator anim;
    [SerializeField] protected int health = 10;
    [SerializeField] protected int maxHealth;
    public CharacterController cc;
    [Space]
    [SerializeField] private float distThreshX = 3f;
    [SerializeField] private float distThreshZ = 3f;
    [Space]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject eneDrop;
    [Space]
    //Speeds
    [SerializeField] private float gravity = -9.81f;
    [SerializeField, Range(0.0f, 15.0f)] private float speed;
    [SerializeField, Range(0.0f, 20.0f)] private float turnSpeed;
    [SerializeField, Range(0.0f, 750.0f)] private float turnRotateSpeed;
    [SerializeField, Range(0.0f, 30.0f)] private float termVelo;
    [Space]
    [SerializeField] private float lastTakeDamage = 0f;
    [SerializeField] private float invFrames = 2f;
    [SerializeField] private GameObject attObj;
    //Vectors
    private Vector3 targetVector;
    private Vector3 moveTowards;
    private Vector3 moveRotation;
    private Vector3 gravityVect;
    private Vector3 velocity;

    private void Awake() {
        target = GameObject.FindGameObjectWithTag("Player");
        targetTrans = target.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetInRange) {
            if (CheckDistance(Mathf.Abs(targetTrans.position.x - transform.position.x), Mathf.Abs(targetTrans.position.z - transform.position.z))) {
                anim.SetBool("Move", false);
                if (isAttacking) {
                    //Do nothing lmao
                }
                else {
                    StartAttack();
                }
            }
            else {
                targetVector = (target.transform.position - transform.position).normalized;

                Quaternion targetRot = Quaternion.LookRotation(moveRotation, Vector3.up); // Returns target rotation.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnRotateSpeed * Time.deltaTime);

                //Smooth Interpolate Rotation
                moveRotation = Vector3.SmoothDamp(moveRotation, targetVector, ref moveRotation, 0.1f, Mathf.Infinity);

                //Smooth Interpolate Movement
                moveTowards = Vector3.SmoothDamp(moveTowards, targetVector, ref moveTowards, 0.05f, 10f);

                //Move
                velocity.x = moveTowards.x * speed;
                velocity.z = moveTowards.z * speed;
                anim.SetBool("Move", true);
                gravityVect.y = gravity;
                velocity.y = (gravityVect.y);
                cc.Move(velocity);
            }
        }
        lastTakeDamage -= Time.deltaTime;
    }

    public virtual void TakeDamage(int damageValue) {
        if (lastTakeDamage < 0) {
            health -= damageValue;
            lastTakeDamage = invFrames;
        }
        if (health <= 0) {
            anim.SetTrigger("Death");
            GameObject project = Instantiate(eneDrop, spawnPoint.position, Quaternion.identity) as GameObject;
            if (transform.parent != null) Destroy(transform.parent.gameObject, 0.5f);
            else Destroy(gameObject, 0.5f);
        }
    }

    private bool CheckDistance(float disX, float disZ) {
        if (disX <= distThreshX && disZ <= distThreshZ) {
            return true;
        }
        else {
            return false;
        }
    }

    void StartAttack() {
        isAttacking = true;
        attackChoice = UnityEngine.Random.Range(2, 3);

        anim.SetBool("IsAttacking", true);
        anim.SetInteger("AttackChoice", attackChoice);
        float duration = 0f; ;
        if (attackChoice == 1) {
            duration = 1f;
        }
        else {
            duration = 2.3f;
        }
        attObj.SetActive(true);
        StartCoroutine(WaitUntilAnimation(duration));
    }
    IEnumerator WaitUntilAnimation(float duration) {
        Debug.Log($"Started at {Time.time}, waiting for {duration} seconds");
        yield return new WaitForSeconds(duration);
        Debug.Log($"Ended at {Time.time}");
        isAttacking = false;
        anim.SetBool("IsAttacking", false);
    }
}
