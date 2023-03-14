using UnityEngine;

public class FireballSpell : MonoBehaviour
{
    float speed = 1f; // prędkość ruchu prefaba

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // pobieramy Rigidbody2D komponent z prefabu
        rb.velocity = transform.right * speed; // ustawiamy prędkość w kierunku "przodu" prefabu
        Destroy(gameObject, 2);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
