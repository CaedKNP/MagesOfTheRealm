using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    [SerializeField]
    public float Dmg;
    [SerializeField]
    public List<ConditionBase> Conditions;

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

    //public void SetDmgAndConditions(float dmg, List<ConditionBase> conditions)
    //{
    //    this.Dmg = dmg;
    //    this.Conditions = conditions;
    //}
}