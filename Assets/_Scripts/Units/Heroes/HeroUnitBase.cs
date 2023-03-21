using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnitBase : UnitBase
{
    private bool _canMove;
    private Stats statistics;
    //private List<Spells> spells;

    private void Awake() => GameManager.OnBeforeStateChanged += OnStateChanged;

    private void OnDestroy() => GameManager.OnBeforeStateChanged -= OnStateChanged;

    private void OnStateChanged(GameState newState)
    {
        if (newState == GameState.Playing) _canMove = true;
    }

    public override void SetStats(Stats stats)
    {
        statistics = stats;
    }

    public override void Attack()
    {
        //Attack implementation
    }

    public override void TakeDamage(int dmg)
    {
        statistics.CurrentHp -= dmg;

        if (statistics.CurrentHp < 0)
            Die();
    }

    public override bool TryMove(Vector2 direction)
    {
        if (_canMove)
        {

        }

        return _canMove;
    }

    public override void LockMovement()
    {
        _canMove = false;
    }

    public override void UnlockMovement()
    {
        _canMove = true;
    }

    public override void Die()
    {
        //Die management
    }
}