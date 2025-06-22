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
    public PlayerMovement plm;

    private void Awake() {
        plm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Player")) {
            CharacterController pc = collision.gameObject.GetComponent<CharacterController>();
            switch (type) {
                case PickupType.Banana:
                    //
                    Debug.Log("Banana Touched");
                    try {
                        Destroy(gameObject);
                        plm.addHealth();
                    }
                    catch (Exception e) { 
                        Debug.LogException(e);
                    }
                    break;
                case PickupType.Pinapple:
                    //
                    Debug.Log("Pinapple pen");
                    try {
                        Destroy(gameObject);
                    }
                    catch (Exception e) { 
                        Debug.LogException(e);
                    }
                    break;
                case PickupType.Mango:
                    //
                    Debug.Log("Mango Hell Yeah");
                    break;
            }
        }
    }
}
