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
        if (collision.gameObject.TryGetComponent(out AttackHandler attack))
        {
            attack.DAMAGE(Dmg, Conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }
}