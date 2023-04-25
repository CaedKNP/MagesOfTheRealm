using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellThunderBolt : MonoBehaviour
{
    float speed = 7f; // prędkość ruchu prefaba
    float targetY;
    public GameObject pfSpell;
    private Rigidbody2D rb;
    public float previousHeight;
    public float timeToDestroy = 0.7f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // pobieramy Rigidbody2D komponent z prefabu
        rb.velocity = transform.right * speed; // ustawiamy prędkość w kierunku "przodu" prefabu
    }

    void Update()
    {
        if (transform.position.y <= previousHeight)
        {
            rb.velocity = Vector2.zero;
            Invoke("DestroySpell", timeToDestroy);
        }
    }

    void DestroySpell()
    {
        // zniszcz prefaba
        Destroy(gameObject);
    }

}
