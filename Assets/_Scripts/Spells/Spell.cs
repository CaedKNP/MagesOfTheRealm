using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell")]
public class Spell : ScriptableObject
{
    /// <summary></summary>
    public string Name;
    /// <summary></summary>
    public string Description;
    /// <summary></summary>
    public Sprite image;
    /// <summary></summary>
    public SpellBase Prefab; //MonoBehaviour

    /// <summary></summary>
    public float DMG;
    /// <summary></summary>
    public List<ConditionBase> conditions;
    /// <summary></summary>
    public float speed;
    /// <summary></summary>
    public float destroyTime;
    /// <summary></summary>
    public Collider2D caster;

    /// <summary></summary>
    public bool CastFromHeroeNoStaff = false;
    /// <summary></summary>
    public float cooldown;
    /// <summary></summary>
    public SpellSlot spellSlot;

    /// <summary>position and rotation to create spell prefab</summary>
    /// <param name="position">Spawn position</param>
    /// <param name="rotation">Spawn rotation</param>
    public void Attack(Vector3 position, Quaternion rotation)
    {
        Instantiate(Prefab, position, rotation);
    }
}