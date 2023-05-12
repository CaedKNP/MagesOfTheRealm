using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBase : MonoBehaviour
{
    public string Name { get; set; }

    protected float speed;
    protected float destroyTime;

    protected void SetSpeedDestroyTime(float speed, float destroyTime)
    {
        this.speed = speed;
        this.destroyTime = destroyTime;
    }

    protected Rigidbody2D rb;

    protected void MyAwake()
    {
        rb = GetComponent<Rigidbody2D>(); // pobieramy Rigidbody2D komponent z prefabu
        rb.velocity = transform.right * speed; // ustawiamy prędkość w kierunku "przodu" prefabu
        Invoke("BeforeDelete", destroyTime);
        //Destroy(gameObject, destroyTime);
    }

    protected virtual void BeforeDelete() {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            BeforeDelete();
        }
        //Destroy(collision.gameObject);

        var conditions = new List<Conditions>
        {
            Conditions.Burn
        };

        if (collision.gameObject.CompareTag("Unit"))
        {
            var unit = collision.GetComponent<UnitBase>();

            unit.TakeDamage(conditions, 3, 5, 1);
        }
    }
}
