using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush2000 : MonoBehaviour, IInteractable
{
	[SerializeField] private Color color;
	public int brushSize;
	[SerializeField] private Renderer brushRenderer;

	public Color Color {
		get { return color; }
		set { color = value; UpdateRenderer(); }
	}

	public Transform GrabTransform => transform;
	public bool CanGrab => true;
	public bool CanInteract => true;
	public Vector3 startPosition;
	public Quaternion startRotation;

	void Start()
	{
		startPosition = transform.position;
		startRotation = transform.rotation;
		UpdateRenderer();
	}

	public void Grab(IInteracter interacter)
	{
		var body = GetComponent<Rigidbody>();
		body.useGravity = false;
		body.isKinematic = true;
		interacter.Attach(this);
	}

	public void Interact(IInteracter interacter)
	{

	}

	public void StopGrab()
	{
		var body = GetComponent<Rigidbody>();
		body.useGravity = true;
		body.isKinematic = false;
		transform.SetParent(null);
	}

	public void StopInteract()
	{

	}

	public void Respawn()
	{
		var body = GetComponent<Rigidbody>();
		body.useGravity = true;
		body.isKinematic = false;
		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
		transform.SetParent(null);
		transform.rotation = startRotation;
		transform.position = startPosition;
	}

	private void UpdateRenderer()
	{
		brushRenderer.material.SetColor("_BrushColor", color);
	}
}
