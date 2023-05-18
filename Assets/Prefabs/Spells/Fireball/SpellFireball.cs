using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellFireball : SpellBase
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
            new ConditionBase() { Conditions = Conditions.Burn, AffectOnTick = 3f, AffectTime = 3f }
        };

        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attack))
        {
            attack.DAMAGE(3, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}