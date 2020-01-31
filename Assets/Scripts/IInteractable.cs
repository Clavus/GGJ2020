
public interface IInteractable
{
	bool CanInteract { get; }
	void Interact(IInteracter interacter);
	void StopInteract();
}

public interface IInteracter
{

}