using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
	private static GameManager instance;

	private GameManager() { }

	public GameStates previousGameState;
	public GameStates gameState;

	public static GameManager Instance {
		get {
			if (instance == null)
			{
				instance = new GameManager();
			}
			return instance;
		}
	}

	public delegate void GameStateChanged(GameStates currentGameState, GameStates newGameState);
	public static event GameStateChanged OnGameStateChanged;

	void Start()
	{
		gameState = GameStates.INTRO;
	}

	public void ChangeGameState(GameStates newGameState)
	{
		previousGameState = gameState;
		GameStates currentGameState = gameState;
		gameState = newGameState;
		OnGameStateChanged(currentGameState, gameState);
	}
}

public enum GameStates
{
	INTRO,
	PLAYING,
	SHOW_SCORE,
	WAIT_FOR_NEXT,
	GAME_OVER,
	FINISH
}

