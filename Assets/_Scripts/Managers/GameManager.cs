using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //If u wanna play on hub change GameState to Hub
    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Hub:
                HandleHub();
                break;
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
                HandleWin();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    void HandleHub()
    {
        Player = UnitManager.Instance.SpawnHero("BlueMage", new Vector2(27, 42));
        //ChangeState(GameState.SpawningHero);
    }

    void HandleStarting()
    {
        //SceneManager.LoadScene("LevelTest");
        map = FindObjectOfType<LevelGenerator>().GenerateMap();
        ChangeState(GameState.SpawningHero);
    }

    void HandleSpawningHero()
    {
        Player = UnitManager.Instance.SpawnHero("OrangeMage");

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
        
    }

    void HandleLose()
    {

    }

    void HandleWin()
    {

    }
}