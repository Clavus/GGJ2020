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
	private float lastFeedbackTime;

	void Start()
	{

	}

	void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, GetControllerEnum()))
		{
			if (grabbedInteractable != null && grabbedInteractable.CanInteract)
				grabbedInteractable.Interact(this);
		}
		else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, GetControllerEnum()))
		{
			if (grabbedInteractable != null && grabbedInteractable.CanInteract)
				grabbedInteractable.StopInteract();
		}

		if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, GetControllerEnum()))
		{
			if (interactableInFocus != null && interactableInFocus.CanGrab)
				interactableInFocus.Grab(this);
		}
		else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, GetControllerEnum()))
		{
			if (grabbedInteractable != null)
				grabbedInteractable.StopGrab();
		}

		if (lastFeedbackTime < Time.time - 0.05f)
			OVRInput.SetControllerVibration(0, 0, GetControllerEnum());
	}

	public void Attach(IInteractable interactable)
	{
		grabbedInteractable = interactable;
		interactable.GrabTransform.SetParent(transform.parent, false); // Attach to hand anchor
		interactable.GrabTransform.localPosition = grabbableOffset;
		interactable.GrabTransform.localRotation = Quaternion.Euler(grabbableRotationOffset);
	}

	public void DoFeedback()
	{
		OVRInput.SetControllerVibration(0.1f, 0.1f, GetControllerEnum());
		lastFeedbackTime = Time.time;
	}

	private OVRInput.Controller GetControllerEnum()
	{
		return hand == EHand.Left ? OVRInput.Controller.LTouch : hand == EHand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.Touch;
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
