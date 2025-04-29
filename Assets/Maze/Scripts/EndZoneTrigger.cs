using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZoneTrigger : MonoBehaviour
{
    public string playerTag = "Player";
    //public string endZoneMessage = "End Zone Reached!";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("End Zone Reached!");
        }
    }
}
