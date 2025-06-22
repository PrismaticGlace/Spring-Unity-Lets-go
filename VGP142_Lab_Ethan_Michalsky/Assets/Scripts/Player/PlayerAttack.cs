using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public float waitFor;
    public int damage;

    private void OnEnable() {
        StartCoroutine(wait());
    }

    IEnumerator wait() {
        yield return new WaitForSeconds((float)waitFor);
        gameObject.SetActive(false);
    }

}
