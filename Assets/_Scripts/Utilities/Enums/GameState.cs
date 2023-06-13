using System;

[Serializable]
public enum GameState
{
    Hub = 0,
    ChangeLevel = 1,
    Starting = 2,
    Win = 3,
    Lose = 4,
    Null = 99
}