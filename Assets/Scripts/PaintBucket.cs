using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBucket : MonoBehaviour
{
	public Color paintbucketColor;
	private AudioSource soundEffect;
    private ParticleSystem splash;

	private void Awake()
	{
		soundEffect = GetComponent<AudioSource>();
	}

    private void Start()
    {
        splash = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mainParticle = splash.main;
        mainParticle.startColor = paintbucketColor;
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
}
