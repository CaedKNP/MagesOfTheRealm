using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell3Thunder : MonoBehaviour
{
    public float timeOut = 0.3f;
    private void Awake()
    {
        Invoke("TimeOut", timeOut);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.ArmorDown, AffectOnTick = 2f, AffectTime = 2f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(15, conditions);
        }
    }
    void TimeOut()
    {
        Destroy(gameObject);
    }
}
