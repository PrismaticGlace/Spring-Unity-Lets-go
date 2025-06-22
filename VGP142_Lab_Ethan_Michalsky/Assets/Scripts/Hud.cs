using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public PlayerMovement playMove;
    public TMP_Text hudText;
    public TMP_Text weaponText;

    void Update() {
        hudText.text = $"Velocity: X: {Mathf.Round(playMove.velocity.x)} | Z: {Mathf.Round(playMove.velocity.z)}";
        if (playMove.currWeapon) {
            weaponText.text = $"Current Weapon: Sword";
        }
        else {
            weaponText.text = $"Current Weapon: Spear";
        }
    }
}
