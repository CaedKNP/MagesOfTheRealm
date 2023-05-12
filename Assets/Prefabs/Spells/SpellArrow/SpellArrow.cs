using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellArrow : SpellBase
{
    protected Animator animator;
    protected void Awake()
    {
        animator = GetComponent<Animator>(); // Zmiana na GetComponent<Animator>()
        SetSpeedDestroyTime(1f, 2f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }
}
