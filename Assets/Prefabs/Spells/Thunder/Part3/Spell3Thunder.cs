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
            new ConditionBase() { Conditions = Conditions.Slow, AffectOnTick = 0.8f, AffectTime = 0.5f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(25, conditions);
        }
    }
    void TimeOut()
    {
        Destroy(gameObject);
    }
}
