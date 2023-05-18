using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellPoison : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(8f, 4f); // Nowe wartoœci dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<AttackHandler>(out AttackHandler attack))
        {
            attack.DAMAGE(Dmg, Conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}