using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour{
    public PlayerMovement playMove;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("EnemyProg")) {
            SceneManager.LoadScene("Game");
        }
        if (!collision.gameObject.CompareTag("Ice")) {
            playMove.inDampDeccel = 0.03f;
        }
        if (collision.gameObject.CompareTag("Wall")) {
            playMove.velocity.x = 0;
            playMove.velocity.z = 0;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("SwordCollect")) {
            playMove.currWeapon = true;
        }
        if (other.gameObject.CompareTag("SpearCollect")) {
            playMove.currWeapon = false;
        }
    }

}
