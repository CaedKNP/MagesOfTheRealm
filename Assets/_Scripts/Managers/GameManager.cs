using Assets._Scripts.Managers;
using Assets.Resources.Entities;
using Assets.Resources.SOs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : StaticInstance<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public static GameObject Player;
    public static int[,] map;
    public static Vector2[,] mapPositions;
    public static List<GameObject> enemies;

    //private HighScore highScore;
    //private List<HighScore> highScores;

    [SerializeField]
    private intSO scoreSO;

    [SerializeField]
    private stringSO mageNameSO;

    public GameState State { get; private set; }

    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "LevelTest":
                ChangeState(GameState.Starting);
                break;
            case "LevelHub":
                ChangeState(GameState.Hub);
                break;
            default:
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Hub:
                HandleHub();
                break;
            case GameState.ChangeLevel:
                HandleLevelChange();
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
        if (SceneManager.GetActiveScene().name != "LevelHub")
        {
            SceneManager.LoadScene("LevelHub");
            //highScore = new();
        }

        Player = UnitManager.Instance.SpawnHero(mageNameSO.String, new Vector2(27, 42));
    }

    void HandleLevelChange()
    {
        SceneManager.LoadScene("LevelTest");
        ChangeState(GameState.Starting);
    }

    void HandleStarting()
    {
        enemies = new();//not sure where to put it
        map = FindObjectOfType<LevelGenerator>().GenerateMap();
        ChangeState(GameState.SpawningHero);
    }

    void HandleSpawningHero()
    {
        Player = UnitManager.Instance.SpawnHero(mageNameSO.String);

        ChangeState(GameState.SpawningEnemies);
    }

    void HandleSpawningEnemies()
    {
        ChangeState(GameState.Playing);
    }

    void HandlePlaying()
    {

    }

    void HandleLose()
    {
        WaveManager.Instance.gameOver = true;
        WaveManager.Instance.waveName.text = "YOU DIED!";

        //highScore.score = scoreSO.Int;
        //highScores.Add(highScore);
        scoreSO.Int = 0;

        var asd = StartCoroutine(WaitSomeSecs());
    }

    IEnumerator WaitSomeSecs()
    {
        var end = Time.time + 3;

        while (Time.time < end)
        {
            yield return null;
        }

        ChangeState(GameState.Hub);
    }

    void HandleWin()
    {

    }
}