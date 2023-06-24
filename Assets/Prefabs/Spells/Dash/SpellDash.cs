﻿using UnityEngine;

public class SpellDash : SpellBase
{
    GameObject player;
    HeroUnitBase playerScript;
    StaffRotation staffRotation;
    [SerializeField]
    private float moveSpeed = 40f;
    private Rigidbody2D rb;
    [SerializeField]
    private ContactFilter2D movementFilter;
    private Vector2 mousePosition;
    [SerializeField]
    private float destroyDelay = 0.5f;
    [SerializeField]
    private float positionThreshold = 0.1f;

    private float destroyTimer;
    private bool isDashing;
    private bool moved = false;

    private void Awake()
    {
        SetSpellStats();
        player = GameManager.Player;
        if (player.TryGetComponent(out AttackHandler unit))
        {
            unit.DAMAGE(DMG, conditions);
        }
        player.TryGetComponent<HeroUnitBase>(out playerScript);
        playerScript.HideWand();
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.identity;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destroyTimer = 1f; // Zerowanie licznika czasu na starcie
        transform.position = player.transform.position;
        Invoke("DestroyObject", destroyTimer);
        SetPlayerRendering(false);
        isDashing = true; // Ustawienie flagi na true na początku dashu
    }

    void SetPlayerRendering(bool enableRendering)
    {
        if (enableRendering)
        {
            //Renderer staffRotatorRenderer = staffRotation.GetComponent<Renderer>();
            //Color staffRotatorColor = staffRotatorRenderer.material.color;
            //staffRotatorColor.a = 0.5f; // 50% przezroczystość
            //staffRotatorRenderer.material.color = staffRotatorColor;
            //.

        }
        else
        {

        }
        Renderer playerRenderer = player.GetComponent<Renderer>();
        playerRenderer.enabled = enableRendering;
    }

    private void FixedUpdate()
    {
        if (!isDashing) return; // Przerwij działanie, jeśli nie trwa dash

        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        if (Vector2.Distance(transform.position, mousePosition) <= positionThreshold)
        {
            DestroyObject();
            return;
        }
        //if (Physics2D.Raycast((Vector2)transform.position, mousePosition, Vector2.Distance(transform.position, mousePosition) + 2, 7).collider != null)
        //{
        //    DestroyObject();
        //    return;
        //}
        if (TryMove(direction))
        {
            destroyTimer += Time.deltaTime; // Zwiększanie licznika czasu
        }
        else
        {
            destroyTimer = destroyDelay; // Resetowanie licznika czasu, jeśli ruch został zatrzymany przez ograniczenie
        }

        //Przemieszczanie gracza w kierunku dashu
        if (moved)
        {
            Vector3 playerDirection = transform.position - player.transform.position;
            playerDirection.Normalize();
            player.transform.position += playerDirection * moveSpeed * Time.deltaTime;
        }
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
        SetPlayerRendering(true);
        playerScript.UnHideWand();
        TeleportPlayer();
        isDashing = false; // Ustawienie flagi na false po zakończeniu dashu
        Destroy(gameObject);
    }

    bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Sprawdzanie potencjalnych kolizji
            RaycastHit2D[] hits = new RaycastHit2D[1];
            int count = rb.Cast(direction, movementFilter, hits, moveSpeed * Time.deltaTime);

            //Vector2 offsetPos = transform.position;
            //offsetPos.y -= 0.6f;
            //hits = Physics2D.RaycastAll(offsetPos, direction, 20, 7);

            //int count = hits.Length;

            if (count == 0)
            {
                rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * direction);
                moved = true;
                return true;
            }
        }
        // Nie można poruszać się, jeśli brak kierunku ruchu
        rb.MovePosition(rb.position + (moveSpeed) * Time.deltaTime * -direction);
        DestroyObject();
        return false;
    }
}