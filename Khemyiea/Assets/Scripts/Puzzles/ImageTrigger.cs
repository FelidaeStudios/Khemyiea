using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTrigger : MonoBehaviour
{
    public GameObject puzzle;
    public GameObject trigger;
    public GameObject blockade;
    public GameObject help;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (puzzle.GetComponent<RotateCheck>().puzzleComplete == true)
        {
            blockade.SetActive(false);
            trigger.SetActive(false);
        }
        else
        {
            help.SetActive(true);
            Invoke("deactivate", 2.0f);
        }
    }

    void deactivate()
    {
        help.SetActive(false);
    }
}
