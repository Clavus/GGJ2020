using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Game : MonoBehaviour
{
	[SerializeField]
	private GameObject vrRig;
	[SerializeField]
	private GameObject noVrRig;
	[SerializeField]
	private bool forceStartNoVr;

	private void Start()
	{
		bool useVR = XRSettings.enabled && !forceStartNoVr;
		vrRig.SetActive(useVR);
		noVrRig.SetActive(!useVR);
		if (!useVR)
			XRSettings.enabled = false;
	}
}
