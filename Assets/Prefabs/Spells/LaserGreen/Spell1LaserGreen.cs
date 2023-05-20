using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1LaserGreen : SpellBase
{
    List<UnitBase> unitsInCollision = new List<UnitBase>();
    public GameObject laserPoint; // MonoBehaviour
    StaffRotation staff;

    protected void Awake()
    {
        SetSpeedDestroyTime(40f, 1f);
        base.MyAwake();
        Invoke("TimeOut", 0.5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Burn, AffectOnTick = 1f, AffectTime = 5f }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(1, conditions);
            unitsInCollision.Add(unit);
        }
    }

    void TimeOut()
    {
        GameObject laser = Instantiate(laserPoint, staff.WizandStaffFirePint.transform.position, transform.rotation);

        Spell2LaserGreen spell2Laser = laser.GetComponent<Spell2LaserGreen>();
        spell2Laser.SetUnitsInCollision(unitsInCollision);
        spell2Laser.StartMoving();

        Destroy(gameObject);
    }
}
