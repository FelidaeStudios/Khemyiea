using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject door;
    public int enoughKeys;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if ((other.gameObject.GetComponent<PlayerController>().key) >= enoughKeys)
            {
                door.SetActive(false);
                transform.position = new Vector3(0, 120, 0);
                other.gameObject.GetComponent<PlayerController>().key -= enoughKeys;
            }
        }
    }
}
