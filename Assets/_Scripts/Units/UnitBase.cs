using Assets._Scripts.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public abstract void SetStats(Stats stats);

    public abstract Task TakeDamage(float dmg, List<ConditionBase> conditions);

    public abstract bool TryMove(Vector2 direction);

    public abstract void LockMovement();

    public abstract void UnlockMovement();

    public abstract void SetupCondtionsBar(Canvas canvas);

    public abstract void Die();
}