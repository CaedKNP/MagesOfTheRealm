using UnityEngine;

public class SpellFrost : MonoBehaviour
{
    float speed = 2f; // prędkość ruchu prefaba

    private Rigidbody2D rb;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // pobieramy Rigidbody2D komponent z prefabu
        rb.velocity = transform.right * speed; // ustawiamy prędkość w kierunku "przodu" prefabu

        //// dodajemy komponent Trail Renderer
        //trailRenderer = gameObject.AddComponent<TrailRenderer>();

        //// ustawiamy kolor linii na niebieski
        //trailRenderer.startColor = Color.blue;
        //trailRenderer.endColor = Color.blue;

        //// ustawiamy grubość linii na 3
        //trailRenderer.startWidth = 3f;
        //trailRenderer.endWidth = 3f;

        Destroy(gameObject, 2);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }
}
