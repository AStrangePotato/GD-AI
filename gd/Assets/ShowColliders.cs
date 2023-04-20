using UnityEngine;

public class ShowColliders : MonoBehaviour {
    public bool showColliders = true;

    private void OnDrawGizmos() {
        if (showColliders) {
            Collider2D[] colliders = FindObjectsOfType<Collider2D>();
            foreach (Collider2D collider in colliders) {
                if (collider.enabled) {
                    Gizmos.color = new Color(220f/255f, 20f/255f, 60f/255f);
                    Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
                }
            }
        }
    }
}