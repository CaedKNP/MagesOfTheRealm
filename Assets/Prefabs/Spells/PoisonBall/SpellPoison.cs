using Assets._Scripts.Spells;
using UnityEngine;

public class SpellPoison : SpellProjectileBase
{
    protected void Awake()
    {
        SetSpellStats();
        MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AttackHandler attack))
        {
            attack.DAMAGE(DMG, conditions);

            Destroy(gameObject);
        }
    }
}