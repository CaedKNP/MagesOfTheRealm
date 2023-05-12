using UnityEngine;

public class SpellFrost : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(1.9f, 2f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }
}