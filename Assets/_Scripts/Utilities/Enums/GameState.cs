using System;

[Serializable]
public enum GameState
{
    Hub = 0,
    ChangeLevel = 1,
    Starting = 2,
    Lose = 3,
    Null = 99
}