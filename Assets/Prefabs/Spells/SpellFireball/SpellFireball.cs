using UnityEngine;

public class SpellFireball : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(1f, 3f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }
}