using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellBlueFireball : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(7f, 3f); // Nowe wartoœci dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Burn, AffectOnTick = 5f, AffectTime = 1f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(7, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }

        //if (collision.gameObject.layer == 11)
        //{
        //    var asd = collision.gameObject.GetComponent<AttackHandler>();

        //    asd.DAMAGE(3, conditions);

        //    if (!BeforeDelete())
        //        Destroy(gameObject);
        //}
    }
}