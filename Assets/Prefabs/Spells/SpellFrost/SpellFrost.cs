using UnityEngine;

public class SpellFrost : SpellBase
{
    protected void Awake()
    {
        SetSpeedDestroyTime(8f, 2f); // Nowe wartości dla speed i destroyTime
        base.MyAwake();
    }
}