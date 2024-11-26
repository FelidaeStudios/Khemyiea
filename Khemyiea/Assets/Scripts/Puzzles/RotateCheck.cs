using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCheck : MonoBehaviour
{
    public GameObject[] rowA;
    public GameObject[] rowB;
    public GameObject barrier;
    public GameObject puzzleScreen;
    public GameObject hud;

    public bool Acheck = true;
    public bool Bcheck = true;
    public bool puzzleComplete = false;

    void Start()
    {
        Acheck = testing(rowA, ref Acheck);
        Bcheck = testing(rowB, ref Bcheck);
    }

    void Update()
    {
        Acheck = testing(rowA, ref Acheck);
        Bcheck = testing(rowB, ref Bcheck);
        puzzleComplete = Acheck && Bcheck;
        if (puzzleComplete == true)
        {
            Invoke("deactivate", 2.0f);
            EndPuzzle();
        }
    }

    void deactivate()
    {
        //keyGrab.SetActive(false);
    }

    void EndPuzzle()
    {
        barrier.SetActive(false);
        puzzleScreen.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f;
    }

    bool testing(GameObject[] row, ref bool check)
    {
        foreach (GameObject block in row)
        {
            if (!block.GetComponent<ImageRotate>().correct)
            {
                return false;
            }
        }
        return true;
    }
}
