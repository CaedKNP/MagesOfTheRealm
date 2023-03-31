using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellThunderBoltInit : MonoBehaviour
{
    float speed = 2f; // prędkość ruchu prefaba
    public GameObject pfSpell;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // pobieramy Rigidbody2D komponent z prefabu
        rb.velocity = transform.right * speed; // ustawiamy prędkość w kierunku "przodu" prefabu
        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Vector2 screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0f, 1f));
        Vector2 spawnPosition = new Vector2(transform.position.x, screenTop.y + 1);
        Quaternion spawnRotation = Quaternion.Euler(0f, 0f, -90f); // obrot o 90 stopni wokół osi Y
        GameObject newSpell = Instantiate(pfSpell, spawnPosition, spawnRotation);

        SpellThunderBolt spellThunderBolt = newSpell.GetComponent<SpellThunderBolt>();
        spellThunderBolt.previousHeight = transform.position.y;
    }

}