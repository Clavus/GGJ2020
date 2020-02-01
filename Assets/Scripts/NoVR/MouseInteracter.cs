using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteracter : MonoBehaviour, IInteracter
{
	public Camera NoVRCamera;
	public float defaultDistance = 0.1f;
	public float offsetOnClick = -0.1f;

	private IInteractable grabbedInteractable;

	private bool objectHeld;
	private bool objectExtended;
	private LayerMask grabMask;
	private LayerMask paintMask;

	void Start()
	{
		grabMask = LayerMask.GetMask("Interactable");
		paintMask = LayerMask.GetMask("PaintCanvas", "PaintBucket");
	}

	private void Update()
	{
		float offsetDistance = 0;
		float castDistance = 2f;

		Ray ray = NoVRCamera.ScreenPointToRay(Input.mousePosition);
		LayerMask maskToCast = (!objectHeld ? grabMask : paintMask);
		RaycastHit mouseRayHit;

		bool hasHit = Physics.Raycast(ray, out mouseRayHit, castDistance, maskToCast.value);
		Debug.Log("Hit? " + hasHit);
		Debug.DrawRay(ray.origin, ray.direction, Color.blue);
		Debug.DrawLine(ray.origin, mouseRayHit.point, Color.red);

		if (Input.GetMouseButtonDown((int)MouseButton.LEFT) && !objectHeld)
		{
			if (hasHit)
			{
				IInteractable interactable = mouseRayHit.collider.GetComponentOrAtBody<IInteractable>();
				if (interactable != null)
				{
					grabbedInteractable = interactable;
					if (grabbedInteractable.CanGrab)
					{
						objectHeld = true;
						grabbedInteractable.Grab(this);
						Debug.Log("Hit brush");
					}
					else
					{
						grabbedInteractable.Interact(this);
					}

				}
			}
		}
		else if (Input.GetMouseButtonDown((int)MouseButton.LEFT) && objectHeld)
		{
			objectHeld = false;
			grabbedInteractable.StopGrab();
		}
		else if (Input.GetMouseButtonDown((int)MouseButton.RIGHT) && objectHeld && !objectExtended)
		{
			objectExtended = true;
			offsetDistance = offsetOnClick;
		}
		else if (Input.GetMouseButtonUp((int)MouseButton.RIGHT) && objectHeld && objectExtended)
		{
			objectExtended = false;
			offsetDistance = 0;
		}

		if (hasHit)
			transform.position = mouseRayHit.point + mouseRayHit.normal * (defaultDistance + offsetDistance);
		else
			transform.position = ray.origin + ray.direction * castDistance;
	}

	void FixedUpdate()
	{

	}

	public void Attach(IInteractable interactable)
	{
		grabbedInteractable = interactable;
		interactable.GrabTransform.SetParent(this.transform.GetChild(0).transform, false);
	}

	private void MoveInteractable(IInteractable interactable, float zOffset)
	{
		interactable.GrabTransform.GetComponent<Transform>().position += new Vector3(0, 0, zOffset);
	}
}

public enum MouseButton
{
	LEFT,
	RIGHT,
	MIDDLE
}
