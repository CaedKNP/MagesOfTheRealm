using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellFrost : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(8f, 4f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }

        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Freeze, AffectOnTick = 0, AffectTime = 2 }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(2, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}