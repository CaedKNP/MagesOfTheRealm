using System;
using UnityEngine;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class GameManager : StaticInstance<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public static GameObject Player;

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
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    private void HandleStarting()
    {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state

        ChangeState(GameState.SpawningHero);
    }

    private void HandleSpawningHero()
    {
        Player = UnitManager.Instance.SpawnHero();

        ChangeState(GameState.SpawningEnemies);
    }

    private void HandleSpawningEnemies()
    {
        UnitManager.Instance.SpawnEnemy();

        ChangeState(GameState.Playing);
    }

    private void HandlePlaying()
    {
        // Playing logic
    }
}

/// <summary>
/// This is obviously an example and I have no idea what kind of game you're making.
/// You can use a similar manager for controlling your menu states or dynamic-cinematics, etc
/// </summary>
[Serializable]
public enum GameState
{
    Starting = 0,
    SpawningHero = 1,
    SpawningEnemies = 2,
    Playing = 3,
    Win = 4,
    Lose = 5,
}