using Assets._Scripts.Managers;
using Assets.Resources.Entities;
using Assets.Resources.SOs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the game behavior
/// </summary>
public class GameManager : StaticInstance<GameManager>
{
    /// <summary>
    /// Invoked before state would change to new one
    /// </summary>
    public static event Action<GameState> OnBeforeStateChanged;

    /// <summary>
    /// Invoked after state would change to new one
    /// </summary>
    public static event Action<GameState> OnAfterStateChanged;

    /// <summary>
    /// Current game state
    /// </summary>
    public GameState State { get; private set; }

    /// <summary>
    /// Reference to player
    /// </summary>
    public static GameObject Player;

    /// <summary>
    /// game map in numeric representation
    /// </summary>
    public static int[,] map;

    /// <summary>
    /// ?????
    /// </summary>
    public static Vector2[,] mapPositions;

    /// <summary>
    /// List of all enemies alive
    /// </summary>
    public static List<GameObject> enemies;

    /// <summary>
    /// Contains info about wave name
    /// </summary>
    public Text waveName;

    /// <summary>
    /// Contains info about amount of enemies alive
    /// </summary>
    public Text enemyCounter;

    /// <summary>
    /// ?????
    /// </summary>
    public List<GameObject> gameObjects;

    /// <summary>
    /// flag that shows if the Scores were saved
    /// </summary>
    public bool ScoresWasSaved = false;

    private List<HighScore> highScores;
    private HighScore highScore;

    [SerializeField]
    private intSO scoreSO;

    [SerializeField]
    private stringSO mageNameSO;

    private Scene _currentScene;

    void Start()
    {
        enemies = new();

        var _ = StartCoroutine(LoadScoresAsync());

        ChangeState(GameState.Hub);
    }

    IEnumerator LoadScoresAsync()
    {
        while (XMLManager.Instance == null)
        {
            yield return null;
        }

        highScores = XMLManager.Instance.LoadScores();
    }

    /// <summary>
    /// Method that allows to manage current game state
    /// </summary>
    /// <param name="newState">new game state</param>
    /// <exception cref="ArgumentOutOfRangeException">if the new state is not handled</exception>
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

        highScore = new() { score = 0 };
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

        highScore.score = scoreSO.Int;
        highScores.Add(highScore);
        scoreSO.Int = 0;

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

    /// <summary>
    /// Returns value of hight scores or new list if null
    /// </summary>
    /// <returns></returns>
    public List<HighScore> GetHightScores()
    {
        return highScores ??= new List<HighScore>();
    }

    protected override void OnApplicationQuit()
    {
        var _ = StartCoroutine(WaitToSave());
    }

    IEnumerator WaitToSave()
    {
        while (!ScoresWasSaved)
        {
            yield return null;
        }

        base.OnApplicationQuit();
    }
}