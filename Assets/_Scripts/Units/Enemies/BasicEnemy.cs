using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float moveSpeed = 1.1f;
    public float minDistance = 1f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

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
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
    }
}