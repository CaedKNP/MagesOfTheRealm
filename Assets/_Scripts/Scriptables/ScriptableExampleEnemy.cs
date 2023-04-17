using System;
using UnityEngine;

/// <summary>
/// Create a scriptable enemy 
/// </summary>
[CreateAssetMenu(fileName = "ExampleEnemy")]
public class ScriptableExampleEnemy : ScriptableExampleUnitBase
{
    public ExampleEnemyType EnemyType;
}

[Serializable]
public enum ExampleEnemyType
{
    SimpleEnemy = 0
}