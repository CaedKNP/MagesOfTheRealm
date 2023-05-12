using System;
using UnityEngine;

/// <summary>
/// enum-based game manager
/// </summary>
public class GameManager : StaticInstance<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public static GameObject Player;
    public static int[,] map;
    public static Vector2[,] mapPositions;

    public GameState State { get; private set; }

    // Kick the game off with the first state
    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawningHero:
                HandleSpawningHero();
                break;
            case GameState.SpawningEnemies:
                HandleSpawningEnemies();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Win:
                //Win logic
                break;
            case GameState.Lose:
                //Lose logic
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    void HandleStarting()
    {
        //May do some start setup, could be environment, cinematics etc
        map = FindObjectOfType<LevelGenerator>().GenerateMap();
        ChangeState(GameState.SpawningHero);
    }

    void HandleSpawningHero()
    {
        Player = UnitManager.Instance.SpawnHero();

        ChangeState(GameState.SpawningEnemies);
    }

    void HandleSpawningEnemies()
    {
        for(int i = 0; i <= 0; i++)
            UnitManager.Instance.SpawnEnemy();

        ChangeState(GameState.Playing);
    }

    void HandlePlaying()
    {
        // Playing logic
    }
}