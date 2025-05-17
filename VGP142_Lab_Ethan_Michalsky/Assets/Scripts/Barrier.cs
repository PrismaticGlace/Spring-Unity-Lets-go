using UnityEngine;
using UnityEngine.SceneManagement;

public class Barrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene("Dungeon");
        }
    }
}
