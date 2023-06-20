using Assets._Scripts.Managers;
using Assets.Resources.Entities;
using Assets.Resources.SOs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : StaticInstance<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public static GameObject Player;
    public static int[,] map;
    public static Vector2[,] mapPositions;
    public static List<GameObject> enemies;

    public Text waveName;
    public Text enemyCounter;

    public List<GameObject> gameObjects;

    private List<HighScore> highScores;
    private HighScore highScore;

    [SerializeField]
    private intSO scoreSO;

    [SerializeField]
    private stringSO mageNameSO;

    public GameState State { get; private set; }

    private Scene _currentScene;

    void Start()
    {
        enemies = new();

        //highScores = XMLManager.Instance.LoadScores();

        ChangeState(GameState.Hub);
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
        var _ = StartCoroutine(LoadAsync("LevelHub", GameState.Null));

        Player = UnitManager.Instance.SpawnHero(mageNameSO.String, new Vector2(27, 42));

        //waveName.text = "Press L To Start";

        highScore = new()
        {
            score = 0
        };
    }

    IEnumerator LoadAsync(string SceneName, GameState state)
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        _currentScene = SceneManager.GetSceneAt(1);

        while (!_currentScene.isLoaded)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(_currentScene);

        if (state != GameState.Null)
            ChangeState(state);
    }

    void HandleLevelChange()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));

        SceneManager.UnloadScene("LevelHub");

        var _ = StartCoroutine(LoadAsync("LevelTest", GameState.Starting));
    }

    void HandleStarting()
    {
        FindObjectOfType<LevelGenerator>().GenerateMap();
    }

    void HandleLose()
    {
        waveName.text = "YOU DIED!";
        WaveManager.Instance.StopAllCoroutines();

        //highScore.score = scoreSO.Int;
        //highScores.Add(highScore);
        //scoreSO.Int = 0;

        var _ = StartCoroutine(WaitSomeSecs());
    }

    IEnumerator WaitSomeSecs()
    {
        var end = Time.time + 3;

        while (Time.time < end)
        {
            yield return null;
        }

        waveName.text = "Press L To Start";
        Destroy(WaveManager.Instance.gameObject);
        LevelChangeToHub();
        ChangeState(GameState.Hub);
    }

    void LevelChangeToHub()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));

        enemies.Clear();

        foreach (Transform children in UnitManager.Instance.transform)
        {
            Destroy(children.gameObject);
        }

        SceneManager.UnloadScene("LevelTest");
    }

    private void OnApplicationQuit()
    {
        if (highScores != null)
            XMLManager.Instance.SaveScores(highScores);
    }
}