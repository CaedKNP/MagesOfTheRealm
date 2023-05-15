using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellBlueFireball : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(7f, 3f); // Nowe warto�ci dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Burn, AffectOnTick = 2f, AffectTime = 3f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(6, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}