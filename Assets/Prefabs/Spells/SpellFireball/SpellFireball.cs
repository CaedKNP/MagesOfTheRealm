using System.Collections.Generic;
using UnityEngine;

public class SpellFireball : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(8f, 3f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }

        var conditions = new List<Conditions>
        {
            Conditions.Burn
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(conditions, 1, 5, 2);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}