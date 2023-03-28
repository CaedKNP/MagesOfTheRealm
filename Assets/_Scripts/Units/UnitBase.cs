using Assets._Scripts.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public abstract void SetStats(Stats stats);

    public abstract void Attack();

    public abstract void TakeDamage(int dmg);

    public abstract bool TryMove(Vector2 direction);

    public abstract void LockMovement();

    public abstract void UnlockMovement();

    public abstract void Die();
}