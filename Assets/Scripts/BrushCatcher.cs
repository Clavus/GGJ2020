using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushCatcher : MonoBehaviour
{
	public Transform respawnLocation;
	private void OnTriggerEnter(Collider other)
	{
		IInteractable interactable = other.GetComponentOrAtBody<IInteractable>();

		// Catch all interactibles and respawn;
		if (interactable != null && !interactable.IsGrabbed)
			interactable.Respawn();
	}
}
