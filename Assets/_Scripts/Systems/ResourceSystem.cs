using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem>
{
    public List<ScriptableExampleHero> ExampleHeroes { get; private set; }
    private Dictionary<string, ScriptableExampleHero> _ExampleHeroesDict;

    public List<ScriptableExampleEnemy> ExampleEnemies { get; private set; }
    private Dictionary<ExampleEnemyType, ScriptableExampleEnemy> _ExampleEnemiesDict;

    public List<Spell> AllSpells { get; private set; }
    private Dictionary<string, Spell> _AllSpellsDict;

    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources()
    {
        ExampleHeroes = Resources.LoadAll<ScriptableExampleHero>("ExampleHeroes").ToList();
        _ExampleHeroesDict = ExampleHeroes.ToDictionary(h => h.HeroName, h => h);

        ExampleEnemies = Resources.LoadAll<ScriptableExampleEnemy>("ExampleEnemies").ToList();
        _ExampleEnemiesDict = ExampleEnemies.ToDictionary(e => e.EnemyType, e => e);

        AllSpells = Resources.LoadAll<Spell>("Spells").ToList();
        _AllSpellsDict = AllSpells.ToDictionary(s => s.Name, s => s);
    }

    public ScriptableExampleHero GetExampleHero(string heroName) => _ExampleHeroesDict[heroName];

    public ScriptableExampleEnemy GetExampleEnemy(ExampleEnemyType enemyType) => _ExampleEnemiesDict[enemyType];

    public Spell GetExampleSpell(string spellName) => _AllSpellsDict[spellName];

    public ScriptableExampleHero GetRandomHero() => ExampleHeroes[Random.Range(0, ExampleHeroes.Count)];

    public ScriptableExampleEnemy GetRandomEnemy() => ExampleEnemies[Random.Range(0, ExampleEnemies.Count)];

    public Spell GetRandomSpell(SpellSlot spellSlot) => AllSpells.Where(s => s.spellSlot == spellSlot).ElementAt(Random.Range(0, AllSpells.Count));
}