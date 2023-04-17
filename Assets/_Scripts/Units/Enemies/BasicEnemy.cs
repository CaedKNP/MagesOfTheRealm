using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : UnitBase
{
    public float moveSpeed = 1.1f;
    public float minDistance = 0.1f;
    public float seeDistance = 5f;
    public float attackCooldown;

    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Transform player;
    List<RaycastHit2D> castCollisions = new();

    private float dotSize = 0.1f;
    private Color dotColor = Color.green;

    private float coneAngle = 45f;
    private float coneDistance = 5f;
    float coneDirection = 180;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameManager.Player.transform;
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

    #region senses
    /// <summary>
    /// Detect player in cone
    /// </summary>
    /// <param name="heading"></param>
    /// <returns></returns>
    private bool SeeSense(float heading)
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
    #endregion

    public override void SetStats(Stats stats)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(int dmg)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Detect collision and move rigidbody if possible
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>false if moving not possible</returns>
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
                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
                return true;
            }

            return false;
        }

        return false;
    }

    public override void LockMovement()
    {
        throw new System.NotImplementedException();
    }

    public override void UnlockMovement()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
    public void Attack()
    {
        if (Time.time - lastAttack <= attackCooldown)
            return;
        dotColor = Color.red;
        lastAttack = Time.time;
        GameManager.Player.GetComponent<HeroUnitBase>().TakeDamage(10);
    }
}
