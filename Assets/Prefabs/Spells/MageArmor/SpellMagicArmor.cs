using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMagicArmor : MonoBehaviour
{
    GameObject player;
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
            new ConditionBase() { Conditions = Conditions.ArmorUp, AffectOnTick = 50f, AffectTime = 2f },
            new ConditionBase() { Conditions = Conditions.SpeedUp, AffectOnTick = 2f, AffectTime = 2f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(0, conditions);
        }
    }

    void TimeOut()
    {
        Destroy(spellCore);
    }
}
