using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellThunderBoltInit : MonoBehaviour
{
    float speed = 4f; // prędkość ruchu prefaba

    private Rigidbody2D rb;

    private void Awake()
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
