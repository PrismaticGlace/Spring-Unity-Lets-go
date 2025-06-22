using UnityEngine;

public class BGMCheck : MonoBehaviour
{
    private AudioSource aus;
    public GameObject player;
    public bool played = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        aus = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (player == true) {
            played = false;
        }
        else {
            if (player == false) {
                aus.Stop();
                played = true;
            }
        }
    }
}
