using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem>
{
    public List<ScriptableExampleHero> ExampleHeroes { get; private set; }
    private Dictionary<ExampleHeroType, ScriptableExampleHero> _ExampleHeroesDict;

    public List<ScriptableExampleEnemy> ExampleEnemies { get; private set; }
    private Dictionary<ExampleEnemyType, ScriptableExampleEnemy> _ExampleEnemiesDict;

    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources()
    {
        ExampleHeroes = Resources.LoadAll<ScriptableExampleHero>("ExampleHeroes").ToList();
        _ExampleHeroesDict = ExampleHeroes.ToDictionary(r => r.HeroType, r => r);

        ExampleEnemies = Resources.LoadAll<ScriptableExampleEnemy>("ExampleEnemies").ToList();
        _ExampleEnemiesDict = ExampleEnemies.ToDictionary(r => r.EnemyType, r => r);
    }

    public ScriptableExampleHero GetExampleHero(ExampleHeroType heroType) => _ExampleHeroesDict[heroType];

    public ScriptableExampleEnemy GetExampleEnemy(ExampleEnemyType enemyType) => _ExampleEnemiesDict[enemyType];

    public ScriptableExampleHero GetRandomHero() => ExampleHeroes[Random.Range(0, ExampleHeroes.Count)];

    public ScriptableExampleEnemy GetRandomEnemy() => ExampleEnemies[Random.Range(0, ExampleEnemies.Count)];
}