using UnityEngine;
using System.Linq;

/// <summary>
/// An scene-specific manager spawning units
/// </summary>
public class UnitManager : StaticInstance<UnitManager>
{
    public GameObject SpawnHero()
    {
        return SpawnUnit(ExampleHeroType.SimpleMage, GetRandomVector());
    }

    public GameObject SpawnEnemy()
    {
        return SpawnUnit(ExampleEnemyType.SimpleEnemy, new Vector3(0,0,0));
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

    Vector3 GetRandomVector()
    {
        int width = GameManager.map.GetLength(1) - 1;
        int height = GameManager.map.GetLength(0) - 1;

        int randomX = Random.Range(0, width);
        int randomY = Random.Range(0, height);

        while (GameManager.map[randomY, randomX] != 0)
        {
            randomX = Random.Range(0, width);
            randomY = Random.Range(0, height);

            Debug.Log($"X: {randomX} - Y: {randomY} || X: {randomX * 0.16f - 2f} - Y: {randomY * 0.16f - 2f}");
        }
        return new Vector3(randomX * 0.16f - 2f, randomY * 0.16f - 2f, 0);
    }
}