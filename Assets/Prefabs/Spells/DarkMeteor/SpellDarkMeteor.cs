using Assets._Scripts.Spells;
using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDarkMeteor : SpellProjectileBase
{
    public Animator darkMeteorAnimator; // Komponent Animator dla obiektu darkMeteor 
    private bool hasPlayedAnimation = false; // Flaga, czy animacja została odtworzona 

    protected void Awake()
    {
        MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AttackHandler unit))
        {
            unit.DAMAGE(3, conditions);

            BeforeDestroy();
        }
    }

    public override bool BeforeDestroy()
    {
        StartCoroutine(AnimateTextureChange());
        rb.velocity = Vector2.zero;
        return false;
    }

    private IEnumerator AnimateTextureChange()
    {
        if (!hasPlayedAnimation)
        {
            darkMeteorAnimator.enabled = true; // Włącz Animator 

            //darkMeteorAnimator.Play("YourAnimationName"); // Odtwórz animację o konkretnej nazwie 

            ExplosiveDamage();

            // Poczekaj na zakończenie animacji
            yield return new WaitForSeconds(darkMeteorAnimator.GetCurrentAnimatorStateInfo(0).length);

            // Wyłącz Animator 
            darkMeteorAnimator.enabled = false;

            hasPlayedAnimation = true;

            // Zniszczenie obiektu darkMeteor
            Destroy(gameObject);
        }
    }

    private void ExplosiveDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 4.5f);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out AttackHandler unit))
                unit.DAMAGE(DMG, new List<ConditionBase>());
        }
    }
}