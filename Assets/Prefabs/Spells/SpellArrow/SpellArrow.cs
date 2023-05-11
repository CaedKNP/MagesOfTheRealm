using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellArrow : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(1f, 2f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }
}
