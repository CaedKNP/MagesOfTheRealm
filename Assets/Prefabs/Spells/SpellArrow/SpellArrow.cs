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
        var conditions = new List<Conditions>
        {
            Conditions.Slow
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(conditions, 3, 5, 50);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}