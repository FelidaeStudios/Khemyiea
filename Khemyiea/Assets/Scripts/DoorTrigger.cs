using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject door;
    public int enoughKeys;
    //public GameObject noKeys;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if ((other.gameObject.GetComponent<PlayerController>().key) >= enoughKeys)
            {
                door.SetActive(false);
                //setMazeScreen();
                transform.position = new Vector3(0, 120, 0);
                //mazeIntro.SetActive(true);
                //Invoke("setMazeScreen", 3.0f);
            }
            /*else
            {
                noKeys.SetActive(true);
                Invoke("setBadScreen", 3.0f);
            }*/
        }
    }
}
