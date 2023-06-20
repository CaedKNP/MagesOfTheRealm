using Assets._Scripts.Utilities;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// State machine and its logic for enemy
/// </summary>
public class BasicEnemy : EnemyBase
{
    [SerializeField]
    private bool attackFromCenter;
    [SerializeField]
    private float rangeOfAttack = 0.1f;
    [SerializeField]
    private float rangeOfRest = 2f;
    [SerializeField]
    private float rangeOfChase = 5f;
    [SerializeField]
    private float attackCooldown = 5;
    [SerializeField]
    private Spell spell;
    bool onCooldown = false;

    private float dotSize = 0.2f;
    private Color dotColor = Color.green;

    private float lastAttack = 0;

    private enum States
    {
        Idle,
        Moving,
        Attacking,
        Rest,
        Die
    }

    private States currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Player.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = States.Moving;
        _anim = GetComponent<Animator>();
        aiData.currentTarget = player.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = dotColor;
        Gizmos.DrawSphere(transform.position, dotSize);
        //float halfFOV = coneAngle / 2.0f;

        //Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
        //Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

        //Vector3 upRayDirection = upRayRotation * transform.right * coneDistance;
        //Vector3 downRayDirection = downRayRotation * transform.right * coneDistance;

        //Gizmos.DrawRay(transform.position, upRayDirection);
        //Gizmos.DrawRay(transform.position, downRayDirection);
        //Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);

        //Gizmos.color = Color.red;

        //foreach (Collider2D e in avoid)
        //{
        //    if (e == null)
        //        continue;
        //    Gizmos.DrawSphere(e.transform.position, dotSize);
        //}

    }

    void Update()
    {
        switch (currentState)
        {
            case States.Idle:
                Idle();
                break;
            case States.Moving:
                Moving();
                break;
            case States.Attacking:
                Attacking();
                break;
            case States.Rest:
                Resting();
                break;
            case States.Die:
                Death();
                break;
            default:
                Debug.LogWarning($"Invalid state: {currentState}");
                break;
        }
    }

    private void Death()
    {
        dotColor = Color.black;
    }

    #region states
    private void Resting()
    {
        dotColor = Color.yellow;

        if (Time.time - lastAttack > attackCooldown)
            onCooldown = false;
        if (!onCooldown)
            ChangeState(States.Moving);
        if (Vector2.Distance(transform.position, player.position) < rangeOfRest)
            Escape();
        else
            ChangeState(States.Idle);
        if (_isDead)
            ChangeState(States.Die);
    }
    private void Idle()
    {
        if (Time.time - lastAttack > attackCooldown)
        {
            //Debug.Log($"Time: {Time.time}| Last Attack: {lastAttack}");
            onCooldown = false;
            ChangeState(States.Attacking);
        }
        dotColor = Color.blue;
        StopAnimation();
        //Patrol();
        if (Vector2.Distance(transform.position, player.position) > rangeOfChase)
            ChangeState(States.Moving);
        if (Vector2.Distance(transform.position, player.position) < rangeOfRest)
            ChangeState(States.Rest);
        if (!onCooldown)
            ChangeState(States.Moving);
        if (_isDead)
            ChangeState(States.Die);
    }
    private void Moving()
    {
        dotColor = Color.green;
        Move();
        if (Vector2.Distance(transform.position, player.position) <= rangeOfAttack)
            ChangeState(States.Attacking);
        if (Vector2.Distance(transform.position, player.position) <= rangeOfRest)
            if (onCooldown)
                ChangeState(States.Rest);
        //if (Vector2.Distance(transform.position, player.position) <= rangeOfRest)
        //    ChangeState(States.Rest);
        if (_isDead)
            ChangeState(States.Die);
    }

    private void Attacking()
    {
        dotColor = Color.red;
        Attack();
        if (onCooldown)
            ChangeState(States.Rest);
        if (Vector2.Distance(transform.position, player.position) > rangeOfAttack)
            ChangeState(States.Moving);
        if (_isDead)
            ChangeState(States.Die);
    }

    private void ChangeState(States newState)
    {
        currentState = newState;
    }
    #endregion

    /// <summary>
    /// Execute attached spell in direction of player
    /// </summary>
    public void Attack()
    {
        if (onCooldown)
            return;

        onCooldown = true;
        lastAttack = Time.time;
        if (attackFromCenter)
        {
            spell.Attack(transform.position, Quaternion.identity);
            return;
        }

        Vector3 dirToPlayer = (player.position - transform.position);
        dirToPlayer.Normalize();
        dirToPlayer *= 2;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        spell.Attack(transform.position + dirToPlayer, Quaternion.AngleAxis(angle, Vector3.forward));
    }

    private void Escape()
    {
        Move();//need to find trasform of running away
    }
}