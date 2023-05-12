using UnityEngine;

public class SpellFireball : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(5f, 3f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }
}