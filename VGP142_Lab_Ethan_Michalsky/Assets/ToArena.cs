using UnityEngine;
using UnityEngine.SceneManagement;

public class ToArena : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        SceneManager.LoadScene("Arena");
    }
}
