using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class EnemyCollision : MonoBehaviour {
    public PlayerAttack pla;
    public CloseUpEnemy cue;

    private void Awake() {
        pla = GameObject.FindGameObjectWithTag("Attack").GetComponent<PlayerAttack>();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Attack")) {
            cue.TakeDamage(pla.damage);
        }
    }
}
