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

    [SerializeField]
    protected AiData aiData = new();
    [SerializeField]
    private ContextSolver contextSolver = new();

    protected Transform player;
    protected Animator _anim;
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private intSO scoreSO;

    void Awake()
    {
        conditionsBar = gameObject.transform.GetChild(0);

        _conditionUI = conditionsBar.GetComponent<ConditionUI>();
    }

    public override void Die()
    {
        base.Die();
        scoreSO.Int++;
        _anim.CrossFade("Death", 0, 0);
        _isDead = true;
        GameManager.enemies.Remove(this.gameObject);
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
                stats.MovementSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

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

    protected void Move()
    {
        heading = contextSolver.ChooseDirection();
        if (!TryMove(heading))
        {
            for (int i = 0; i < 8; i++)
            {
                heading = aiData.direction[i];
                if (TryMove(heading))
                    break;
            }
        }

        if (!TryMove(heading))
            return;

        Vector3 pos = rb.position + stats.MovementSpeed * Time.fixedDeltaTime * heading;
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
}
