using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDarkMeteor : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(1.4f, 1.2f); // Nowe wartos dla speed i destroyTime
        base.MyAwake();
    }

    protected override void BeforeDelete()
    {
        //animation.CrossFade("Default", 0.1f); // Wybierz domyślną animację, czas przejścia wynosi 0,1 sekundy
        //rb.simulated = false; // Wyłącz symulację Rigidbody, aby pocisk przestał się poruszać
        //Invoke("DestroyObject", animation.clip.length); // Wywołaj metodę DestroyObject() po zakończeniu drugiej animacji
    }


}
