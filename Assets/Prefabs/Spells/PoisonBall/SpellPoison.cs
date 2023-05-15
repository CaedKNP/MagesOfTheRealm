using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellPoison : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(8f, 4f); // Nowe wartoœci dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Poison, AffectOnTick = 2, AffectTime = 2 }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(2, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}