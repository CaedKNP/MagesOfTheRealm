using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1.1f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTo(player.position);
    }

    void MoveTo(Vector2 _targetPosition)
    {
        Vector2 direction = _targetPosition - rb.position;
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
    }

}
