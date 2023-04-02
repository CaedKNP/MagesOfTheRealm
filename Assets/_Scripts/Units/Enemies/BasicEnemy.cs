using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float moveSpeed = 1.1f;
    public float minDistance = 0.1f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Transform player;

    private float dotSize = 0.1f;
    private Color dotColor = Color.green;

    private float coneAngle = 45f;
    private float coneDistance = 5f;
    float coneDirection = 180;

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

    private void Idle()
    {
        dotColor = Color.green;
        if (SeeSense(coneDirection))
            ChangeState(States.Moving);
    }

    private void Moving()
    {
        MoveTo(player.position);
        if (Vector2.Distance(transform.position, player.position) <= minDistance)
            ChangeState(States.Attacking);
        if (!SeeSense(coneDirection))
            ChangeState(States.Idle);
    }

    private void Attacking()
    {
        dotColor = Color.red;
        if (Vector2.Distance(transform.position, player.position) > minDistance)
            ChangeState(States.Idle);
    }

    private void ChangeState(States newState)
    {
        currentState = newState;
    }

    private bool SeeSense(float heading)
    {
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

    void MoveTo(Vector2 _targetPosition)
    {
        Vector2 direction = _targetPosition - rb.position;
        direction.Normalize();
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
    }
}
