using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSwordLvl1 : SpellBase
{
    public Animator animator;
    protected void Awake()
    {
        Animation();
    }

    void Animation()
    {
        animator.speed=animator.speed*2.5f;
        animator.enabled = true; // Enable the Animator
        StartCoroutine(WaitForAnimationToEnd());
        ExplosiveDamage();
    }

    IEnumerator WaitForAnimationToEnd()
    {
        // Wait until the animation finishes playing
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        // Animation has finished playing, destroy the object
        Destroy(gameObject);
    }

    private void ExplosiveDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 4.5f);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out UnitBase unit))
                unit.TakeDamage(7, new List<ConditionBase>());
        }
    }
}
