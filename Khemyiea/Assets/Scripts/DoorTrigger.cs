using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject door;
    public int enoughProtons;
    public int enoughNeutrons;
    public int enoughElectrons;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if ((other.gameObject.GetComponent<PlayerController>().proton) >= enoughProtons && (other.gameObject.GetComponent<PlayerController>().neutron) >= enoughNeutrons && (other.gameObject.GetComponent<PlayerController>().electron) >= enoughElectrons)
            {
                other.gameObject.GetComponent<PlayerController>().proton -= enoughProtons;
                other.gameObject.GetComponent<PlayerController>().neutron -= enoughNeutrons;
                other.gameObject.GetComponent<PlayerController>().electron -= enoughElectrons;
                door.SetActive(false);
                transform.position = new Vector3(0, 120, 0);
                GameUI.instance.UpdateProtonText(other.gameObject.GetComponent<PlayerController>().proton);
                GameUI.instance.UpdateNeutronText(other.gameObject.GetComponent<PlayerController>().neutron);
                GameUI.instance.UpdateElectronText(other.gameObject.GetComponent<PlayerController>().electron);
            }
        }
    }
}
