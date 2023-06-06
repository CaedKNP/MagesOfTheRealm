using Assets._Scripts.Spells;
using System.Collections.Generic;
using UnityEngine;

public class Spell1LaserGreen : SpellProjectileBase
{
    List<AttackHandler> unitsInCollision = new();
    GameObject laserPoint;
    protected void Awake()
    {
        base.MyAwake();
        Invoke("TimeOut", 0.5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AttackHandler unit))
        {
            unitsInCollision.Add(unit);
        }
    }
    private void MoveLaserToUnits(ref GameObject laserPointPref)
    {
        foreach (AttackHandler unit in unitsInCollision)
        {
            laserPointPref.transform.position = unit.transform.position;
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