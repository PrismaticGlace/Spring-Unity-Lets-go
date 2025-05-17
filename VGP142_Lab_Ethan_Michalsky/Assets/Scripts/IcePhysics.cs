using UnityEngine;
using UnityEngine.SceneManagement;

public class IcePhysics : MonoBehaviour
{
    public PlayerMovement playMove;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            playMove.inDampDeccel = 0.025f;
        }
    }
}
