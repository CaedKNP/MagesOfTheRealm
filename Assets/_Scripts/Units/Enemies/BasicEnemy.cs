using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BasicEnemy : EnemyBase
{
    public float rangeOfAttack = 0.1f;
    public float rangeOfRest = 2f;
    public float attackCooldown = 5;
    bool onCooldown = false;

    private float dotSize = 0.1f;
    private Color dotColor = Color.green;

    private float lastAttack = 0;

    public enum States
    {
        Idle,
        Moving,
        Attacking,
        Rest
    }

    private States currentState;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Player.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = States.Moving;
        _anim = GetComponent<Animator>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = dotColor;
        Gizmos.DrawSphere(transform.position, dotSize);
        float halfFOV = coneAngle / 2.0f;

        Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
        Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

        Vector3 upRayDirection = upRayRotation * transform.right * coneDistance;
        Vector3 downRayDirection = downRayRotation * transform.right * coneDistance;

        Gizmos.DrawRay(transform.position, upRayDirection);
        Gizmos.DrawRay(transform.position, downRayDirection);
        Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
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
            default:
                Debug.LogWarning($"Invalid state: {currentState}");
                break;
        }
    }

    #region states
    private void Resting()
    {
        if (Time.time - lastAttack > attackCooldown)
        {
            onCooldown = false;
            ChangeState(States.Attacking);
        }
        if (Vector2.Distance(transform.position, player.position) <= rangeOfRest)
            Escape();
        else
            ChangeState(States.Moving);
    }
    private void Idle()
    {
        dotColor = Color.green;
        Patrol();
        if (SeeSense(coneDirection))
            ChangeState(States.Moving);
    }
    private void Moving()
    {
        TryMove(player.position - transform.position);
        if (Vector2.Distance(transform.position, player.position) <= rangeOfAttack)
            ChangeState(States.Attacking);
        if (Vector2.Distance(transform.position, player.position) <= rangeOfRest)
            ChangeState(States.Rest);
    }

    private void Attacking()
    {
        Attack();
        if (onCooldown)
            ChangeState(States.Rest);
        if (Vector2.Distance(transform.position, player.position) <= rangeOfAttack)
            ChangeState(States.Moving);
    }

    private void ChangeState(States newState)
    {
        currentState = newState;
    }
    #endregion

    public void Attack()
    {
        if (onCooldown)
            return;
        onCooldown = true;
        dotColor = Color.red;
        lastAttack = Time.time;
        GameManager.Player.GetComponent<HeroUnitBase>().TakeDamage(new List<Conditions>() { Conditions.Burn}, 0, 3, 1);
        //Debug.Log("HIT!");
    }
    private void Escape()
    {
        TryMove(-(player.position - transform.position));
    }
}