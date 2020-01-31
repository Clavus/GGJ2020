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


    // Start is called before the first frame update
    void Awake()
    {
        CountdownSettings();
    }

    private void CountdownSettings()
    {
        _timerSeconds = timerSeconds;
        _fillAmountInterval = 1 / timerSeconds;
        _tickerRotationInterval = 360 / timerSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            _timerSeconds -= Time.deltaTime;
            tickerTrail.fillAmount += (_fillAmountInterval * Time.deltaTime);
            tickerRect.Rotate(0, 0, -(_tickerRotationInterval * Time.deltaTime));
        }

        if(_timerSeconds <= 0)
        {
            active = false;
            //Game over!
        }
    }

    public void RestartTimer(float time)
    {
        timerSeconds = time;
        CountdownSettings();
        active = true;
    }
}
