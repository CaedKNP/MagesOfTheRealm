using Assets._Scripts.Managers;
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

    //Bool if hub wasn`t already initialized
    bool _isFirstHub = true;

    //private HighScore highScore;
    //private List<HighScore> highScores;

    [SerializeField]
    private intSO scoreSO;

    [SerializeField]
    private stringSO mageNameSO;

    public GameState State { get; private set; }

    private Scene _currentScene;

    void Start()
    {
        enemies = new();

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
        var _ = StartCoroutine(LoadAsync("LevelHub", GameState.Null));

        if (_isFirstHub)
        {
            Player = UnitManager.Instance.SpawnHero(mageNameSO.String, new Vector2(27, 42));
            _isFirstHub = false;
        }
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
        WaveManager.Instance.gameOver = true;
        WaveManager.Instance.waveName.text = "YOU DIED!";

        //highScore.score = scoreSO.Int;
        //highScores.Add(highScore);
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

        HandleLevelChangeToHub();
        ChangeState(GameState.Hub);
    }

    void HandleLevelChangeToHub()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));

        //foreach (var item in UnitManager.Instance.transform)
        //{
            //delete all childrens
        //}
        
        _isFirstHub = true;
        SceneManager.UnloadScene("LevelTest");
    }

    void HandleWin()
    {

    }
}