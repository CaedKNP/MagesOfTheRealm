using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellPoison : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(8f, 4f); // Nowe warto�ci dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Poison, AffectOnTick = 2, AffectTime = 2 }
        };

        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attack))
        {
            attack.DAMAGE(3, conditions);

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