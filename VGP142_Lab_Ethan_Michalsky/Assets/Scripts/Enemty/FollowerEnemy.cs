using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Windows;
public class FollowerEnemy : MonoBehaviour{

    public bool canMove = true;
    public GameObject target;
    public Transform targetTrans;
    public CharacterController cc;
    [Space]
    //Projectile Things
    [SerializeField] private float distThreshX = 25f;
    [SerializeField] private float distThreshZ = 25f;
    [SerializeField] private float projectileFireRate = 2.0f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject projectilePf;
    private float timeLastFired = 0;
    [Space]
    //Speeds
    [SerializeField, Range(0.0f, 15.0f)] private float speed;
    [SerializeField, Range(0.0f, 20.0f)] private float turnSpeed;
    [SerializeField, Range(0.0f, 750.0f)] private float turnRotateSpeed;
    [Space]
    [SerializeField] protected int health = 10;
    [SerializeField] private float lastTakeDamage = 0f;
    [SerializeField] private float invFrames = 2f;
    [SerializeField] private GameObject eneDrop;
    protected Animator anim;
    //Vectors
    private Vector3 targetVector;
    private Vector3 moveRotation;

    private void Awake() {
        target = GameObject.FindGameObjectWithTag("Player");
        targetTrans = target.transform;
    }

    void Start() {
        if (projectileFireRate <= 0) {
            projectileFireRate = 2f;
        }
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (health <= 0) {
            Destroy(gameObject);
        }

        if (canMove) {
            targetVector = (target.transform.position - transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(moveRotation, Vector3.up); // Returns target rotation.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnRotateSpeed * Time.deltaTime);

            //Smooth Interpolate Rotation
            moveRotation = Vector3.SmoothDamp(moveRotation, targetVector, ref moveRotation, 0.1f, Mathf.Infinity);

            //Smooth Interpolate Movement
            //moveTowards = Vector3.SmoothDamp(moveTowards, targetVector, ref moveTowards, 0.05f, 10f);

            //Move
            //velocity.x = moveTowards.x * speed;
            //velocity.z = moveTowards.z * speed;
            //velocity.y = moveTowards.y * speed;
            CheckDistance(Mathf.Abs(targetTrans.position.x - transform.position.x), Mathf.Abs(targetTrans.position.z - transform.position.z));
            //Move
            //cc.Move(velocity * Time.deltaTime);
        }
        lastTakeDamage -= Time.deltaTime;
    }

    public void Fire() {
        anim.SetBool("Firing", true);
        GameObject project = Instantiate(projectilePf, spawnPoint.position, Quaternion.identity) as GameObject;
        Rigidbody prb = project.GetComponent<Rigidbody>();
        Vector3 fireTo = (target.transform.position - spawnPoint.transform.position).normalized;
        prb.AddForce(fireTo * 3f, ForceMode.Impulse);
        Debug.Log("Did it add the velocity");
        StartCoroutine(ResetFire());
    }

    IEnumerator ResetFire() {
        yield return new WaitForSeconds(2);
        anim.SetBool("Firing", false);
    }

    void CheckDistance(float disX, float disZ) {
        if (disX <= distThreshX && disZ <= distThreshZ) {
            CheckFire();
        }
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

    void CheckFire() {
        Debug.Log("Trying to Fire");
        if (Time.time >= timeLastFired + projectileFireRate) {
            Fire();
            timeLastFired = Time.time;
        }
    }
}
