using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBar : MonoBehaviour
{
	[SerializeField] private Transform scoreScalarTransform;
	[SerializeField] private Transform scoreScalarThresholdTransform;
	[SerializeField] private Renderer scoreBarRenderer;
	[SerializeField] private Gradient barGradient;
	[SerializeField] private AudioSource winAudio;
	[SerializeField] private ParticleSystem winConfetti;
	[SerializeField] private AudioSource failAudio;

	private float progress;
	private float targetProgress;
	private float threshold = 0.9f;

	public void SetThreshold(float threshold)
	{
		this.threshold = threshold;
		scoreScalarThresholdTransform.localPosition = new Vector3(0, threshold, 0);
	}

	public void UpdateScore(float score)
	{
		targetProgress = score;
	}

	public void DoResult()
	{
		if (progress >= threshold)
		{
			winAudio?.Play();
			winConfetti?.Play();
		}
		else
		{
			failAudio?.Play();
		}
	}

	private void Update()
	{
		progress = Mathf.MoveTowards(progress, targetProgress, 0.25f * Time.deltaTime);
		if (progress > 0)
			scoreBarRenderer.material.SetColor("_BaseColor", barGradient.Evaluate(progress));
	}

	public void ResetBar()
	{
		targetProgress = 0;
		progress = 0;
		scoreScalarTransform.localScale = new Vector3(1, 0, 1);
	}
}
