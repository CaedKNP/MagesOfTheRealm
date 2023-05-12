using System.Collections;
using UnityEngine;

public class SpellDarkMeteor : SpellBase
{
    public Animator darkMeteorAnimator; // Komponent Animator dla obiektu darkMeteor
    private bool hasPlayedAnimation = false; // Flaga, czy animacja została odtworzona

    protected void Awake()
    {
        SetSpeedDestroyTime(6f, 1.2f); // Nowe wartości dla speed i destroyTime
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

            //darkMeteorAnimator.Play("YourAnimationName"); // Odtwórz animację o konkretnej nazwie

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
