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

        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attack))
        {
            attack.DAMAGE(Dmg, Conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}