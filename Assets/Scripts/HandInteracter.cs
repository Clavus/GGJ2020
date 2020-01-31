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

	public EHand Hand => hand;

	private IInteractable interactableInFocus;

	void Start()
	{

	}

	void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, hand == EHand.Left ? OVRInput.Controller.LTouch : hand == EHand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.Touch))
		{
			Debug.Log("Trigger pulled on " + hand);
		}

		if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, hand == EHand.Left ? OVRInput.Controller.LTouch : hand == EHand.Right ? OVRInput.Controller.RTouch : OVRInput.Controller.Touch))
		{
			Debug.Log("Hand grab pulled on " + hand);
			if (interactableInFocus != null)
				interactableInFocus.Interact(this);
		}

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
