using UnityEngine;

/// <summary>
/// An scene-specific manager spawning units
/// </summary>
public class UnitManager : StaticInstance<UnitManager>
{
    [SerializeField]
    private Canvas worldSpaceCanvas;

    public GameObject SpawnHero(string mageName)
    {
        return SpawnUnit(mageName, GetRandomVector());
    }

    public GameObject SpawnHero(string mageName, Vector2 vector2)
    {
        return SpawnUnit(mageName, vector2);
    }

    public GameObject SpawnEnemy()
    {
        return SpawnUnit(ExampleEnemyType.SimpleEnemy, GetRandomVector());
    }

    GameObject SpawnUnit(string unitName, Vector3 pos)
    {
        var ScriptableHero = ResourceSystem.Instance.GetExampleHero(unitName);

        if (ScriptableHero != null)
        {
            var heroSpawned = Instantiate(ScriptableHero.Prefab, pos, Quaternion.identity, transform);

            Camera.main.gameObject.GetComponent<CameraManager>().target = heroSpawned.transform;

            var stats = ScriptableHero.BaseStats;

            stats.CurrentHp = stats.MaxHp;

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

            stats.CurrentHp = stats.MaxHp;

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

        while (GameManager.map[randomY, randomX] != 0 ||
            GameManager.map[randomY - 1, randomX] != 0 ||
            GameManager.map[randomY + 1, randomX] != 0 ||
            GameManager.map[randomY, randomX - 1] != 0 ||
            GameManager.map[randomY, randomX + 1] != 0)
        {
            randomX = Random.Range(1, width);
            randomY = Random.Range(1, height);
        }
        return new Vector3(randomX * 1.6f, randomY * 1.6f, 0);
    }
}