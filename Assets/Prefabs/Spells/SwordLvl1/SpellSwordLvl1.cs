﻿using System.Collections;
using UnityEngine;

public class SpellSwordLvl1 : SpellBase
{
    [SerializeField]
    float radius;
    [SerializeField]
    Animator animator;
    protected void Awake()
    {
        SetSpellStats();
        Animation();
    }

    void Animation()
    {
        animator.speed *= 2.5f;
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), radius);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out AttackHandler unit))
                unit.DAMAGE(DMG, conditions);
        }
    }
}