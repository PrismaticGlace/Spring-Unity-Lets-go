using UnityEngine;

public class SavePlatform : MonoBehaviour {
    [SerializeField] private GameManager gameMana;
    [SerializeField] private PlayerMovement pla;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            pla.PlayerSaveData();
            gameMana.SaveGame();
            Destroy(gameObject);
        }
    }
}
