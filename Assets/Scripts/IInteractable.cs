
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
	void Respawn();
}

public interface IInteracter
{
	void Attach(IInteractable interactable);
	void DoFeedback(float multiplier = 1f);
}