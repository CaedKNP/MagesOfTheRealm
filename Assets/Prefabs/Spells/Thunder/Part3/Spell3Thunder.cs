using UnityEngine;

public class Spell3Thunder : SpellBase
{
    [SerializeField]
    float timeOut = 0.3f;
    private void Awake()
    {
        SetSpellStats();
        Invoke("TimeOut", timeOut);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AttackHandler unit))
        {
            unit.DAMAGE(DMG, conditions);
        }
    }
    void TimeOut()
    {
        Destroy(gameObject);
    }
}