using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPress : MonoBehaviour, IInteractable
{
	[SerializeField] private float buttonPressDelay = 0.3f;

	public Transform GrabTransform => throw new System.NotImplementedException();

	public bool IsGrabbed => false;
	public bool CanGrab => false;

	public bool CanInteract => true;

	private float lastButtonPress;
	private Animation _animation;
	private AudioSource _audio;

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
		_audio = GetComponent<AudioSource>();
	}

	private void Press()
	{
		if (lastButtonPress > Time.time - buttonPressDelay)
			return;

		lastButtonPress = Time.time;

		_animation?.Play();
		_audio?.Play();
		//countDownScript.RestartTimer(60);
		//SceneManager.LoadScene(0);
		Game.Instance.OnButtonPressed();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody != null) // Is dynamic object
			Press();
	}
}
