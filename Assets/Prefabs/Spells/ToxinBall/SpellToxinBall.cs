using Assets._Scripts.Spells;
using UnityEngine;

public class SpellToxinBall : SpellProjectileBase
{
    protected void Awake()
    {
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