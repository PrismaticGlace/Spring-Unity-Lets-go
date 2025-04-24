using UnityEngine;

public class CameraMove : MonoBehaviour {

    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;
    [SerializeField] private float minZPos;
    [SerializeField] private float maxZPos;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxYPos;

    public Transform playerRef;

    // Update is called once per frame
    void Update() {
        if (!playerRef) {
            return;
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(playerRef.position.x + 60, minXPos, maxXPos);
        pos.z = Mathf.Clamp(playerRef.position.z - 50, minZPos, maxZPos);
        pos.y = Mathf.Clamp(playerRef.position.y - 32, minYPos, maxYPos);
        transform.position = pos;

    }
}
