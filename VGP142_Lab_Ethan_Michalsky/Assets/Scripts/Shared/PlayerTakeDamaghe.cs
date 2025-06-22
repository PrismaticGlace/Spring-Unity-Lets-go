using UnityEngine;

public class PlayerTakeDamaghe : MonoBehaviour {
    public PlayerMovement pla;
    public PlayerAttack attack;

    private void Awake() {
        pla = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            pla.TakePlayerDamage(attack.damage);
        }
    }

}
