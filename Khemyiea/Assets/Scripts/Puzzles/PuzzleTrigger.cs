using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    public GameObject hud;
    public GameObject puzzle;

    public void PuzzleStart()
    {
        hud.SetActive(false);
        puzzle.SetActive(true);
        Time.timeScale = 0f;
    }
}
