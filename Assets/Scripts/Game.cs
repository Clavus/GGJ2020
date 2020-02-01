using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

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
	private float correctnessThreshold = 0.8f;

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
			StartRandomScenario(1);
		}
	}

	private void StartRandomScenario(int difficulty)
	{
		if (IsScenarioActive)
		{
			Debug.LogError("Scenario already active!");
			return;
		}

		ScenarioSO[] scenariosAtDifficulty = scenarios.Where(x => x.difficulty == difficulty).ToArray();
		ScenarioSO randomScenario = scenariosAtDifficulty[Random.Range(0, scenariosAtDifficulty.Length - 1)];

		StartCoroutine(ScenarioRoutine(randomScenario));
	}

	private IEnumerator ScenarioRoutine(ScenarioSO scenarioData)
	{
		IsScenarioActive = true;
		Texture2D paintingToDisplay = scenarioData.incorrectColorLayers[Random.Range(0, scenarioData.incorrectColorLayers.Length - 1)];
		paintCanvas.SetCanvasTexture(paintingToDisplay, scenarioData.lineArt);

		StartCountDown();

		float nextCheck = Time.time + 5f;
		while (true)
		{
#if UNITY_EDITOR
			// Test correcctness periodically
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
#endif
			yield return null;
		}

		IsScenarioActive = false;
	}

	private void StartCountDown()
	{
		countdown.gameObject.SetActive(true);
	}
}
