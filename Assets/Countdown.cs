using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public float timerSeconds;
    private float _timerSeconds;
    private float _fillAmountInterval;
    private Image _image;

    // Start is called before the first frame update
    void Awake()
    {
        _image = GetComponent<Image>();
        _timerSeconds = timerSeconds;
        _fillAmountInterval = _image.fillAmount / timerSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        _timerSeconds -= Time.deltaTime;
        _image.fillAmount -= ();
        if(_timerSeconds <= 0)
        {
            //Game over!
        }
    }
}
