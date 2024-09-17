using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{

    [Header("Player")]
    public GameObject player;
    public Transform[] spawnPoint;
    public float respawnTime;
    //public int curHp;
    //public int maxHp;
    //public HealthBar healthBar;
    //public PlayerController player;


    void Start()
    {
        SpawnPlayer();
        //player = 
    }

    void SpawnPlayer()
    {
        //Vector2 position = new Vector2(8, 0);
        Instantiate(player, spawnPoint[Random.Range(0, spawnPoint.Length)].position, Quaternion.identity);
    }
}