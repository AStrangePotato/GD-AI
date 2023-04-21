using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject target; // reference to the player's Transform component
    public float smoothing; // the rate at which the camera moves towards the target position
    Movement movement;
    private Vector3 offset; // the distance between the camera and the player

    void Start() {
        offset = transform.position - target.transform.position;
        movement = target.GetComponent<Movement>();
    }

    void FixedUpdate() {
        Vector3 targetCamPos = target.transform.position + offset;
        if (movement.CurrentGamemode == Gamemodes.Ship) {
            targetCamPos.y = 0f;
        }
        targetCamPos.y = Mathf.Clamp(targetCamPos.y, -2.16f, 4.5f);
        
        transform.position = Vector3.Lerp(transform.position, targetCamPos, Time.deltaTime * smoothing);

    }
}
