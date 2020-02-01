using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;

    private GameManager() { }

    public GameStates previousGameState;
    public GameStates GameState;

    public static GameManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new GameManager(); 
            }
            return instance;
        }
    }


    public delegate void GameStateChanged(GameStates currentGameState, GameStates newGameState);
    public static event GameStateChanged OnGameStateChanged;

    public GameStates gameState;

    void Start()
    {
        gameState = GameStates.INTRO;
    }

    public void ChangeGameState(GameStates newGameState)
    {
        previousGameState = GameState;
        GameStates currentGameState = GameState;
        GameState = newGameState;
        OnGameStateChanged(currentGameState, GameState);
    }
}

public enum GameStates
{ 
    INTRO,
    PLAYING,
    FINISH
}

