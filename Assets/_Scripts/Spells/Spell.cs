using UnityEngine;

public abstract class Spell
{
    public GameObject Prefab { get; set; }

    public string Name { get; set; }

    public float Cooldown { get; set; }

    public abstract void Cast();
}