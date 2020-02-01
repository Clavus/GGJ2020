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
	[SerializeField]
	private PaintController paintCanvas;
	[SerializeField]
	private ScenarioSO[] scenarios;

	public bool IsScenarioActive { get; private set; }

	private void Start()
	{
		bool useVR = XRSettings.enabled && !forceStartNoVr;
		vrRig.SetActive(useVR);
		noVrRig.SetActive(!useVR);
		if (!useVR)
			XRSettings.enabled = false;

		StartRandomScenario();
	}

	private void StartRandomScenario()
	{
		if (IsScenarioActive)
		{
			Debug.LogError("Scenario already active!");
			return;
		}

		StartCoroutine(ScenarioRoutine(scenarios[Random.Range(0, scenarios.Length - 1)]));
	}

	private IEnumerator ScenarioRoutine(ScenarioSO scenarioData)
	{

		IsScenarioActive = true;
		paintCanvas.SetCanvasTexture(scenarioData.incorrectColorLayers[Random.Range(0, scenarioData.incorrectColorLayers.Length - 1)],
			scenarioData.lineArt);

		float nextCheck = Time.time + 5f;
		while (true)
		{
			if (nextCheck < Time.time)
			{
				nextCheck = Time.time + 5f;
				Debug.Log("Correct perc: " + paintCanvas.GetCorrectnessFraction(scenarioData.colorLayer));
			}

			yield return null;
		}

		IsScenarioActive = false;
	}
}
