using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float moveSpeed = 1.1f;
    public float minDistance = 1f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new();
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(rb.position, GameManager.Player.transform.position) >= minDistance)
            MoveTo(GameManager.Player.transform.position);
    }

    void MoveTo(Vector2 _targetPosition)
    {
        Vector2 direction = _targetPosition - rb.position;
        direction.Normalize();
        TryMove(direction);
    }

    private bool TryMove(Vector2 direction)
    {
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

        // Can't move if there's no direction to move in
        return false;
    }

}
