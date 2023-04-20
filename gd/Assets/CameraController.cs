using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target; // reference to the player's Transform component
    public float smoothing = 3.0f; // the rate at which the camera moves towards the target position

    private Vector3 offset; // the distance between the camera and the player

    void Start() {
        offset = transform.position - target.position;
    }

    void FixedUpdate() {
        Vector3 targetCamPos = target.position + offset;
        targetCamPos.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
