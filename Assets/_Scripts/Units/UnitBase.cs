using Assets._Scripts.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public abstract void SetStats(Stats stats);

    public abstract Task TakeDamage(List<Conditions> conditions, int dmg, float affectTime, int dmgToTake);

    public abstract bool TryMove(Vector2 direction);

    public abstract void LockMovement();

    public abstract void UnlockMovement();

    public abstract void Die();
}