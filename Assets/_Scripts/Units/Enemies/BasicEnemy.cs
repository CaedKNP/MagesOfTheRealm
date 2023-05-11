using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BasicEnemy : EnemyBase
{
    public float minDistance = 0.1f;
    public float attackCooldown = 100;

    SpriteRenderer spriteRenderer;

    private float dotSize = 0.1f;
    private Color dotColor = Color.green;

    private float lastAttack = 0;

    public enum States
    {
        Idle,
        Moving,
        Attacking
    }

    private States currentState;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Player.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = States.Moving;
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
            default:
                Debug.LogWarning($"Invalid state: {currentState}");
                break;
        }
    }

    #region states
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
        if (Vector2.Distance(transform.position, player.position) <= minDistance)
            ChangeState(States.Attacking);
        if (!SeeSense(coneDirection))
            ChangeState(States.Idle);
    }

    private void Attacking()
    {
        Attack();
        if (Vector2.Distance(transform.position, player.position) > minDistance)
            ChangeState(States.Idle);
    }

    private void ChangeState(States newState)
    {
        currentState = newState;
    }
    #endregion

    public void Attack()
    {
        if (Time.time - lastAttack <= attackCooldown)
            return;
        dotColor = Color.red;
        lastAttack = Time.time;
        GameManager.Player.GetComponent<HeroUnitBase>().TakeDamage(new List<Conditions>() { Conditions.Burn}, 0, 3, 1);
        //Debug.Log("HIT!");
    }
}