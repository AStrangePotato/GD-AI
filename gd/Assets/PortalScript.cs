using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public Gamemodes Gamemode;
    public Speeds Speed;
    public bool Gravity;
    public int key;

    private void OnTriggerEnter2D(Collider2D collision) {
        try {
            Movement movement = collision.gameObject.GetComponent<Movement>();
            movement.EnterPortal(Gamemode, Speed, Gravity ? 1:-1, key);
        }

        catch {

        }
    }
}
