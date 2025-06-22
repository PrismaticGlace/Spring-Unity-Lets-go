using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float health = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            //Hi
        }
        else {
            Destroy(gameObject);
        }
    }
}
