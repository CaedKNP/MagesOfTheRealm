using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMagicArmor : MonoBehaviour
{
    private GameObject spellCore; // Reference to the spellCore object
    private void Awake()
    {
        Invoke("TimeOut", 2f);

        // Find the spellCore object in the swordCore prefab
        spellCore = transform.parent?.gameObject;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { /*Conditions = Conditions.Haste, AffectOnTick = 1f, AffectTime = 1f*/ }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(1, conditions);
        }
    }

    void TimeOut()
    {
        Destroy(spellCore);
    }
}
