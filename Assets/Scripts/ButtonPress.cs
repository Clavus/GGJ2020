using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPress : MonoBehaviour, IInteractable
{
	public Countdown countDownScript;
	public PaintingSwitcher paintingSwitcher;

	public Transform GrabTransform => throw new System.NotImplementedException();

	public bool CanGrab => false;

	public bool CanInteract => true;

	private Animation _animation;


	public void Grab(IInteracter interacter)
	{
	}

	public void Interact(IInteracter interacter)
	{
		Press();
	}

	public void StopGrab()
	{
	}

	public void StopInteract()
	{
		throw new System.NotImplementedException();
	}

	public void Respawn()
	{

	}

	private void Awake()
	{
		_animation = GetComponent<Animation>();
		if (_animation == null)
			Debug.Log("No animation found");
	}

	private void Press()
	{
		_animation.Play();
		// TODO: Some kind of trigger to end the game
		paintingSwitcher.SwitchPainting();
		//countDownScript.RestartTimer(60);
		//SceneManager.LoadScene(0);
	}
}
