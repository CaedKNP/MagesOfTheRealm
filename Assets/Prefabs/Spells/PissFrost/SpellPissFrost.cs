using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPissFrost : SpellBase
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
            new ConditionBase() { Conditions = Conditions.Slow, AffectOnTick = 0.5f, AffectTime = 3 }
        };

        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attack))
        {
            attack.DAMAGE(4, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }

        //if (collision.gameObject.layer == 11)
        //{
        //    var asd = collision.gameObject.GetComponent<AttackHandler>();

        //    asd.DAMAGE(2, conditions);

        //    if (!BeforeDelete())
        //        Destroy(gameObject);
        //}
    }
}
