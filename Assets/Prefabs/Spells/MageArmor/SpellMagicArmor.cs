using UnityEngine;

public class SpellMagicArmor : SpellBase
{
    GameObject player;
    private GameObject spellCore; // Reference to the spellCore object
    private void Awake()
    {
        SetSpellStats();
        Invoke("TimeOut", destroyTime);
        // Find the spellCore object in the swordCore prefab
        spellCore = transform.parent?.gameObject;
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
        Destroy(spellCore);
    }
}