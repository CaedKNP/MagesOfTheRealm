using UnityEngine;

public class SpellFireball : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(8f, 3f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }
}