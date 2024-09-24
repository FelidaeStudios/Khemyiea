using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PickupType
{
    Key,
    Health
}

public class ItemCollector : MonoBehaviour
{
    //private int items = 0;

    //[SerializeField] public TextMeshProUGUI itemsText;

    public PickupType type;
    public int value;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (type == PickupType.Key)
                //player("GiveItem", player, value);
                player.GiveKey(value);
            else if (type == PickupType.Health)
                //player("Heal", player, value);
                player.Heal(value);
            Destroy(gameObject);
        }
    }
}