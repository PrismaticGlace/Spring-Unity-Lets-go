using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class EnemyShootCollision : MonoBehaviour {
    public FollowerEnemy fle;
    public PlayerAttack pla;

    private void Awake() {
        pla = GameObject.FindGameObjectWithTag("Attack").GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Attack")) {
            fle.TakeDamage(pla.damage);
        }
    }
}
