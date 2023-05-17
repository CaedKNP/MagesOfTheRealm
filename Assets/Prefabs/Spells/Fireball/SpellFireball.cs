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
        if (collision == caster)
            return;

        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Burn, AffectOnTick = 3f, AffectTime = 3f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(3, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }

        if (collision.gameObject.layer == 11)
        {
            var asd = collision.gameObject.GetComponent<AttackHandler>();

            asd.DAMAGE(3, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}