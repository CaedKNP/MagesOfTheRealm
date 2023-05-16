using System;

[Serializable]
public enum GameState
{
    Hub = 0,
    ChangeLevel = 1,
    Starting = 2,
    SpawningHero = 3,
    SpawningEnemies = 4,
    Playing = 5,
    Win = 6,
    Lose = 7
}