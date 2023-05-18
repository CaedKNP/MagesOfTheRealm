using System;
using UnityEngine;

public class SpellDash : MonoBehaviour
{
    GameObject player;
    StaffRotation staffRotation;
    public float moveSpeed = 40f;
    Rigidbody2D rb;
    Vector2 mousePosition;
    float destroyTimer = 0.3f; // Licznik czasu  
    Vector2 direction; // Kierunek poruszania się  

    void Awake()
    {
        SetPrefPosition();
        SetDirection();
        Invoke("DestroyObject", destroyTimer);
    }
    void SetPrefPosition()
    {
        player = GameManager.Player;
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.identity;
        transform.position = player.transform.position;
    }

    void SetDirection()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();
    }

    void SetPlayerRendering(bool enableRendering)
    {
    }

    void FixedUpdate()
    {
        if (TryMove(direction)) { }
        PullPlayer();
    }

    void PullPlayer()
    {
        Vector3 playerDirection = transform.position - player.transform.position;
        playerDirection.Normalize();
        player.transform.position += playerDirection * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            DestroyObject();
        }
    }

    void TeleportPlayer()
    {
        player.transform.position = transform.position;
    }

    void DestroyObject()
    {
        TeleportPlayer();
        Destroy(gameObject);
    }

    bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Sprawdzanie potencjalnych kolizji  
            RaycastHit2D[] hits = new RaycastHit2D[1];
            int count = rb.Cast(direction, hits, moveSpeed * Time.deltaTime);

            if (count == 0)
            {
                rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * direction);
                return true;
            }
        }
        // Nie można poruszać się, jeśli brak kierunku ruchu  
        DestroyObject();
        return false;
    }
}