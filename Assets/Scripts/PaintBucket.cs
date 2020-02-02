using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBucket : MonoBehaviour
{
	public Color paintbucketColor;
	[SerializeField] private float particleSpotSize = 20f;

	private AudioSource soundEffect;
	private ParticleSystem splash;
	private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

	private void Awake()
	{
		soundEffect = GetComponent<AudioSource>();
	}

	private void Start()
	{
		splash = GetComponentInChildren<ParticleSystem>();
	}

	public void ChangePaintOnBrush(PaintBrush2000 brush)
	{
		brush.Color = paintbucketColor;
		Debug.Log("Changed paintbrush color to: " + paintbucketColor);
	}

	private void OnTriggerEnter(Collider other)
	{
		PaintBrush2000 paintBrush = other.GetComponentOrAtBody<PaintBrush2000>();

		if (paintBrush != null)
		{
			ChangePaintOnBrush(paintBrush);
			soundEffect.pitch = 0.8f + Random.value * 0.4f;
			soundEffect.PlayOneShot(soundEffect.clip);
			splash.Play();
		}
	}

	void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = splash.GetCollisionEvents(other, collisionEvents);

		for (int i = 0; i < collisionEvents.Count; i++)
		{
			Vector3 pos = collisionEvents[i].intersection;
			PaintController paintCanvas = collisionEvents[i].colliderComponent.GetComponent<PaintController>();
			//Debug.Log($"Particle collision with {other.name}");

			if (paintCanvas != null)
			{
				paintCanvas.ApplyPaint(pos, (int)(particleSpotSize * 0.8f + (particleSpotSize * 0.4f * Random.value)), paintbucketColor);
				//Debug.Log($"Applying particle[{i}] paint to {paintCanvas.gameObject.name}");
			}
		}
	}

}
