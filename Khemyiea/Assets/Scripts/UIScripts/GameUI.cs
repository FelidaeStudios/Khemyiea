using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI protonText;
    public TextMeshProUGUI neutronText;
    public TextMeshProUGUI electronText;

    // instance
    public static GameUI instance;

    void Awake()
    {
        instance = this;
    }

    public void UpdateProtonText(int item)
    {
        protonText.text = "" + item;
    }

    public void UpdateNeutronText(int item)
    {
        neutronText.text = "" + item;
    }

    public void UpdateElectronText(int item)
    {
        electronText.text = "" + item;
    }
}