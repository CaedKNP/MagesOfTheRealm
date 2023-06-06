using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell")]
public class Spell : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite image;
    public SpellBase Prefab; //MonoBehaviour

    public float DMG;
    public List<ConditionBase> conditions;
    public float speed;
    public float destroyTime;
    public Collider2D caster;

    public bool CastFromHeroeNoStaff = false;
    public float cooldown;
    public SpellSlot spellSlot;

    public void Attack(Vector3 position, Quaternion rotation)
    {
        Instantiate(Prefab, position, rotation);
    }
}