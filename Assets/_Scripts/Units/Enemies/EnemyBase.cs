using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public abstract class EnemyBase : UnitBase
{
    #region MovementParam
    public float moveSpeed = 1.1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    protected Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new();
    bool _canMove;

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

    public override async Task TakeDamage(List<Conditions> conditions, int dmg, float affectTime, int dmgToTake)
    {

        await ConditionAffect(conditions, affectTime, dmgToTake);
        statistics.CurrentHp -= dmg;
        if (statistics.CurrentHp <= 0)
            Die();
        return;
    }
    private async Task ConditionAffect(List<Conditions> conditions, float affectTime, int dmgToTake)
    {
        foreach (Conditions con in conditions)
        {
            await Affect(con, affectTime, dmgToTake);
        }

        return;
    }

    private async Task Affect(Conditions con, float affectTime, int dmgToTake)
    {
        float end;

        switch (con)
        {
            case global::Conditions.Burn:

                await GetTickDmg(affectTime, dmgToTake);

                break;
            case global::Conditions.Slow:

                end = Time.time + affectTime;
                var tempSpeed = statistics.MovementSpeed;

                while (Time.time < end)
                {
                    statistics.MovementSpeed -= dmgToTake;
                    await Task.Yield();
                }

                statistics.MovementSpeed = tempSpeed;

                break;
            case global::Conditions.Freeze:

                end = Time.time + affectTime;

                while (Time.time < end)
                {
                    _canMove = false;
                    await Task.Yield();
                }

                break;
            case global::Conditions.Poison:
                await GetTickDmg(affectTime, dmgToTake);
                break;
            case global::Conditions.SpeedUp:

                end = Time.time + affectTime;
                var tempMoveSpeed = _canMove;

                while (Time.time < end)
                {
                    statistics.MovementSpeed += dmgToTake;
                    await Task.Yield();
                }

                _canMove = tempMoveSpeed;

                break;
            case global::Conditions.ArmorUp:

                end = Time.time + affectTime;
                var tempArmor = statistics.Armor;

                while (Time.time < end)
                {
                    statistics.Armor += dmgToTake;
                    await Task.Yield();
                }

                statistics.Armor = tempArmor;

                break;
            case global::Conditions.ArmorDown:

                end = Time.time + affectTime;
                var tempArmorDown = statistics.Armor;

                while (Time.time < end)
                {
                    statistics.Armor -= dmgToTake;
                    await Task.Yield();
                }

                statistics.Armor = tempArmorDown;

                break;
            case global::Conditions.Haste:

                end = Time.time + affectTime;
                var tempCooldown = statistics.CooldownModifier;

                while (Time.time < end)
                {
                    statistics.CooldownModifier += dmgToTake;
                    await Task.Yield();
                }

                statistics.CooldownModifier = tempCooldown;

                break;
            case global::Conditions.DmgUp:

                end = Time.time + affectTime;
                var tempDmg = statistics.DmgModifier;

                while (Time.time < end)
                {
                    statistics.DmgModifier += dmgToTake;
                    await Task.Yield();
                }

                statistics.DmgModifier = tempDmg;

                break;
            default:
                break;
        }
    }

    private async Task GetTickDmg(float affectTime, int dmgToTake)
    {
        var end = Time.time + affectTime;
        while (Time.time < end)
        {
            statistics.CurrentHp -= dmgToTake;
            await Task.Delay(1000);
        }
    }

    public override bool TryMove(Vector2 direction)
    {
        if (!_canMove)
            return false;

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
