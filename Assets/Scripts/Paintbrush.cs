using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour, IInteractable
{
	public Color color;

	public Rigidbody Body => GetComponent<Rigidbody>();
	public bool CanGrab => true;
	public bool CanInteract => true;

	public void Grab(IInteracter interacter)
	{
		interacter.Attach(this);
	}

	public void Interact(IInteracter interacter)
	{

	}

	public void StopGrab()
	{
		Body.useGravity = true;
		Body.transform.SetParent(null);
	}

	public void StopInteract()
	{

	}
}
