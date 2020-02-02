using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
	public float timerSeconds;
	public Image tickerTrail;
	public RectTransform tickerRect;
	public bool active;

	private float _timerSeconds;
	private float _fillAmountInterval;
	private float _tickerRotationInterval;

	private AudioSource audioSource;

	// Start is called before the first frame update
	void Awake()
	{
		CountdownSettings();
		audioSource = GetComponent<AudioSource>();
	}

	private void CountdownSettings()
	{
		_timerSeconds = timerSeconds;
		_fillAmountInterval = 1 / timerSeconds;
		_tickerRotationInterval = 360 / timerSeconds;

		tickerTrail.fillAmount = 0;
		tickerRect.localEulerAngles = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
		if (active)
		{
			_timerSeconds -= Time.deltaTime;
			tickerTrail.fillAmount += (_fillAmountInterval * Time.deltaTime);
			tickerRect.Rotate(0, 0, -(_tickerRotationInterval * Time.deltaTime), Space.Self);
		}

		if (_timerSeconds <= 0)
		{
			active = false;
			Game.Instance.OnCountdownEnded();
		}
	}

	public void RestartTimer(float time)
	{
		timerSeconds = time;
		CountdownSettings();
		active = true;
		StartCoroutine(PlayClockSound());
	}

	IEnumerator PlayClockSound()
	{
		float maxDelay = 3f;
		float delay = maxDelay;
		while (active)
		{
			if (_timerSeconds > timerSeconds / 2)
			{
				audioSource.PlayOneShot(audioSource.clip);
				yield return new WaitForSeconds(maxDelay);
			}
			else if (_timerSeconds < timerSeconds / 2)
			{
				delay = maxDelay - Mathf.Lerp(maxDelay - .1f, 0, (_timerSeconds / (timerSeconds / 2)));
				audioSource.PlayOneShot(audioSource.clip);
				yield return new WaitForSeconds(delay);
			}
		}
	}
}
