using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTeleporter : MonoBehaviour
{
    [SerializeField] string _nextLevelName;
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            WorldTeleport(collision.gameObject);
        }
    }

    public void WorldTeleport(GameObject player)
    {
        SceneManager.LoadScene(_nextLevelName);
    }
}
