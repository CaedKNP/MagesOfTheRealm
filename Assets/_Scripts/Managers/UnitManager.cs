using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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

    public GameObject SpawnEnemy(ExampleEnemyType e)
    {
        return SpawnUnit(e, GetRandomSpawner());
    }

    GameObject SpawnUnit(string unitName, Vector3 pos)
    {
        var ScriptableHero = ResourceSystem.Instance.GetExampleHero(unitName);

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

    Vector3 GetRandomSpawner()
    {
        List<Vector2Int> spawners = new List<Vector2Int>();
        for (int i = 0; i < GameManager.map.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.map.GetLength(1); j++)
            {
                if (GameManager.map[i, j] == 2)
                    spawners.Add(new(i, j));
            }
        }

        Vector2Int tempPoint = spawners.ElementAt(Random.Range(0, spawners.Count)); // get random spwaner

        //Choose direction 
        switch (Random.Range(0, 8))
        {
            case 0:
                tempPoint = new Vector2Int(tempPoint.x + 1, tempPoint.y);
                break;
            case 1:
                tempPoint = new Vector2Int(tempPoint.x - 1, tempPoint.y);
                break;
            case 2:
                tempPoint = new Vector2Int(tempPoint.x + 1, tempPoint.y + 1);
                break;
            case 3:
                tempPoint = new Vector2Int(tempPoint.x - 1, tempPoint.y + 1);
                break;
            case 4:
                tempPoint = new Vector2Int(tempPoint.x - 1, tempPoint.y - 1);
                break;
            case 5:
                tempPoint = new Vector2Int(tempPoint.x + 1, tempPoint.y - 1);
                break;
            case 6:
                tempPoint = new Vector2Int(tempPoint.x, tempPoint.y - 1);
                break;
            case 7:
                tempPoint = new Vector2Int(tempPoint.x, tempPoint.y + 1);
                break;
        }
        return TileToPosition(tempPoint);
    }
    Vector2 TileToPosition(Vector2Int target)
    {
        return new(target.x * 1.6f, target.y * 1.6f);
    }
}