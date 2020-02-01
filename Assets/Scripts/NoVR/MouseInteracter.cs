using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteracter : MonoBehaviour, IInteracter
{
    Vector3 position;
    public Camera NoVRCamera;

    private IInteractable grabbedInteractable;

    private bool objectHeld;
    private bool objectExtended;

    void Start()
    {
        
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 1f;
        this.transform.position = NoVRCamera.ScreenToWorldPoint(mousePosition);

        int layerMask = 1 << 10;

        if (Input.GetMouseButtonDown((int)MouseButton.LEFT) && !objectHeld)
        {
            Ray ray = NoVRCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseRay;
            if (Physics.Raycast(ray, out mouseRay, 10, layerMask))
            {
                if (mouseRay.transform.GetComponent<IInteractable>() != null)
                {
                    grabbedInteractable = mouseRay.transform.GetComponent<IInteractable>();
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
            MoveInteractable(grabbedInteractable, .05f);
        }
        else if (Input.GetMouseButtonUp((int)MouseButton.RIGHT) && objectHeld && objectExtended)
        {
            objectExtended = false;
            MoveInteractable(grabbedInteractable, -.05f);
        }
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
