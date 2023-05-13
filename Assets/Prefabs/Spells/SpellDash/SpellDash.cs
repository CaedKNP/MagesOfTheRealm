using UnityEngine;

public class SpellDash : MonoBehaviour
{
    GameObject player;
    public float moveSpeed = 40f;
    private Rigidbody2D rb;
    private Vector2 mousePosition;
    public float destroyDelay = 0.5f; // Czas opóźnienia przed zniszczeniem
    public float positionThreshold = 0.1f; // Prog odchylenia od pozycji docelowej

    private float destroyTimer; // Licznik czasu

    private void Awake()
    {
        player = GameManager.Player;
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.identity;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destroyTimer = 1f; // Zerowanie licznika czasu na starcie
        transform.position = player.transform.position;
        Invoke("DestroyObject", destroyTimer);
    }

    private void FixedUpdate()
    {
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        if (Vector2.Distance(transform.position, mousePosition) <= positionThreshold)
        {
            DestroyObject();
        }

        if (TryMove(direction))
        {
            destroyTimer += Time.deltaTime; // Zwiększanie licznika czasu
        }
        else
        {
            destroyTimer = destroyDelay; // Resetowanie licznika czasu, jeśli ruch został zatrzymany przez ograniczenie
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
