using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : UnitBase
{
    #region MovementParam
    public float moveSpeed = 1.1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    protected Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new();
    #endregion

    #region PatrolParam
    public Vector2 PatrolPoint;
    public float PatrolRadius;
    private Vector2 randomDestination;
    private float lastPatrol = 0;
    #endregion

    #region SensesParam
    public float seeDistance = 5f;
    protected float coneAngle = 45f;
    protected float coneDistance = 5f;
    protected float coneDirection = 180;
    #endregion

    protected Transform player;
    private Stats statistics;

    public override void Die()
    {
        Debug.Log($"{name} is dead");
    }

    public override void LockMovement()
    {
        throw new System.NotImplementedException();
    }

    public override void SetStats(Stats stats)
    {
        statistics = stats;
    }

    public override void TakeDamage(int dmg)
    {
        statistics.CurrentHp -= dmg;
        if (statistics.CurrentHp <= 0)
            Die();
    }

    public override bool TryMove(Vector2 direction)
    {
        direction.Normalize();
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                Vector3 pos = rb.position + moveSpeed * Time.fixedDeltaTime * direction;
                rb.MovePosition(pos);
                //Debug.Log(direction);
                return true;
            }

            return false;
        }

        return false;
    }

    public override void UnlockMovement()
    {
        throw new System.NotImplementedException();
    }
    protected void Patrol()
    {
        TryMove(randomDestination);
        if (Time.time - lastPatrol <= 1)
            return;
        randomDestination = Random.insideUnitCircle * PatrolRadius;//new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        randomDestination += PatrolPoint;

        //Debug.Log(randomDestination);
        lastPatrol = Time.time;
    }
    protected bool SeeSense(float heading)
    {
        if (Vector2.Distance(player.position, transform.position) >= seeDistance)
            return false;
        Vector2 dir = (Vector2)player.position - (Vector2)transform.position;
        coneDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector2 directionToPosition = (Vector2)player.position - (Vector2)transform.position;
        Vector2 headingVector = new Vector2(Mathf.Cos(heading * Mathf.Deg2Rad), Mathf.Sin(heading * Mathf.Deg2Rad));
        float angleToPosition = Vector2.Angle(headingVector, directionToPosition);

        if (angleToPosition <= coneAngle / 2f && directionToPosition.magnitude <= coneDistance)
            return true;
        else
            return false;
    }
}
