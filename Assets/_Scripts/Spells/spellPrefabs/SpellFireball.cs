using UnityEngine;

public class SpellFireball : MonoBehaviour
{
    float speed = 1f; // prędkość ruchu prefaba
    Rigidbody2D rb;

    public void Cast()
    {
        throw new System.NotImplementedException();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // pobieramy Rigidbody2D komponent z prefabu
        rb.velocity = transform.right * speed; // ustawiamy prędkość w kierunku "przodu" prefabu
        Destroy(gameObject, 2);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
        //Destroy(collision.gameObject);
    }
}