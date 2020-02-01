using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour, IInteractable
{
	public Color color;
	public int brushSize;

	public Transform GrabTransform => transform;
	public bool CanGrab => true;
	public bool CanInteract => true;

	public void Grab(IInteracter interacter)
	{
		var body = GetComponent<Rigidbody>();
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		body.useGravity = false;
		body.isKinematic = true;
		interacter.Attach(this);
	}

	public void Interact(IInteracter interacter)
	{

	}

	public void StopGrab()
	{
		var body = GetComponent<Rigidbody>();
		body.useGravity = true;
		body.isKinematic = false;
		transform.SetParent(null);
	}

	public void StopInteract()
	{

	}
}
