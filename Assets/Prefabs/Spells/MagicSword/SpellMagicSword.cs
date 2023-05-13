using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellMagicSword : MonoBehaviour
{
    private GameObject spellCore; // Referencja do obiektu spellCore

    protected void Awake()
    {
        // Znajduje obiekt spellCore w prefabie swordCore
        spellCore = transform.parent?.gameObject;
        spellCore.transform.Rotate(Vector3.forward, -65f);
        Invoke("TimeOut", 5f);
    }

    private void FixedUpdate()
    {
        spellCore.transform.Rotate(Vector3.forward, 7f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Burn, AffectOnTick = 1f, AffectTime = 1f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(3, conditions);
            Destroy(spellCore);
        }
    }
    void TimeOut()
    {
        Destroy(spellCore);
    }
}
