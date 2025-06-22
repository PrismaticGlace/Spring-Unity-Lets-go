using TMPro;
using UnityEngine;

public class Death : MonoBehaviour {
    public static Death Instance {
        get {
            if (!instance) {
                instance = new GameObject("Death").AddComponent<Death>();
            }
            return instance;
        }
    }
    private static Death instance;

    void Start() {
        gameObject.SetActive(false);
    }

    void Awake() {
        if (instance && (instance.GetInstanceID() != GetInstanceID())) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
