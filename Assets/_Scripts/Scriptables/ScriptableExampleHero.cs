using System;
using UnityEngine;

/// <summary>
/// Create a scriptable hero 
/// </summary>
[CreateAssetMenu(fileName = "SimpleMage")]
public class ScriptableExampleHero : ScriptableExampleUnitBase
{
    public ExampleHeroType HeroType;
}

[Serializable]
public enum ExampleHeroType
{
    SimpleMage = 0
}