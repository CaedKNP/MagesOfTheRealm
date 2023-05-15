using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAcidBall : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(7f, 3f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Poison, AffectOnTick = 3f, AffectTime = 3f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(3, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}
