using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Collider collider;
    private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        if (_animation == null) Debug.Log("No animation found");
        if (collider == null) Debug.Log("No collider specified");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == collider)
        {
            _animation.Play();
            // TODO: Some kind of trigger to end the game
        }
    }
}
