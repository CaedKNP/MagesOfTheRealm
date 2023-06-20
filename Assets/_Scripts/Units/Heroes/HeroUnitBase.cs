using Assets._Scripts.Utilities;
using Assets.Resources.SOs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroUnitBase : UnitBase
{
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    BoxCollider2D collider;
    List<RaycastHit2D> castCollisions = new();

    [SerializeField]
    private stringSO mageNameSO;

    [SerializeField]
    Spell PrimarySpell;
    float primaryCooldownCounter = 0f;

    [SerializeField]
    Spell SecondarySpell;
    float secondaryCooldownCounter = 0f;

    [SerializeField]
    Spell QSpell;
    float QCooldownCounter = 0f;

    [SerializeField]
    Spell ESpell;
    float ECooldownCounter = 0f;

    [SerializeField]
    Spell DashSpell;
    float DashCooldownCounter = 0f;

    StaffRotation spellRotator;

    Component conditionsBar;

    ConditionUI _conditionUI;

    private Animator _anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spellRotator = GetComponentInChildren<StaffRotation>();

        healthBar = FindObjectOfType<HealthBarManager>();
        healthBar.SetMaxHealth(stats.MaxHp);

        _anim = GetComponent<Animator>();

        conditionsBar = gameObject.transform.GetChild(0);

        _conditionUI = conditionsBar.GetComponent<ConditionUI>();
    }

    void FixedUpdate()
    {
        if (!_isDead)
            TryMove();
    }

    public void ChangeMage(string mageName)
    {
        changeRoutine ??= StartCoroutine(WaitAndChange(mageName));
    }

    private IEnumerator WaitAndChange(string mageName)
    {
        yield return new WaitForSeconds(1f);

        this.gameObject.SetActive(false);
        mageNameSO.String = mageName;
        GameManager.Player = UnitManager.Instance.SpawnHero(mageName, this.gameObject.transform.position);
        Destroy(this.gameObject);

        changeRoutine = null;
    }

    #region Movement

    void TryMove()
    {
        if (_canMove)
        {
            Move();
        }
    }

    void Move()
    {
        // If movement input is not 0, try to move
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);
            _anim.CrossFade("Walk", 0, 0);
            //Gliding around walls
            #region Gliding

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!success)
            {
                _ = TryMove(new Vector2(0, movementInput.y));
            }

            #endregion

        }
        else
        {
            if (stats.CurrentHp > 0)
                _anim.CrossFade("Idle", 0, 0);

        }

        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public override bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = collider.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                stats.MovementSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + stats.MovementSpeed * Time.fixedDeltaTime * direction);
                return true;
            }

            return false;
        }
        return false;
    }
    public bool TryMove(Vector2 direction, float speed)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = collider.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                speed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
                return true;
            }

            return false;
        }
        return false;
    }

    #endregion

    public override void TakeDamage(float dmgToTake, List<ConditionBase> conditions)
    {
        base.TakeDamage(dmgToTake, conditions);
        healthBar.SetHealth(stats.CurrentHp);
    }

    #region Conditions

    private void ConditionAffect(List<ConditionBase> conditions)
    {
        if (conditions != null && conditions.Count > 0)
            foreach (ConditionBase condition in conditions)
                Affect(condition);
    }

    private void Affect(ConditionBase condition)
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

    private IEnumerator BurnTask(ConditionBase condition)
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

    private IEnumerator SlowTask(ConditionBase condition)
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

    private IEnumerator PoisonTask(ConditionBase condition)
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

    private IEnumerator FreezeTask(ConditionBase condition)
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

    private IEnumerator SpeedUpTask(ConditionBase condition)
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

    private IEnumerator ArmorUpTask(ConditionBase condition)
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

    private IEnumerator ArmorDownTask(ConditionBase condition)
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

    private IEnumerator HasteTask(ConditionBase condition)
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

    private IEnumerator DmgUpTask(ConditionBase condition)
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

    #region Input

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnPrimaryAttack()
    {
        if (!_isDead)
            if (Time.time > primaryCooldownCounter)
            {
                CastSpell(PrimarySpell);
                primaryCooldownCounter = Time.time + PrimarySpell.cooldown * stats.CooldownModifier;
            }
    }

    void OnSecondaryAttack()
    {
        if (!_isDead)
            if (Time.time > secondaryCooldownCounter)
            {
                CastSpell(SecondarySpell);
                secondaryCooldownCounter = Time.time + SecondarySpell.cooldown * stats.CooldownModifier;
            }
    }

    void OnQSpell()
    {
        if (!_isDead)
            if (Time.time > QCooldownCounter)
            {
                CastSpell(QSpell);
                QCooldownCounter = Time.time + QSpell.cooldown * stats.CooldownModifier;
            }
    }

    void OnESpell()
    {
        if (!_isDead)
            if (Time.time > ECooldownCounter)
            {
                CastSpell(ESpell);
                ECooldownCounter = Time.time + ESpell.cooldown * stats.CooldownModifier;
            }
    }

    void OnDodge()
    {
        if (!_isDead)
            if (Time.time > DashCooldownCounter)
            {
                CastSpell(DashSpell);
                DashCooldownCounter = Time.time + DashSpell.cooldown * stats.CooldownModifier;
            }
    }

    void OnStart()
    {
        if (GameManager.Instance.State == GameState.Hub)
            GameManager.Instance.ChangeState(GameState.ChangeLevel);
    }

    void OnRestart()
    {
        if (GameManager.Instance.State == GameState.Starting)
            GameManager.Instance.ChangeState(GameState.Hub);
    }

    void OnInteraction()
    {
    }

    #endregion

    void CastSpell(Spell spell)
    {
        if (spellRotator != null)
        {
            if (spell.CastFromHeroeNoStaff)
            {
                spell.caster = collider;
                spell.Attack(transform.position, spellRotator.WizandStaffFirePint.transform.rotation);
            }
            else
            {
                spell.caster = collider;
                spell.Attack(spellRotator.WizandStaffFirePint.transform.position, spellRotator.WizandStaffFirePint.transform.rotation);
            }
        }
        else
        {
            Debug.LogWarning("SpellRotator is not assigned!");
        }
    }

    public void HideWand()
    {
        spellRotator.gameObject.active = false;
    }
    public void UnHideWand()
    {
        spellRotator.gameObject.active = true;
    }

    public override void Die()
    {
        if (_isDead)
            return;

        base.Die();
        _anim.CrossFade("Death", 0, 0);
        HideWand();
        GameManager.Instance.ChangeState(GameState.Lose);
    }
}