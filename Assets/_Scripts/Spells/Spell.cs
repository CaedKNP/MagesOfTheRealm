using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell")]
public class Spell : ScriptableObject
{
    public Sprite image;
    public SpellBase Prefab; //MonoBehaviour
    public bool CastFromHeroeNoStaff = false;
    public int ID;
    public float cooldown;
    public SpellSlot spellSlot;

    //public float Dmg;
    //public List<ConditionBase> Conditions;

    // Used in menus
    public string Name;
    public string Description;

    public SpellBase Cast(Vector3 position, Quaternion rotation)
    {
        var spellInstance = Instantiate(Prefab, position, rotation);
        //spellInstance.SetDmgAndConditions(Dmg, Conditions);
        return spellInstance;
    }
}