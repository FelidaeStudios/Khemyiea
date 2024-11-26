using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject teleportLocation;
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Teleport(collision.gameObject);
        }
    }

    public void Teleport(GameObject player)
    {
        player.transform.position = teleportLocation.transform.position;
    }
}
