using Assets._Scripts.Spells;
using System.Collections.Generic;
using UnityEngine;

public class Spell1LaserGreen : SpellProjectileBase
{
    List<AttackHandler> _unitsInCollision = new();
    public GameObject laserPoint;
    StaffRotation staff;
    protected void Awake()
    {
        staff = GetComponentInChildren<StaffRotation>();
        base.MyAwake();
        Invoke("TimeToShoot", 1f);
        Invoke("TimeOut", 3f);  
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AttackHandler unit))
        {
            _unitsInCollision.Add(unit);
        }
    }
    private void TimeToShoot()
    {
        Debug.Log("UnitsInCllision " + "laserPointPref before spawn");
        GameObject laserPointPref = Instantiate(laserPoint, staff.WizandStaffFirePint.transform.position, staff.WizandStaffFirePint.transform.rotation);
        Debug.Log("UnitsInCllision " + "laserPointPref after spawn");
        foreach (var unit in _unitsInCollision)
        {
            laserPointPref.transform.position = unit.transform.position;
            Debug.Log("UnitsInCllision " + unit.transform.position);
        }
    }

    void TimeOut()
    {
        Destroy(gameObject);
        Destroy(laserPoint);//nie działa
    }
}