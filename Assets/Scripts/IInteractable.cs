
using UnityEngine;

public interface IInteractable
{
	Transform GrabTransform { get; }

	bool CanGrab { get; }
	void Grab(IInteracter interacter);
	void StopGrab();
	bool CanInteract { get; }
	void Interact(IInteracter interacter);
	void StopInteract();
}

public interface IInteracter
{
	void Attach(IInteractable interactable);
}