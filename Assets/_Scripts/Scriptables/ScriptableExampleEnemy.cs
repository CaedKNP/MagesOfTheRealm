using UnityEngine;

/// <summary>
/// Create a scriptable enemy 
/// </summary>
[CreateAssetMenu(fileName = "ExampleEnemy")]
public class ScriptableExampleEnemy : ScriptableExampleUnitBase
{
    public ExampleEnemyType EnemyType;
}