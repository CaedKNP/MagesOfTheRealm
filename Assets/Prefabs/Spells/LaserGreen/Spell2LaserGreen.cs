using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell2LaserGreen : MonoBehaviour
{
    float laserSpeed = 1f;

    List<UnitBase> unitsInCollision = new List<UnitBase>();

    public void SetUnitsInCollision(List<UnitBase> units)
    {
        unitsInCollision = units;
    }

    public void StartMoving()
    {
        StartCoroutine(MoveLaserToPoint());
    }

    IEnumerator MoveLaserToPoint()
    {
        foreach (UnitBase unit in unitsInCollision)
        {
            Vector3 targetPosition = unit.transform.position;

            while (Vector3.Distance(transform.position, targetPosition) > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, laserSpeed * Time.deltaTime);
                yield return null;
            }
        }

        Destroy(gameObject);
    }

}
