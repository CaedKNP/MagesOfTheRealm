using System;

[Serializable]
public enum GameState
{
    Hub = 0,
    Starting = 1,
    SpawningHero = 2,
    SpawningEnemies = 3,
    Playing = 4,
    Win = 5,
    Lose = 6
}