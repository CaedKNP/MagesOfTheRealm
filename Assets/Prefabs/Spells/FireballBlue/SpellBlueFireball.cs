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
        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(Dmg, Conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}