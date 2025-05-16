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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Wtf");
        if (collision.gameObject.CompareTag("Enemy")) {
            //Hi
        }
        else {
            Destroy(gameObject);
        }
    }
}
