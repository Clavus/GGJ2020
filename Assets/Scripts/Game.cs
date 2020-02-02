using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;
using System.Timers;

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
	private ScoreBar scoreBar;
	[SerializeField]
	private Countdown countdown;
	[SerializeField]
	private StarCollection starCollection;
	[SerializeField]
	private ScenarioSO[] scenarios;
	[SerializeField]
	private int[] difficultiesToBeat = new int[] { 1, 1, 2, 2, 3, 3 };
	[SerializeField]
	private int startLives;
	[SerializeField]
	private float correctnessThreshold = 0.8f;
	[SerializeField]
	private float timerDuration = 120f;
	[SerializeField]
	private float timeReductionPerDifficulty = 30f;

	[SerializeField]
	private Texture2D introTexture;
	[SerializeField]
	private Texture2D introBackground;
	[SerializeField]
	private Texture2D gameOverTexture;
	[SerializeField]
	private Texture2D gameOverBackground;
	[SerializeField]
	private Texture2D finishTexture;
	[SerializeField]
	private Texture2D finishBackground;

	public bool IsScenarioActive => activeScenario != null;
	public int LivesLeft { get; private set; }

	private ScenarioSO activeScenario;
	private ScenarioSO lastScenario;
	private int currentLevelIndex = 0;

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
		LivesLeft = startLives;
		GameManager.Instance.ChangeGameState(GameStates.INTRO);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
			Restart();

		if (Input.GetKeyDown(KeyCode.Space))
			Object.FindObjectsOfType<PaintBrush2000>().ToList().ForEach(x => x.Respawn());

		if (Input.GetKey(KeyCode.Escape))
		{
			Debug.Log("Game quit!");
			Application.Quit();
		}
	}
	
	public void OnButtonPressed()
	{
		switch (GameManager.Instance.gameState)
		{
			case GameStates.INTRO:
			case GameStates.WAIT_FOR_NEXT:
				GameManager.Instance.ChangeGameState(GameStates.PLAYING);
				break;
			case GameStates.PLAYING:
				GameManager.Instance.ChangeGameState(GameStates.SHOW_SCORE);
				break;
			case GameStates.SHOW_SCORE:
				break;
			case GameStates.GAME_OVER:
			case GameStates.FINISH:
				Restart();
				break;
		}
	}

	public void OnCountdownEnded()
	{
		if (GameManager.Instance.gameState == GameStates.PLAYING)
			GameManager.Instance.ChangeGameState(GameStates.SHOW_SCORE);
	}

	public void Restart()
	{
		currentLevelIndex = 0;
		LivesLeft = startLives;
		activeScenario = null;
		scoreBar.ResetBar();
		starCollection.Restart();
		countdown.active = false;
		countdown.gameObject.SetActive(false);
		GameManager.Instance.ChangeGameState(GameStates.INTRO);
	}

	private void GameStageChanged(GameStates currentGameState, GameStates newGameState)
	{
		Debug.Log("Game state changed to: " + newGameState);
		switch (newGameState)
		{
			case GameStates.INTRO:
				countdown.gameObject.SetActive(false);
				paintCanvas.SetCanvasTexture(introBackground, introTexture);
				break;
			case GameStates.PLAYING:
				StartRandomScenario(difficultiesToBeat[currentLevelIndex]);
				break;
			case GameStates.SHOW_SCORE:
				countdown.active = false;
				lastScenario = activeScenario;
#if UNITY_EDITOR
				var s = System.Diagnostics.Stopwatch.StartNew();
				int startFrameCount = Time.frameCount;
#endif
				paintCanvas.CalculateCorrectnessFraction(activeScenario.colorLayer, 0.05f,
					progress => scoreBar.UpdateScore(progress),
					result =>
					{
#if UNITY_EDITOR
						s.Stop();
						Debug.Log($"Calculated completion perc: {result}, Elapsed time: {s.ElapsedMilliseconds}ms, Frames: {Time.frameCount - startFrameCount}");
#endif

						currentLevelIndex++;
						bool win = result >= correctnessThreshold;
						scoreBar.UpdateScore(result);
						if (!win)
						{
							scoreBar.DoFail();
							LivesLeft--;
							starCollection.UpdateStars();
							if (LivesLeft <= 0)
							{
								GameManager.Instance.ChangeGameState(GameStates.GAME_OVER);
								return;
							}
						}

						if (currentLevelIndex < difficultiesToBeat.Length)
							GameManager.Instance.ChangeGameState(GameStates.WAIT_FOR_NEXT);
						else
							GameManager.Instance.ChangeGameState(GameStates.FINISH);
					});
				break;
			case GameStates.WAIT_FOR_NEXT:
				countdown.gameObject.SetActive(false);
				activeScenario = null;
				break;
			case GameStates.GAME_OVER:
				paintCanvas.SetCanvasTexture(gameOverBackground, gameOverTexture);
				break;
			case GameStates.FINISH:
				paintCanvas.SetCanvasTexture(finishBackground, finishTexture);
				break;
		}
	}

	private void StartRandomScenario(int difficulty)
	{
		if (IsScenarioActive)
		{
			Debug.LogError("Scenario already active!");
			return;
		}

		Debug.Log($"Starting scenario at difficulty {difficulty} (level index {currentLevelIndex})");

		ScenarioSO[] scenariosAtDifficulty = scenarios.Where(x => x.difficulty == difficulty && x != lastScenario).ToArray();
		ScenarioSO randomScenario = scenariosAtDifficulty[Random.Range(0, scenariosAtDifficulty.Length - 1)];

		activeScenario = randomScenario;

		Texture2D paintingToDisplay = activeScenario.incorrectColorLayers[Random.Range(0, activeScenario.incorrectColorLayers.Length - 1)];
		paintCanvas.SetCanvasTexture(paintingToDisplay, activeScenario.lineArt);

		scoreBar.ResetBar();
		scoreBar.SetThreshold(correctnessThreshold);
		countdown.gameObject.SetActive(true);
		countdown.RestartTimer(timerDuration - (timeReductionPerDifficulty * (difficulty - 1)));
	}
}
