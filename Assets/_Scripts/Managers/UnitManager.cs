using UnityEngine;

/// <summary>
/// An scene-specific manager spawning units
/// </summary>
public class UnitManager : StaticInstance<UnitManager>
{
    public void SpawnHero()
    {
        SpawnUnit(ExampleHeroType.SimpleMage, new Vector3(0, 0, 0));
    }

    void SpawnUnit(ExampleHeroType t, Vector3 pos)
    {
        var simpleMageScriptable = ResourceSystem.Instance.GetExampleHero(t);

        if (simpleMageScriptable != null)
        {

            var spawned = Instantiate(simpleMageScriptable.Prefab, pos, Quaternion.identity, transform);
            Camera.main.gameObject.GetComponent<CameraManager>().target = spawned.transform;

            var stats = simpleMageScriptable.BaseStats;
            // Apply possible modifications here (artifacts, clothets...)
            //stats.MaxHp += 3;

            spawned.SetStats(stats);
        }
    }
}