using UnityEngine;

namespace Assets._Scripts.Spells
{
    /// <summary></summary>
    public class SpellProjectileBase : SpellBase
    {
        protected Rigidbody2D rb;

        protected void MyAwake()
        {
            SetSpellStats();
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;

            if (BeforeDestroy())
                Destroy(gameObject, destroyTime);
        }

        /// <summary>the last function called before the spell prefab is destroyed</summary>
        protected virtual bool BeforeDestroy()
        {
            return true;
        }
    }
}