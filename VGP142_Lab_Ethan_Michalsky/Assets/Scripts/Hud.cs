using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public PlayerMovement playMove;
    public TMP_Text hudText;

    void Update() {
        hudText.text = $"Velocity: X: {Mathf.Round(playMove.velocity.x)} | Z: {Mathf.Round(playMove.velocity.z)}";
    }
}
