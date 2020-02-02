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
        float castDistance = 4f;

        Ray ray = NoVRCamera.ScreenPointToRay(Input.mousePosition);
        LayerMask maskToCast = (!objectHeld ? grabMask : paintMask);

        RaycastHit[] mouseRayHits = Physics.RaycastAll(ray, castDistance, maskToCast.value);
        bool hasHit = mouseRayHits.Length > 0;
        Debug.Log("Hit? " + hasHit);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue);
        if (hasHit)
            Debug.DrawLine(ray.origin, mouseRayHits[0].point, Color.red);

        if (Input.GetMouseButtonDown((int)MouseButton.LEFT) && !objectHeld)
        {
            if (hasHit)
            {
                IInteractable interactable = mouseRayHits[0].collider.GetComponentOrAtBody<IInteractable>();
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
            if (hasHit && mouseRayHits.Length >= 2)
                transform.position = (mouseRayHits[1].point + mouseRayHits[1].normal * (defaultDistance + offsetDistance)) + new Vector3(0, 0.065f, 0);
            else
                transform.position = (ray.origin + ray.direction);
            //transform.position = ray.origin + ray.direction * castDistance;
            if (hitmarker != null)
                hitmarker.transform.position = new Vector3(0, -30, 0);
        }
        else
        {
            if (hasHit)
            {
                transform.position = (mouseRayHits[0].point + mouseRayHits[0].normal * (defaultDistance + offsetDistance));
                if (mouseRayHits.Length >= 2)
                {
                    if (hitmarker == null)
                        hitmarker = Instantiate(Hitmarker);
                    hitmarker.transform.position = mouseRayHits[1].point;
                    hitmarker.transform.rotation = mouseRayHits[1].transform.rotation;

                }
                else
                {
                    if (hitmarker != null)
                        hitmarker.transform.position = new Vector3(0, -30, 0);
                }
            }
            else
            {
                transform.position = ray.origin + ray.direction;
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
