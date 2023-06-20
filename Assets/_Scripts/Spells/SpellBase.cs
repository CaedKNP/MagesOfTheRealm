using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpellBase : MonoBehaviour
{
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected float DMG;
    [SerializeField]
    protected List<ConditionBase> conditions;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float destroyTime;

    protected void SetSpellStats()
    {
        var spell = ResourceSystem.Instance.GetExampleSpell(Name);
        DMG = spell.DMG;
        conditions = spell.conditions;
        speed = spell.speed;
        destroyTime = spell.destroyTime;
    }
}