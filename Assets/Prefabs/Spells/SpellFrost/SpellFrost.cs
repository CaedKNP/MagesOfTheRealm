using System.Collections.Generic;
using UnityEngine;

public class SpellFrost : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(8f, 4f); // Nowe wartości dla speed i destroyTime
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
            Conditions.Freeze
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(conditions, 2, 3, 50);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}