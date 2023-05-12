using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSword : SpellBase
{
    public Animator darkMeteorAnimator; // Komponent Animator dla obiektu darkMeteor
    private bool hasPlayedAnimation = false; // Flaga, czy animacja została odtworzona

    protected void Awake()
    {
        SetSpeedDestroyTime(0f, 0f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }

    protected override void BeforeDelete()
    {
        StartCoroutine(AnimateTextureChange());
        rb.velocity = Vector2.zero;
    }

    private IEnumerator AnimateTextureChange()
    {
        if (!hasPlayedAnimation)
        {
            darkMeteorAnimator.enabled = true; // Włącz Animator
            // Poczekaj na zakończenie animacji
            yield return new WaitForSeconds(darkMeteorAnimator.GetCurrentAnimatorStateInfo(0).length);

            // Wyłącz Animator
            darkMeteorAnimator.enabled = false;
            hasPlayedAnimation = true;
        }

        // Zniszczenie obiektu darkMeteor
        Destroy(gameObject);
    }
}
