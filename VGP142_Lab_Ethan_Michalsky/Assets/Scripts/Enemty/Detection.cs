using UnityEngine;

public class Detection : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CloseUpEnemy cue;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            cue.targetInRange = true;
        }
    }

}
