using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class EnemyBase : UnitBase
{
    #region MovementParam
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    protected Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new();
    bool _canMove = true;

    #endregion

    #region PatrolParam
    public Vector2 PatrolPoint;
    public float PatrolRadius;
    private Vector2 randomDestination;
    private float lastPatrol = 0;
    #endregion

    #region SensesParam
    public float seeDistance = 10f;
    protected float coneAngle = 45f;
    protected float coneDistance = 5f;
    protected float coneDirection = 180;
    #endregion

    protected Transform player;
    protected Stats statistics;
    protected Animator _anim;
    protected SpriteRenderer spriteRenderer;

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

    public override async Task TakeDamage(float dmg, List<ConditionBase> conditions)
    {

        await ConditionAffect(conditions);
        statistics.CurrentHp -= Convert.ToInt32(dmg);
        if (statistics.CurrentHp <= 0)
            Die();
        return;
    }

    private async Task ConditionAffect(List<ConditionBase> conditions)
    {
        foreach (ConditionBase condition in conditions)
        {
            await Affect(condition);
        }

        return;
    }

    private async Task Affect(ConditionBase con)
    {
        float end;

        switch (con.Conditions)
        {
            case global::Conditions.Burn:

                await GetTickDmg(con.AffectTime, con.AffectOnTick);

                break;
            case global::Conditions.Slow:

                end = Time.time + con.AffectTime;
                var tempSpeed = statistics.MovementSpeed;

                while (Time.time < end)
                {
                    statistics.MovementSpeed -= con.AffectOnTick;
                    await Task.Yield();
                }

                statistics.MovementSpeed = tempSpeed;

                break;
            case global::Conditions.Freeze:

                end = Time.time + con.AffectTime;

                while (Time.time < end)
                {
                    _canMove = false;
                    await Task.Yield();
                }

                break;
            case global::Conditions.Poison:
                await GetTickDmg(con.AffectTime, con.AffectOnTick);
                break;
            case global::Conditions.SpeedUp:

                end = Time.time + con.AffectTime;
                var tempMoveSpeed = _canMove;

                while (Time.time < end)
                {
                    statistics.MovementSpeed += con.AffectOnTick;
                    await Task.Yield();
                }

                _canMove = tempMoveSpeed;

                break;
            case global::Conditions.ArmorUp:

                end = Time.time + con.AffectTime;
                var tempArmor = statistics.Armor;

                while (Time.time < end)
                {
                    statistics.Armor += con.AffectOnTick;
                    await Task.Yield();
                }

                statistics.Armor = tempArmor;

                break;
            case global::Conditions.ArmorDown:

                end = Time.time + con.AffectTime;
                var tempArmorDown = statistics.Armor;

                while (Time.time < end)
                {
                    statistics.Armor -= con.AffectOnTick;
                    await Task.Yield();
                }

                statistics.Armor = tempArmorDown;

                break;
            case global::Conditions.Haste:

                end = Time.time + con.AffectTime;
                var tempCooldown = statistics.CooldownModifier;

                while (Time.time < end)
                {
                    statistics.CooldownModifier += con.AffectOnTick;
                    await Task.Yield();
                }

                statistics.CooldownModifier = tempCooldown;

                break;
            case global::Conditions.DmgUp:

                end = Time.time + con.AffectTime;
                var tempDmg = statistics.DmgModifier;

                while (Time.time < end)
                {
                    statistics.DmgModifier += con.AffectOnTick;
                    await Task.Yield();
                }

                statistics.DmgModifier = tempDmg;

                break;
            default:
                break;
        }
    }

    private async Task GetTickDmg(float affectTime, float dmgToTake)
    {
        var end = Time.time + affectTime;
        while (Time.time < end)
        {
            statistics.CurrentHp -= Convert.ToInt32(dmgToTake);
            await Task.Delay(1000);
        }
    }

    public void StopAnimation()
    {
        _anim.CrossFade("Idle", 0, 0);
    }
    public override bool TryMove(Vector2 direction)
    {
        if (!_canMove)
            return false;

        _anim.CrossFade("Walk", 0, 0);
        direction.Normalize();
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                statistics.MovementSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                Vector3 pos = rb.position + statistics.MovementSpeed * Time.fixedDeltaTime * direction;
                rb.MovePosition(pos);
                if (direction.x < 0)
                {
                    spriteRenderer.flipX = false;
                }
                else if (direction.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                //Debug.Log(direction);
                return true;
            }

            return false;
        }
        else
        {
            _anim.CrossFade("Idle", 0, 0);
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
