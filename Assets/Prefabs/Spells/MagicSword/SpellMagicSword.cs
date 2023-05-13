using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class SpellMagicSword : MonoBehaviour
{
    private GameObject spellCore; // Referencja do obiektu spellCore

    protected void Awake()
    {
        //SetSpeedDestroyTime(0f, 2f); // Nowe wartości dla speed i destroyTime
        //base.MyAwake();

        // Znajdź obiekt spellCore w prefabie swordCore
        spellCore = transform.parent?.gameObject;
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

                Destroy(gameObject);
            
        }
    }
}
