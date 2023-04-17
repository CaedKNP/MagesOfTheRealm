using UnityEngine;

/// <summary>
/// An scene-specific manager spawning units
/// </summary>
public class UnitManager : StaticInstance<UnitManager>
{
    public GameObject SpawnHero()
    {
        return SpawnUnit(ExampleHeroType.SimpleMage, new Vector3(0, 0, 0));
    }

    public GameObject SpawnEnemy()
    {
        return SpawnUnit(ExampleEnemyType.SimpleEnemy, new Vector3(-1.38f, 0.371f, 0));
    }

    GameObject SpawnUnit(ExampleHeroType t, Vector3 pos)
    {
        var ScriptableHero = ResourceSystem.Instance.GetExampleHero(t);

        if (ScriptableHero != null)
        {
            var heroSpawned = Instantiate(ScriptableHero.Prefab, pos, Quaternion.identity, transform);

            Camera.main.gameObject.GetComponent<CameraManager>().target = heroSpawned.transform;

            var stats = ScriptableHero.BaseStats;

            // Apply possible modifications here (artifacts, clothets...): stats.MaxHp += 3;

            heroSpawned.SetStats(stats);

            return heroSpawned.gameObject;
        }

        return null;
    }

    GameObject SpawnUnit(ExampleEnemyType t, Vector3 pos)
    {
        var ScriptableEnemy = ResourceSystem.Instance.GetExampleEnemy(t);

        if (ScriptableEnemy != null)
        {
            var enemySpawned = Instantiate(ScriptableEnemy.Prefab, pos, Quaternion.identity, transform);

            var stats = ScriptableEnemy.BaseStats;

            // Apply possible modifications here (artifacts, clothets...): stats.MaxHp -= 3;

            enemySpawned.SetStats(stats);

            return enemySpawned.gameObject;
        }

        return null;
    }
}