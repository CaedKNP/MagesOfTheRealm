using Assets._Scripts.Managers;
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

    public List<GameObject> gameObjects;

    //private HighScore highScore;
    //private List<HighScore> highScores;

    [SerializeField]
    private intSO scoreSO;

    [SerializeField]
    private stringSO mageNameSO;

    public GameState State { get; private set; }

    private Scene _currentScene;

    void DontDestroyEach()
    {
        if (gameObjects is not null)
            foreach (var gameObject in gameObjects)
            {
                DontDestroyOnLoad(this.gameObject);
            }
    }

    void Start()
    {
        //DontDestroyEach();

        //switch (SceneManager.GetActiveScene().name)
        //{
        //    case "LevelTest":
        //        ChangeState(GameState.Starting);
        //        break;
        //    case "LevelHub":
        //        ChangeState(GameState.Hub);
        //        break;
        //    default:
        //        break;
        //}

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
        //if (SceneManager.GetActiveScene().name != "LevelHub")
        //{
        //    SceneManager.LoadScene("LevelHub");
        //    //highScore = new();
        //}

        var _ = StartCoroutine(LoadAsync("LevelHub", true));
    }

    IEnumerator LoadAsync(string SceneName, bool spawnPlayer)
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        _currentScene = SceneManager.GetSceneAt(1);

        while (!_currentScene.isLoaded)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(_currentScene);

        if (spawnPlayer)
            Player = UnitManager.Instance.SpawnHero(mageNameSO.String, new Vector2(27, 42));
        else
            ChangeState(GameState.Starting);
    }

    void HandleLevelChange()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        SceneManager.UnloadScene("LevelHub");

        var _ = StartCoroutine(LoadAsync("LevelTest", false));
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