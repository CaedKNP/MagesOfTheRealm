using UnityEngine;

public class SpellFrost : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(2f, 1f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }
}