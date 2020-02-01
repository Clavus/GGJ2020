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
	private bool thresholdReached;

	public void SetThreshold(float threshold)
	{
		this.threshold = threshold;
		scoreScalarThresholdTransform.localPosition = new Vector3(0, threshold, 0);
	}

	public void UpdateScore(float score)
	{
		targetProgress = score;
	}

	public void DoFail()
	{
		failAudio?.Play();
	}

	private void Update()
	{
		progress = Mathf.MoveTowards(progress, targetProgress, 0.5f * Time.deltaTime);
		if (progress > 0)
		{
			scoreBarRenderer.material.SetColor("_BaseColor", barGradient.Evaluate(progress));
			scoreScalarTransform.localScale = new Vector3(1, progress, 1);
		}
		if (!thresholdReached && progress > threshold)
		{
			thresholdReached = true;
			winAudio?.Play();
			winConfetti?.Play();
		}
	}

	public void ResetBar()
	{
		thresholdReached = false;
		targetProgress = 0;
		progress = 0;
		scoreScalarTransform.localScale = Vector3.zero;
	}
}
