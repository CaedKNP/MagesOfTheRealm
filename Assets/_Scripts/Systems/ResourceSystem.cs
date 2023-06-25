using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Repository for all scriptable objects
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem>
{
    /// <summary>
    /// List of all heroes
    /// </summary>
    public List<ScriptableExampleHero> ExampleHeroes { get; private set; }
    private Dictionary<string, ScriptableExampleHero> _ExampleHeroesDict;

    /// <summary>
    /// List of all enemies
    /// </summary>
    public List<ScriptableExampleEnemy> ExampleEnemies { get; private set; }
    private Dictionary<ExampleEnemyType, ScriptableExampleEnemy> _ExampleEnemiesDict;

    /// <summary>
    /// List of all spells
    /// </summary>
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

    /// <summary>
    /// Returns some hero
    /// </summary>
    /// <param name="heroName">name of hero</param>
    /// <returns>reference to hero</returns>
    public ScriptableExampleHero GetExampleHero(string heroName) => _ExampleHeroesDict[heroName];

    /// <summary>
    /// Returns enemy of some type
    /// </summary>
    /// <param name="enemyType">type of wanted enemy</param>
    /// <returns>reference to enemy</returns>
    public ScriptableExampleEnemy GetExampleEnemy(ExampleEnemyType enemyType) => _ExampleEnemiesDict[enemyType];

    /// <summary>
    /// Returns some spell
    /// </summary>
    /// <param name="spellName">mane of spell to get</param>
    /// <returns>reference to spell</returns>
    public Spell GetExampleSpell(string spellName) => _AllSpellsDict[spellName];

    /// <summary>
    /// Returns random hero
    /// </summary>
    /// <returns>reference to hero</returns>
    public ScriptableExampleHero GetRandomHero() => ExampleHeroes[Random.Range(0, ExampleHeroes.Count)];

    /// <summary>
    /// Returns random enemy
    /// </summary>
    /// <returns>reference to emeny</returns>
    public ScriptableExampleEnemy GetRandomEnemy() => ExampleEnemies[Random.Range(0, ExampleEnemies.Count)];

    /// <summary>
    /// Returns random spell for some slot
    /// </summary>
    /// <param name="spellSlot">slot of the spell</param>
    /// <returns>reference to spell</returns>
    public Spell GetRandomSpell(SpellSlot spellSlot) => AllSpells.Where(s => s.spellSlot == spellSlot).ElementAt(Random.Range(0, AllSpells.Count));
}