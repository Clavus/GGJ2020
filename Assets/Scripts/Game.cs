using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Game : MonoBehaviour
{
	public static Game Instance { get; private set; }

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

	[SerializeField]
	private Texture2D introTexture;
	[SerializeField]
	private Texture2D introBackground;

	public bool IsScenarioActive { get; private set; }

	public Countdown countdown;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		bool useVR = XRSettings.enabled && !forceStartNoVr;
		vrRig.SetActive(useVR);
		noVrRig.SetActive(!useVR);
		if (!useVR)
			XRSettings.enabled = false;

		GameManager.OnGameStateChanged += GameStageChanged;

		if (GameManager.Instance.gameState == GameStates.INTRO)
			paintCanvas.SetCanvasTexture(introBackground, introTexture);
	}

	public void OnButtonPressed()
	{
		if (GameManager.Instance.gameState == GameStates.INTRO)
			GameManager.Instance.ChangeGameState(GameStates.PLAYING);
	}

	private void GameStageChanged(GameStates currentGameState, GameStates newGameState)
	{
		if (newGameState == GameStates.PLAYING)
		{
			StartRandomScenario();
		}
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

		StartCountDown();

		float nextCheck = Time.time + 5f;
		while (true)
		{
			if (nextCheck < Time.time)
			{
				nextCheck = Time.time + 5f;
				var s = System.Diagnostics.Stopwatch.StartNew();
				int startFrameCount = Time.frameCount;
				paintCanvas.CalculateCorrectnessFraction(scenarioData.colorLayer, 0.05f, result =>
				{
					s.Stop();
					Debug.Log($"Correct perc: {result}, Elapsed time: {s.ElapsedMilliseconds}ms, Frames: {Time.frameCount - startFrameCount}");
				});
			}

			yield return null;
		}

		IsScenarioActive = false;
	}

	private void StartCountDown()
	{
		countdown.gameObject.SetActive(true);
	}
}
