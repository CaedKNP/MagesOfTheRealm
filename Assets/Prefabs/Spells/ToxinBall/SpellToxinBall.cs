using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellToxinBall : SpellBase
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
            new ConditionBase() { Conditions = Conditions.Poison, AffectOnTick = 3, AffectTime = 6 }
        };

        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attack))
        {
            attack.DAMAGE(2, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }


    }
}

