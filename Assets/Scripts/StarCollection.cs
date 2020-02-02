using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollection : MonoBehaviour
{
	[SerializeField] private Rigidbody[] stars;
	[SerializeField] private float launchForce = 10;
	private int starsLeft;

	private void Start()
	{
		starsLeft = stars.Length;
	}

	public void UpdateStars()
	{
		int livesLeft = Game.Instance.LivesLeft;

		while (starsLeft > livesLeft)
			PopStar();
	}

	private void PopStar()
	{
		starsLeft--;
		var star = stars[starsLeft];
		star.isKinematic = false;
		star.useGravity = true;
		star.AddForce((Random.onUnitSphere + Vector3.up * 1.5f).normalized * launchForce, ForceMode.Impulse);
		star.AddTorque(Random.onUnitSphere, ForceMode.Impulse);
	}
}
