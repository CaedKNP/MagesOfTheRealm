using Assets._Scripts.Spells;
using System.Collections.Generic;
using UnityEngine;

public class Spell1LaserGreen : SpellProjectileBase
{
    List<AttackHandler> unitsInCollision = new();
    public GameObject laserPoint;
    protected void Awake()
    {
        base.MyAwake();
        Invoke("TimeOut", 3f);
        Invoke("TimeToShoot", 1f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AttackHandler unit))
        {
            unitsInCollision.Add(unit);
            Debug.Log("dsdsdsd "+unitsInCollision.Count);
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
        laserPoint = GetComponent<GameObject>();
        GameObject laserPointPref = Instantiate(laserPoint, transform.position, transform.rotation);
        MoveLaserToUnits(ref laserPointPref);
        Destroy(gameObject);
        Destroy(laserPointPref);//nie działa
    }
}