using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteracter : MonoBehaviour, IInteracter
{
	public Camera NoVRCamera;
	public float defaultDistance = 0.1f;
	public float offsetOnClick = -0.1f;
	[SerializeField] private Vector3 grabbableOffset = new Vector3(0, 0, -0.25f);
	[SerializeField] private Vector3 grabbableRotationOffset = new Vector3(-75, 0, 0);
    [SerializeField] private GameObject Hitmarker;

	private IInteractable grabbedInteractable;

	private bool objectHeld;
	private bool objectExtended;
	private LayerMask grabMask;
	private LayerMask paintMask;
    private GameObject hitmarker;

	void Start()
	{
		grabMask = LayerMask.GetMask("Interactable");
		paintMask = LayerMask.GetMask("PaintCanvas", "PaintBucket");
	}

	private void Update()
	{
		float offsetDistance = (!objectExtended ? 0 : offsetOnClick);
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
                        StartCoroutine(MouseToMiddle());
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

        if (objectExtended)
        {
            if (hasHit)
                transform.position = (mouseRayHit.point + mouseRayHit.normal * (defaultDistance + offsetDistance)) + new Vector3(0, 0.065f, 0);
            else
                transform.position = (ray.origin + ray.direction) + new Vector3(0, 0.05f, -0.1f);
            //transform.position = ray.origin + ray.direction * castDistance;
            if (hitmarker != null)
                hitmarker.transform.position = new Vector3(0, -30, 0);
        }
        else
        {
            transform.position = new Vector3(ray.origin.x + ray.direction.x, ray.origin.y + ray.direction.y, 0.95f);

            if (hasHit && objectHeld)
            {
                if (hitmarker == null)
                    hitmarker = Instantiate(Hitmarker);
                hitmarker.transform.position = mouseRayHit.point;
                hitmarker.transform.rotation = mouseRayHit.collider.transform.rotation;
            }
            else
            {
                if (hitmarker != null)
                    hitmarker.transform.position = new Vector3(0, -30, 0);
            }
        }

		if (objectHeld)
		{
			grabbedInteractable.GrabTransform.localPosition = grabbableOffset;
			grabbedInteractable.GrabTransform.localRotation = Quaternion.Euler(grabbableRotationOffset);
		}
	}

	void FixedUpdate()
	{

	}

	public void Attach(IInteractable interactable)
	{
		grabbedInteractable = interactable;
		interactable.GrabTransform.SetParent(this.transform.GetChild(0).transform, false);
		interactable.GrabTransform.localPosition = grabbableOffset;
		interactable.GrabTransform.localRotation = Quaternion.Euler(grabbableRotationOffset);
	}

	public void DoFeedback(float multiplier = 1f)
	{

	}

    IEnumerator MouseToMiddle()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yield return null;
        Cursor.lockState = CursorLockMode.None;
    }
}

public enum MouseButton
{
	LEFT,
	RIGHT,
	MIDDLE
}
