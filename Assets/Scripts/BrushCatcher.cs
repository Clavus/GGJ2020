using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushCatcher : MonoBehaviour
{
    public Transform respawnLocation;
    private void OnTriggerEnter(Collider other)
    {
        // Catch all interactibles and respawn;
        if(other.gameObject.layer == 10)
        {
            other.gameObject.transform.parent.position = respawnLocation.position;
            other.gameObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
