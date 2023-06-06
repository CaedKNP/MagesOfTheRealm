using Assets._Scripts.Utilities;
using Assets.Resources.SOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;
using Stats = Assets._Scripts.Utilities.Stats;

public abstract class EnemyBase : UnitBase
{
    #region MovementParam
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    protected Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new();
    bool _canMove = true;
    protected Vector2 heading;
    protected float[] wages = new float[8];
    protected Collider2D[] avoid;

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

    [SerializeField]
    private LayerMask layerMask;

    protected Transform player;
    protected Animator _anim;
    protected SpriteRenderer spriteRenderer;

    protected Stats _stats;

    [SerializeField]
    Component conditionsBar;

    ConditionUI _conditionUI;

    [SerializeField]
    private intSO scoreSO;

    Coroutine burnRoutine, freezeRoutine, slowRoutine, speedUpRoutine, poisonRoutine, armorUpRoutine, armorDownRoutine, hasteRoutine, dmgUpRoutine;
    protected bool _isDead = false;

    void Awake()
    {
        conditionsBar = gameObject.transform.GetChild(0);

        _conditionUI = conditionsBar.GetComponent<ConditionUI>();
    }

    public override void Die()
    {
        Debug.Log($"{name} is dead");
        scoreSO.Int++;
        _anim.CrossFade("Death", 0, 0);
        _canMove = false;
        _isDead = true;
        GameManager.enemies.Remove(this.gameObject);
        Destroy(this.gameObject, 0.8f);
    }

    public override void SetStats(Stats stats)
    {
        this._stats = stats;
    }

    public override void TakeDamage(float dmgToTake, List<ConditionBase> conditions)
    {
        _stats.CurrentHp -= Convert.ToInt32(dmgToTake * _stats.Armor);

        if (_stats.CurrentHp <= 0)
            Die();

        ConditionAffect(conditions);
    }

    private void ConditionAffect(List<ConditionBase> conditions)
    {
        if (conditions.Count > 0 && conditions != null)
            foreach (ConditionBase condition in conditions)
                Affect(condition);
    }

    private void Affect(ConditionBase condition)
    {
        switch (condition.Condition)
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

        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
        {
            _stats.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            if (_stats.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(0);
        burnRoutine = null;
    }

    private IEnumerator SlowTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(1);

        var end = Time.time + condition.AffectTime;
        var tempSpeed = _stats.MovementSpeed;
        _stats.MovementSpeed -= _stats.MovementSpeed * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(1);
        _stats.MovementSpeed = tempSpeed;
        slowRoutine = null;
    }

    private IEnumerator PoisonTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(2);
        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
        {
            _stats.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            if (_stats.CurrentHp <= 0)
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

        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
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

        var end = Time.time + condition.AffectTime;
        var tempMoveSpeed = _stats.MovementSpeed;
        _stats.MovementSpeed += _stats.MovementSpeed * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(4);
        _stats.MovementSpeed = tempMoveSpeed;
        speedUpRoutine = null;
    }

    private IEnumerator ArmorUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(5);

        var end = Time.time + condition.AffectTime;
        var tempArmor = _stats.Armor;
        _stats.Armor += _stats.Armor * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _stats.Armor = tempArmor;

        _conditionUI.RemoveConditionSprite(5);
        armorUpRoutine = null;
    }

    private IEnumerator ArmorDownTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(6);

        var end = Time.time + condition.AffectTime;
        var tempArmorDown = _stats.Armor;
        _stats.Armor -= _stats.Armor * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _stats.Armor = tempArmorDown;

        _conditionUI.RemoveConditionSprite(6);
        armorDownRoutine = null;
    }

    private IEnumerator HasteTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(7);

        var end = Time.time + condition.AffectTime;
        var tempCooldown = _stats.CooldownModifier;
        _stats.CooldownModifier += _stats.CooldownModifier * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _stats.CooldownModifier = tempCooldown;

        _conditionUI.RemoveConditionSprite(7);
        hasteRoutine = null;
    }

    private IEnumerator DmgUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(8);

        var end = Time.time + condition.AffectTime;
        var tempDmg = _stats.DmgModifier;
        _stats.DmgModifier += _stats.DmgModifier * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _stats.DmgModifier = tempDmg;

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
        {
            if(_isDead)
                return false;
            StopAnimation();
            return false;
        }

        _anim.CrossFade("Walk", 0, 0);
        direction.Normalize();

        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                _stats.MovementSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
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

    protected void Move(Vector2 desiredDir)
    {
        int choosenDir = 0;
        Vector2[] dir = new Vector2[8];//possible directions
        float[] wagesGood = new float[8];
        float[] wagesBad = new float[8];
        avoid = Detect();

        dir[0] = new Vector2(0, 1).normalized;
        dir[1] = new Vector2(1, 1).normalized;
        dir[2] = new Vector2(1, 0).normalized;
        dir[3] = new Vector2(1, -1).normalized;
        dir[4] = new Vector2(0, -1).normalized;
        dir[5] = new Vector2(-1, -1).normalized;
        dir[6] = new Vector2(-1, 0).normalized;
        dir[7] = new Vector2(-1, 1).normalized;

        foreach (Collider2D obstacleCollider in avoid)
        {
            if (obstacleCollider.tag == "Player")
                continue;
            Vector2 directionToObstacle
                = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            //calculate weight based on the distance Enemy<--->Obstacle
            float weight = distanceToObstacle;

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < dir.Length; i++)
            {
                float result = Vector2.Dot(directionToObstacleNormalized, dir[i]);

                float valueToPutIn = result * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > wagesBad[i])
                {
                    wagesBad[i] = valueToPutIn;
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            float result = Vector2.Dot(desiredDir.normalized, dir[i]);
            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > wages[i])
                {
                    wagesGood[i] = valueToPutIn;
                }

            }
        }

        for (int i = 0; i < 8; i++)
        {
            //wages[i] = wagesGood[i];
            wages[i] = Mathf.Clamp01(wagesGood[i] - wagesBad[i]);
            //if (wages[choosenDir] < wages[i])
            //    choosenDir = i;
        }

        for (int i = 0; i < 8; i++)
        {
            heading += dir[i] * wages[i];
        }

        heading.Normalize();

        if (!TryMove(heading))
        {
            for (int i = 0; i < 8; i++)
            {
                heading = dir[i];
                if (TryMove(heading))
                    break;
            }
        }

        if (!TryMove(heading))
            return;

        Vector3 pos = rb.position + _stats.MovementSpeed * Time.fixedDeltaTime * heading;
        rb.MovePosition(pos);
        if (heading.x <= 0.1)
        {
            spriteRenderer.flipX = false;
        }
        else if (heading.x > 0.1)
        {
            spriteRenderer.flipX = true;
        }
    }

    private Vector2 GetClosestEnemy()
    {
        Vector2 pos = rb.position;
        Vector2 closestEnemy = rb.position;
        float closestDist = float.MaxValue;
        foreach (GameObject e in GameManager.enemies)
        {
            float tempDist = Vector2.Distance((Vector2)e.transform.position, pos);
            if (tempDist == 0)
                continue;
            if (tempDist < closestDist)
            {
                closestDist = tempDist;
                closestEnemy = e.transform.position;
            }
        }

        return closestEnemy;
    }
    public Collider2D[] Detect()
    {
        return Physics2D.OverlapCircleAll(transform.position, 2);
    }
}
