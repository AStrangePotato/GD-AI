using UnityEngine;

public class RestartAudio : MonoBehaviour {
    AudioSource audioSource;
    public Movement movement;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (movement.runOver) // press R to restart audio
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }
}