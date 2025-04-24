using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public enum PickupType {
        Banana,
        Pinapple,
        Mango
    }

    public PickupType type;

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Player")) {
            CharacterController pc = collision.gameObject.GetComponent<CharacterController>();
            switch (type) {
                case PickupType.Banana:
                    //
                    Debug.Log("Banana Touched");
                    break;
                case PickupType.Pinapple:
                    //
                    Debug.Log("Pinapple pen");
                    break;
                case PickupType.Mango:
                    //
                    Debug.Log("Mango Hell Yeah");
                    break;
            }
        }
    }
}
