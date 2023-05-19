using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    public string Name;
    protected float DMG;
    protected List<ConditionBase> conditions;
    protected float speed;
    protected float destroyTime;

    public void SetSpellStats()
    {
        var spell = ResourceSystem.Instance.GetExampleSpell(Name);
        DMG = spell.DMG;
        conditions = spell.conditions;
        speed = spell.speed;
        destroyTime = spell.destroyTime;
    }
}