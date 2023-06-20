using Assets._Scripts.Utilities;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for every unit in game
/// </summary>
public abstract class UnitBase : MonoBehaviour
{
    protected Assets._Scripts.Utilities.Stats stats;

    protected bool _canMove = true;
    protected bool _isDead = false;

    protected Component conditionsBar;
    protected ConditionUI _conditionUI;
    /// <summary>
    /// Health bar 
    /// </summary>
    public GameObject healthBarManagerObj;
    protected HealthBarManager healthBar;

    protected Coroutine changeRoutine, burnRoutine, freezeRoutine, slowRoutine, speedUpRoutine, poisonRoutine, armorUpRoutine, armorDownRoutine, hasteRoutine, dmgUpRoutine;
    public abstract bool TryMove(Vector2 direction);

    /// <summary>
    /// Destroy gameObject
    /// </summary>
    public virtual void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        _canMove = true;
        Debug.Log($"{name} is dead");
        _canMove = false;
    }
    /// <summary>
    /// Modify hp of unit and attach condition
    /// </summary>
    /// <param name="dmgToTake">Amount of dmg to be </param>
    /// <param name="conditions">List of condition to be attach to unit</param>
    public virtual void TakeDamage(float dmgToTake, List<ConditionBase> conditions)
    {
        //stats.CurrentHp -= Convert.ToInt32(dmgToTake * stats.Armor); DMG UP
        stats.CurrentHp -= Convert.ToInt32(dmgToTake / stats.Armor); //Armor up

        if (stats.CurrentHp <= 0)
            Die();

        ConditionAffect(conditions);
    }
    /// <summary>
    /// Attach stats structure to unit
    /// </summary>
    /// <param name="stats"></param>
    public virtual void SetStats(Assets._Scripts.Utilities.Stats stats)
    {
        this.stats = stats;
    }

    #region Conditions

    protected virtual void ConditionAffect(List<ConditionBase> conditions)
    {
        if (conditions != null && conditions.Count > 0)
            foreach (ConditionBase condition in conditions)
                Affect(condition);
    }

    protected virtual void Affect(ConditionBase condition)
    {
        switch (condition.Condition)
        {
            case global::Conditions.Burn:

                burnRoutine ??= StartCoroutine(BurnTask(condition));

                break;
            case global::Conditions.Slow:

                slowRoutine ??= StartCoroutine(SlowTask(condition));

                break;
            case global::Conditions.Freeze:

                freezeRoutine ??= StartCoroutine(FreezeTask(condition));

                break;
            case global::Conditions.Poison:

                poisonRoutine ??= StartCoroutine(PoisonTask(condition));

                break;
            case global::Conditions.SpeedUp:

                speedUpRoutine ??= StartCoroutine(SpeedUpTask(condition));

                break;
            case global::Conditions.ArmorUp:

                armorUpRoutine ??= StartCoroutine(ArmorUpTask(condition));

                break;
            case global::Conditions.ArmorDown:

                armorDownRoutine ??= StartCoroutine(ArmorDownTask(condition));

                break;
            case global::Conditions.Haste:

                hasteRoutine ??= StartCoroutine(HasteTask(condition));

                break;
            case global::Conditions.DmgUp:

                dmgUpRoutine ??= StartCoroutine(DmgUpTask(condition));

                break;
            default:
                break;
        }
    }

    protected virtual IEnumerator BurnTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(0);
        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
        {
            stats.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            healthBar.SetHealth(stats.CurrentHp);

            if (stats.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(0);
        burnRoutine = null;
    }

    protected virtual IEnumerator SlowTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(1);

        var end = Time.time + condition.AffectTime;
        var tempSpeed = stats.MovementSpeed;

        stats.MovementSpeed -= stats.MovementSpeed * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(1);
        stats.MovementSpeed = tempSpeed;
        slowRoutine = null;
    }

    protected virtual IEnumerator PoisonTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(2);
        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
        {
            stats.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            healthBar.SetHealth(stats.CurrentHp);

            if (stats.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(2);
        poisonRoutine = null;
    }

    protected virtual IEnumerator FreezeTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(3);

        _canMove = false;

        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(3);
        _canMove = true;
        freezeRoutine = null;
    }

    protected virtual IEnumerator SpeedUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(4);

        var end = Time.time + condition.AffectTime;
        var tempMoveSpeed = stats.MovementSpeed;

        stats.MovementSpeed += stats.MovementSpeed * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(4);
        stats.MovementSpeed = tempMoveSpeed;
        speedUpRoutine = null;
    }

    protected virtual IEnumerator ArmorUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(5);
        var end = Time.time + condition.AffectTime;
        var tempArmor = stats.Armor;

        stats.Armor += stats.Armor * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        stats.Armor = tempArmor;

        _conditionUI.RemoveConditionSprite(5);
        armorUpRoutine = null;
    }

    protected virtual IEnumerator ArmorDownTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(6);

        var end = Time.time + condition.AffectTime;
        var tempArmorDown = stats.Armor;

        stats.Armor -= stats.Armor * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        stats.Armor = tempArmorDown;

        _conditionUI.RemoveConditionSprite(6);
        armorDownRoutine = null;
    }

    protected virtual IEnumerator HasteTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(7);

        var end = Time.time + condition.AffectTime;
        var tempCooldown = stats.CooldownModifier;

        stats.CooldownModifier += stats.CooldownModifier * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        stats.CooldownModifier = tempCooldown;

        _conditionUI.RemoveConditionSprite(7);
        hasteRoutine = null;
    }

    protected virtual IEnumerator DmgUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(8);

        var end = Time.time + condition.AffectTime;
        var tempDmg = stats.DmgModifier;

        stats.DmgModifier += stats.DmgModifier * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        stats.DmgModifier = tempDmg;

        _conditionUI.RemoveConditionSprite(8);
        dmgUpRoutine = null;
    }

    #endregion
}