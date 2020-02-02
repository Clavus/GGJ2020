using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush2000 : MonoBehaviour, IInteractable
{
	[SerializeField] private Color color;
	public int brushSize = 20;
	[SerializeField] private Renderer brushRenderer;
	[SerializeField] private ParticleSystem leakingParticles;
	[SerializeField] private int particleSpotSize = 5;

	public Color Color {
		get { return color; }
		set { color = value; OnPaintChanged(); }
	}

	public Transform GrabTransform => transform;
	public bool IsGrabbed { get; private set; }
	public bool CanGrab => !IsGrabbed;
	public bool CanInteract => true;
	public Vector3 startPosition;
	public Quaternion startRotation;
	private IInteracter currentInteracter;
	private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

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
		currentInteracter = interacter;
		IsGrabbed = true;
		Cursor.visible = false;
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
		currentInteracter = null;
		IsGrabbed = false;
		Cursor.visible = true;
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

	public void OnPaint()
	{
		if (currentInteracter != null)
			currentInteracter.DoFeedback();
	}

	private void OnPaintChanged()
	{
		if (currentInteracter != null)
			currentInteracter.DoFeedback(10);

		UpdateRenderer();
	}

	void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = leakingParticles.GetCollisionEvents(other, collisionEvents);

		for (int i = 0; i < collisionEvents.Count; i++)
		{
			Vector3 pos = collisionEvents[i].intersection;
			if (collisionEvents[i].colliderComponent != null)
			{
				PaintController paintCanvas = collisionEvents[i].colliderComponent.GetComponent<PaintController>();
				if (paintCanvas != null)
					paintCanvas.ApplyPaint(pos, particleSpotSize, color);
			}

		}
	}

	private void UpdateRenderer()
	{
		if (leakingParticles != null)
		{
			leakingParticles.Clear();
			var main = leakingParticles.main;
			main.startColor = color;
			leakingParticles.Play();
		}
		brushRenderer.material.SetColor("_BrushColor", color);
	}
}
