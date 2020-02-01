using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHand
{
	None, Left, Right, Both
}

public class HandInteracter : MonoBehaviour, IInteracter
{
	[SerializeField]
	private EHand hand;
	[SerializeField] private Vector3 grabbableOffset = new Vector3(0, 0.03f, -0.05f);
	[SerializeField] private Vector3 grabbableRotationOffset = new Vector3(-30, 0, 0);

	public EHand Hand => hand;

	private IInteractable interactableInFocus;
	private IInteractable grabbedInteractable;

	void Start()
	{

	}

	void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, hand == EHand.Left ? OVRInput.Controller.LTouch : hand == EHand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.Touch))
		{
			if (grabbedInteractable != null && grabbedInteractable.CanInteract)
				grabbedInteractable.Interact(this);
		}
		else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, hand == EHand.Left ? OVRInput.Controller.LTouch : hand == EHand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.Touch))
		{
			if (grabbedInteractable != null && grabbedInteractable.CanInteract)
				grabbedInteractable.StopInteract();
		}

		if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, hand == EHand.Left ? OVRInput.Controller.LTouch : hand == EHand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.Touch))
		{
			if (interactableInFocus != null && interactableInFocus.CanGrab)
				interactableInFocus.Grab(this);
		}
		else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, hand == EHand.Left ? OVRInput.Controller.LTouch : hand == EHand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.Touch))
		{
			if (grabbedInteractable != null)
				grabbedInteractable.StopGrab();
		}
	}

	public void Attach(IInteractable interactable)
	{
		grabbedInteractable = interactable;
		interactable.GrabTransform.SetParent(transform.parent, false); // Attach to hand anchor
		interactable.GrabTransform.localPosition = grabbableOffset;
		interactable.GrabTransform.localRotation = Quaternion.Euler(grabbableRotationOffset);
	}

	private void OnTriggerEnter(Collider other)
	{
		interactableInFocus = other.GetComponentOrAtBody<IInteractable>();
	}

	private void OnTriggerExit(Collider other)
	{
		if (interactableInFocus == other.GetComponentOrAtBody<IInteractable>())
			interactableInFocus = null;
	}

}
