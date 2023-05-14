using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDarkMeteor : SpellBase
{
    public Animator darkMeteorAnimator; // Komponent Animator dla obiektu darkMeteor 
    private bool hasPlayedAnimation = false; // Flaga, czy animacja została odtworzona 

    protected void Awake()
    {
        SetSpeedDestroyTime(6f, 2.2f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var conditions = new List<ConditionBase>
        {
            new ConditionBase() { Conditions = Conditions.Poison, AffectTime  = 5, AffectOnTick = 1 }
        };

        if (collision.gameObject.TryGetComponent<UnitBase>(out UnitBase unit))
        {
            unit.TakeDamage(3, conditions);

            if (!BeforeDelete())
                Destroy(gameObject);
        }
    }

    protected override bool BeforeDelete()
    {
        StartCoroutine(AnimateTextureChange());
        rb.velocity = Vector2.zero;

        return true;
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
            if (collider.TryGetComponent(out UnitBase unit))
                unit.TakeDamage(2, new List<ConditionBase>());
        }
    }
}