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
		transform.localPosition = new Vector3(0, 0.03f, -0.05f);
        transform.localRotation = Quaternion.Euler(-30, 0, 0);
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
        transform.localRotation = Quaternion.identity;
		transform.SetParent(null);
	}

	public void StopInteract()
	{

	}
}
