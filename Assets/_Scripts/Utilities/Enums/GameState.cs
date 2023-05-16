using System;

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