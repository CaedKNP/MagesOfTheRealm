using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1LaserGreen : SpellBase
{
    List<UnitBase> unitsInCollision = new List<UnitBase>();
    GameObject laserPoint;
    protected void Awake()
    {

        SetSpeedDestroyTime(40f, 1f);
        base.MyAwake();
        Invoke("TimeOut", 0.5f);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unitsInCollision.Add(unit); 
        }
    }
    private void MoveLaserToUnits(ref GameObject laserPointPref)
    {
        foreach (UnitBase unit in unitsInCollision)
        {
            laserPointPref.transform.position = unit.transform.position;
        }
    }
    void TimeOut()
    {
        laserPoint = GetComponent<GameObject>();
        GameObject laserPointPref = Instantiate(laserPoint, transform.position, transform.rotation);
        MoveLaserToUnits(ref laserPointPref);
        Destroy(gameObject);
        Destroy(laserPointPref);//nie działa
    }
}
