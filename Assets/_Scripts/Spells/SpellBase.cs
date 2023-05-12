using UnityEngine;

public abstract class SpellBase : MonoBehaviour
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
        Destroy(gameObject, destroyTime);
    }

    protected virtual bool BeforeDelete() { return false; }
}