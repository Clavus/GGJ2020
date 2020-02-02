using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollection : MonoBehaviour
{
	[SerializeField] private Rigidbody[] stars;
	[SerializeField] private float launchForce = 10;
	private int starsLeft;
	private Vector3[] startPositions;
	private Quaternion[] startRotations;

	private void Start()
	{
		starsLeft = stars.Length;
		startPositions = new Vector3[stars.Length];
		startRotations = new Quaternion[stars.Length];
		for (int i = 0; i < stars.Length; i++)
		{
			startPositions[i] = stars[i].position;
			startRotations[i] = stars[i].rotation;
		}
	}

	public void UpdateStars()
	{
		int livesLeft = Game.Instance.LivesLeft;

		while (starsLeft > livesLeft)
			PopStar();
	}

	public void Restart()
	{
		for (int i = 0; i < stars.Length; i++)
		{
			stars[i].isKinematic = true;
			stars[i].useGravity = false;
			stars[i].velocity = Vector3.zero;
			stars[i].angularVelocity = Vector3.zero;
			stars[i].MovePosition(startPositions[i]);
			stars[i].MoveRotation(startRotations[i]);
		}
		starsLeft = stars.Length;
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
