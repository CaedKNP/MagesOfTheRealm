using UnityEngine;
using UnityEngine.UIElements;

public abstract class SpellBase : MonoBehaviour
{
    public string Name { get; set; }

    protected float speed;
    protected float destroyTime;
    public Collider2D caster;

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
        //Invoke("BeforeDelete", destroyTime);
        Destroy(gameObject, destroyTime); 
    }

    protected virtual bool BeforeDelete() { return false; }
}