using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPress : MonoBehaviour, IInteractable
{
    public Collider collider;
    private Animation _animation;

    public Transform GrabTransform => throw new System.NotImplementedException();

    public bool CanGrab => false;

    public bool CanInteract => true;

    public void Grab(IInteracter interacter)
    {
    }

    public void Interact(IInteracter interacter)
    {
        Press();
    }

    public void StopGrab()
    {
    }

    public void StopInteract()
    {
        throw new System.NotImplementedException();
    }

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
            Press();
        }
    }

    private void Press()
    {
        _animation.Play();
        // TODO: Some kind of trigger to end the game
        SceneManager.LoadScene(0);
    }
}
