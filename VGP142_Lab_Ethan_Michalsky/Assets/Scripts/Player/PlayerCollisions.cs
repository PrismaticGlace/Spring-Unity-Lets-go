using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour{

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("EnemyProg")) {
            SceneManager.LoadScene("Game");
        }
    }
}
