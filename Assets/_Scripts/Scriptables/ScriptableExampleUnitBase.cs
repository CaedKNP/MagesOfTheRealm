using Assets._Scripts.Utilities;
using UnityEngine;

/// <summary>
/// Keeping all relevant information about a unit on a scriptable means we can gather and show
/// info on the menu screen, without instantiating the unit prefab.
/// </summary>
public abstract class ScriptableExampleUnitBase : ScriptableObject {

    [SerializeField] Stats _stats;
    public Stats BaseStats => _stats;

    // Used in game
    public UnitBase Prefab;
    
    // Used in menus
    public string Description;
    public Sprite MenuSprite;
}