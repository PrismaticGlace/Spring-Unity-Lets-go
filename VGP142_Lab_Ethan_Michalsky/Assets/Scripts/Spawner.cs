using UnityEngine;

public class Spawner : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject ObjectOne;
    public GameObject ObjectTwo;

    private void Start() {
        int ran = Random.Range(1, 3);
        if (ran == 1) {
            Instantiate(ObjectOne, transform.position, Quaternion.identity);
        }
        else if (ran == 2) {
            Instantiate(ObjectTwo, transform.position, Quaternion.identity);
        }
    }
}