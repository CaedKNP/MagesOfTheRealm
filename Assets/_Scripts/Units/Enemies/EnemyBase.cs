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

    Component conditionsBar;

    ConditionUI _conditionUI;

    Coroutine burnRoutine, freezeRoutine, slowRoutine, speedUpRoutine, poisonRoutine, armorUpRoutine, armorDownRoutine, hasteRoutine, dmgUpRoutine;

    public override void Die()
    {
        Debug.Log($"{name} is dead");
    }

    public override void SetStats(Stats stats)
    {
        statistics = stats;
    }

    public override void TakeDamage(float dmg, List<ConditionBase> conditions)
    {
        ConditionAffect(conditions);
        statistics.CurrentHp -= Convert.ToInt32(dmg);
        if (statistics.CurrentHp <= 0)
            Die();
        return;
    }

    private void ConditionAffect(List<ConditionBase> conditions)
    {
        foreach (ConditionBase condition in conditions)
        {
            Affect(condition);
        }
    }

    private void Affect(ConditionBase condition)
    {
        switch (condition.Conditions)
        {
            case global::Conditions.Burn:

                burnRoutine ??= StartCoroutine(BurnTask(condition));

                break;
            case global::Conditions.Slow:

                slowRoutine ??= StartCoroutine(SlowTask(condition));

                break;
            case global::Conditions.Freeze:

                freezeRoutine ??= StartCoroutine(FreezeTask(condition));

                break;
            case global::Conditions.Poison:

                poisonRoutine ??= StartCoroutine(PoisonTask(condition));

                break;
            case global::Conditions.SpeedUp:

                speedUpRoutine ??= StartCoroutine(SpeedUpTask(condition));

                break;
            case global::Conditions.ArmorUp:

                armorUpRoutine ??= StartCoroutine(ArmorUpTask(condition));

                break;
            case global::Conditions.ArmorDown:

                armorDownRoutine ??= StartCoroutine(ArmorDownTask(condition));

                break;
            case global::Conditions.Haste:

                hasteRoutine ??= StartCoroutine(HasteTask(condition));

                break;
            case global::Conditions.DmgUp:

                dmgUpRoutine ??= StartCoroutine(DmgUpTask(condition));

                break;
            default:
                break;
        }
    }

    private IEnumerator BurnTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(0);

        var end = DateTime.Now.Second + condition.AffectTime;

        while (DateTime.Now.Second < end)
        {
            statistics.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            if (statistics.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(0);
        burnRoutine = null;
    }

    private IEnumerator SlowTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(1);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempSpeed = statistics.MovementSpeed;

        statistics.MovementSpeed -= statistics.MovementSpeed * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(1);
        statistics.MovementSpeed = tempSpeed;
        slowRoutine = null;
    }

    private IEnumerator PoisonTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(2);
        var end = DateTime.Now.Second + condition.AffectTime;

        while (DateTime.Now.Second < end)
        {
            statistics.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            if (statistics.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(2);
        poisonRoutine = null;
    }

    private IEnumerator FreezeTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(3);

        _canMove = false;

        var end = DateTime.Now.Second + condition.AffectTime;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(3);
        _canMove = true;
        freezeRoutine = null;
    }

    private IEnumerator SpeedUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(4);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempMoveSpeed = statistics.MovementSpeed;

        statistics.MovementSpeed += statistics.MovementSpeed * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(4);
        statistics.MovementSpeed = tempMoveSpeed;
        speedUpRoutine = null;
    }

    private IEnumerator ArmorUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(5);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempArmor = statistics.Armor;

        statistics.Armor += statistics.Armor * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.Armor = tempArmor;

        _conditionUI.RemoveConditionSprite(5);
        armorUpRoutine = null;
    }

    private IEnumerator ArmorDownTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(6);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempArmorDown = statistics.Armor;

        statistics.Armor -= statistics.Armor * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.Armor = tempArmorDown;

        _conditionUI.RemoveConditionSprite(6);
        armorDownRoutine = null;
    }

    private IEnumerator HasteTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(7);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempCooldown = statistics.CooldownModifier;

        statistics.CooldownModifier += statistics.CooldownModifier * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.CooldownModifier = tempCooldown;

        _conditionUI.RemoveConditionSprite(7);
        hasteRoutine = null;
    }

    private IEnumerator DmgUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(8);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempDmg = statistics.DmgModifier;

        statistics.DmgModifier += statistics.DmgModifier * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.DmgModifier = tempDmg;

        _conditionUI.RemoveConditionSprite(8);
        dmgUpRoutine = null;
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
