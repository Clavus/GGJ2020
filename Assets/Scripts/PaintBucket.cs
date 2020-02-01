﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBucket : MonoBehaviour
{
	public Color paintbucketColor;
	private AudioSource soundEffect;
	private Renderer _renderer;

	private void Awake()
	{
		_renderer = GetComponentInChildren<Renderer>();
		if (_renderer != null)
			_renderer.material.SetColor("_BaseColor", paintbucketColor);
		soundEffect = GetComponent<AudioSource>();
	}

	public void ChangePaintOnBrush(PaintBrush2000 brush)
	{
		brush.color = paintbucketColor;
		Debug.Log("Changed paintbrush color to: " + paintbucketColor);
	}

	private void OnTriggerEnter(Collider other)
	{
		PaintBrush2000 paintBrush = other.GetComponentOrAtBody<PaintBrush2000>();

		if (paintBrush != null)
		{
			ChangePaintOnBrush(paintBrush);
			soundEffect.PlayOneShot(soundEffect.clip);
		}
	}
}
