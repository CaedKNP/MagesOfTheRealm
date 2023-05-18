using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellArrow : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(5f, 5f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase { Conditions = Conditions.Slow, AffectTime = 0.3f, AffectOnTick = 0.7f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(6, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attack))
        {
            attack.DAMAGE(3, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}